using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyAI : MonoBehaviour
{
    //temp hack to show attacking
    public GameObject attackEffect;
    [Header("Pathfinding")]
    public float aggroRange = 10f;
    public float speed = 400f;
    public float jump = 0.5f;
    public float jumpOffset = 0.1f;
    public float jumpNodeReq = 0.8f;
    public float nextWaypointCheck = 1f;
    public LayerMask ignoreLayers;
    Vector3 localScale;

    protected float[] damageMult = new float[3];
    [Header("Combat")]
    //public GameObject attackCir;
    [SerializeField] protected float hp;
    public EnemyAttackEffect shine;
    protected float deathAnimTime;
    protected float pathRefreshTime;

    protected bool flying;
    protected bool grounded;
    protected bool chaser;
    protected bool turret;
    protected bool hasFacing;
    protected int facing;
    protected int currentWaypoint;

    protected bool dying;
    [SerializeField] protected bool attacking;
    [SerializeField] protected bool counterable;
    [SerializeField] protected bool flinching;
    [SerializeField] protected float range;
    protected float flinchCheck;
    protected float flinchThreshold;

    protected Transform player;
    protected Transform startPos;
    protected Transform target;
    protected Animator anim;
    protected LayerMask playerLayer;
    protected Path path;
    protected Seeker seeker;
    protected Rigidbody2D rb2d;
    protected Attack[] meleeAttacks;
    public GameObject[] rangedAttacks;
    public RoomControl room;

    protected const float VERT_COLL_DIST = 0.34f;
    public class Attack
    {
        public float windUp;
        public Vector2 attackPos;
        
        public float rad;
        public float attackPushForce;
        public int attackType;
        public float damage;
        public bool counterable;
        public string animName;

        public Attack(float windUp, Vector2 attackPos, float rad, int attackType, float damage, bool counterable, float attackPushForce, string animName)
        {
            this.windUp = windUp;
            this.attackPos = attackPos;
            this.rad = rad;
            this.attackType = attackType;
            this.damage = damage;
            this.counterable = counterable;
            this.attackPushForce = attackPushForce;
            this.animName = animName;
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        CustomStart();
    }

    private void Awake()
    {
        localScale = gameObject.transform.localScale;
    }
    protected void CustomStart()
    {
        facing = 1;
        dying = false;
        attacking = false;
        flinching = false;
        player = GameObject.Find("player").GetComponent<Transform>();
        startPos = transform;
        target = player;
        anim = gameObject.GetComponentInChildren<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        pathRefreshTime = 0.25f;

        currentWaypoint = 0;

        startPos = transform;

        flinchCheck = 0f;

        InvokeRepeating("UpdatePath", 0f, pathRefreshTime);
        InvokeRepeating("UpdateIdlePos", 0f, 3f);

        EnemySpecificStart();
    }
    abstract protected void EnemySpecificStart();
    /*
     * example enemy
    {
        flying = false;
        grounded = false;
        chaser = true;
        hasFacing = true;

        hp = 100f;
        flinchThreshold = hp / 2;

        attacks = new Attack[] {new Attack(1f, transform, 1.5f, 0, 5f, true),
                                new Attack(2f, transform, 1.5f, 0, 20f, false)};
        damageMult = new float[] { 1f, 1f, 1f };
    }*/

    protected void FixedUpdate()
    {
        if (!flinching && !dying)
        {
            if (hasFacing)
            {
                if (rb2d.velocity.x > 0 || player.transform.position.x > transform.position.x)
                {
                    facing = 1;
                    Vector3 newScale = localScale;
                    newScale.x = localScale.x;
                    gameObject.transform.localScale = newScale;
                }
                else if (rb2d.velocity.x < -0 || player.transform.position.x < transform.position.x)
                {
                    facing = -1;
                    Vector3 newScale = localScale;
                    newScale.x = -localScale.x;
                    gameObject.transform.localScale = newScale;
                }
            }
            //isangry checks aggro distance, chaser is true when melee only
            if (IsAngry() && chaser)
            {
                target = player;
                float dist = Vector2.Distance(rb2d.position, target.position);
                if (dist < nextWaypointCheck && (grounded || flying) && !attacking)
                    MeleeCombat();
                else if (!attacking)
                    Pathfinding();
            }
            else if (IsAngry() && !chaser)
            {
                target = player;
                float dist = Vector2.Distance(rb2d.position, target.position);
                if (InRange())
                {
                    Vector2 dir = ((Vector2)target.position - rb2d.position).normalized;
                    RaycastHit2D hit = Physics2D.Raycast(rb2d.position, dir, range, ~ignoreLayers);
                    
                    if (hit.collider.gameObject.name == "player")
                    {//no obstacles between enemy and player, shoot
                        if (!attacking)
                            RangedCombat();
                    }
                    else if (!turret && !attacking)//obstacle hit, do pathfinding
                        Pathfinding();
                    else
                    {
                        //idle
                    }
                }
                else if (!turret && !attacking)
                { //not in range, not turret, not attacking: move closer
                    Pathfinding();
                }
            }
            else
            {
                target = startPos;
                PathfindingIdle();
            }
        }
    }

    protected void MeleeCombat()
    {
        if (attacking) return;
        anim.SetBool("walk", true);
        attacking = true;
        //pick a random attack
        System.Random random = new System.Random();
        int num = random.Next(0, meleeAttacks.Length);
        //play the animation
        anim.Play(meleeAttacks[num].animName);
        //start timer to make the attack
        StartCoroutine(AttackDamageTimer(meleeAttacks[num]));
        
    }
    protected void RangedCombat()
    {
        attacking = true;
        System.Random random = new System.Random();
        int num = random.Next(0, rangedAttacks.Length);
        //play the animation
        anim.SetTrigger("shoot");
        StartCoroutine(RangedAttack(rangedAttacks[num]));
    }
    protected void UpdatePath()
    {
        if (IsAngry() && seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }
    protected void Pathfinding()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
            return;
        Vector2 feetPos = transform.position;
        feetPos.y -= VERT_COLL_DIST;
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        grounded = Physics2D.OverlapCircle(feetPos, VERT_COLL_DIST, groundLayer);
            //Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + jumpOffset);
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = new Vector2(dir.x * speed * Time.deltaTime, 0f);

        anim.SetBool("walk", true);

        //jump if grounded, not a flying enemy, and needs to jump to reach target
        if (!flying && grounded)
        {
            if (dir.y > jumpNodeReq)
                rb2d.AddForce(Vector2.up * speed * jump);
        }

        rb2d.AddForce(force);
        float dist = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointCheck)
            currentWaypoint++;

        
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;    
        }
    }
    protected void IdlePos()
    {

    }
    protected void PathfindingIdle()
    {
        //do pathfinding when idle
    }
    protected bool IsAngry()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return (distance < aggroRange) ? true : false;
    }
    protected bool InRange()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return (distance < range) ? true : false;
    }
    public void TakeDamage(float damage, int type)
    {
        hp -= damage * damageMult[type];
        flinchCheck += damage * damageMult[type];
        if (hp <= 0 && !dying)
            Die();
        else if (flinchCheck >= flinchThreshold)
        {

            StartCoroutine(Flinching(0.75f + PlayerStats.enemyFlinchMod));
        }
    }
    protected void Countered()
    {
        StartCoroutine(Flinching(1.25f + PlayerStats.enemyFlinchMod));
    }
    protected void Die()
    {
        dying = true;
        //play death animation
        anim.SetTrigger("die");
        //destroy game object
        if (!room.cleared)
            room.EnemyKilled();
        StopAllCoroutines();
        StartCoroutine(DeleteMe());
    }

    protected IEnumerator DeleteMe()
    {
        yield return new WaitForSeconds(0.2f);
        deathAnimTime = anim.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(deathAnimTime);
        Destroy(this.gameObject);
    }
    IEnumerator AttackDamageTimer(Attack attack)
    {
        float animLength = anim.GetCurrentAnimatorClipInfo(0).Length;

        if (!attack.counterable)
            shine.PlayEffect();
        
        yield return new WaitForSeconds(attack.windUp);
        //windUp is the amount of time between the start of the animation and when it should deal damage

        Vector2 offset = attack.attackPos;
        if (hasFacing)
            offset.x *= facing;
        Vector2 actualPos = (Vector2)transform.position + offset;

        GameObject effect = Instantiate(attackEffect, actualPos, Quaternion.identity);
        effect.GetComponent<AttackEffectScript>().InitEffect(attack.attackType, attack.rad);
        //check for player
        Collider2D[] hits = Physics2D.OverlapCircleAll(actualPos, attack.rad, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerController pc = hit.gameObject.GetComponent<PlayerController>();
            //if player is countering and attack is counterable
            if (attack.counterable && pc.CheckCounter())
            {
                Countered();
                pc.CounterAttack();
            }
            else
                pc.TakeDamage(attack.damage, attack.attackType, actualPos, attack.attackPushForce);
        }
        yield return new WaitForSeconds(animLength);
        attacking = false;
    }
    private IEnumerator RangedAttack(GameObject attack)
    {
        float animLength = anim.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(animLength);
        Vector2 dir = (target.position - transform.position).normalized;
        GameObject p = Instantiate(attack, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>().Shoot(dir);
        attacking = false;
        
    }

    private IEnumerator Flinching(float wait)
    {
        flinchCheck = 0f;
        flinching = true;
        anim.SetTrigger("flinch");
        yield return new WaitForSeconds(wait);
        flinching = false;
    }
}
