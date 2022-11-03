using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public float aggroRange = 10f;
    public float speed = 400f;
    public float jump = 0.3f;
    public float jumpOffset = 0.1f;
    public float jumpNodeReq = 0.8f;
    public float nextWaypointCheck = 1f;

    protected float[] damageMult = new float[3];
    [Header("Combat")]
    //public GameObject attackCir;
    [SerializeField] protected float hp;
    protected float deathAnimTime;
    protected float pathRefreshTime;

    protected bool flying;
    protected bool grounded;
    protected bool chaser;
    protected bool hasFacing;
    protected int currentWaypoint;

    protected bool dying;
    [SerializeField] protected bool attacking;
    [SerializeField] protected bool counterable;
    [SerializeField] protected bool flinching;
    protected float flinchCheck;
    protected float flinchThreshold;
    protected float flinchTime;

    protected Transform player;
    protected Transform startPos;
    protected Transform target;
    protected Animator anim;
    protected LayerMask playerLayer;
    protected Path path;
    protected Seeker seeker;
    protected Rigidbody2D rb2d;
    protected Attack[] attacks;

    protected const float VERT_COLL_DIST = 0.34f;
    public class Attack
    {
        public float windUp;
        public Transform attackPos;
        public float rad;
        public int attackType;
        public float damage;
        public bool counterable;
        public bool melee;

        public Attack(float windUp, Transform attackPos, float rad, int attackType, float damage, bool counterable, bool melee)
        {
            this.windUp = windUp;
            this.attackPos = attackPos;
            this.rad = rad;
            this.attackType = attackType;
            this.damage = damage;
            this.counterable = counterable;
            this.melee = melee;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomStart();
    }
    protected void CustomStart()
    {
        dying = false;
        attacking = false;
        flinching = false;
        player = GameObject.Find("player").GetComponent<Transform>();
        startPos = transform;
        target = player;
        anim = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        pathRefreshTime = 0.25f;

        currentWaypoint = 0;

        startPos = transform;

        flinchCheck = 0f;
        flinchTime = 0f;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        CustomFixedUpdate();
    }
    protected void CustomFixedUpdate()
    {
        
        if (hp <= 0f && !dying)
            Die();
        else if (!flinching)
        {
            
            //isangry checks aggro distance, chaser is false if the enemy is turret type
            if (IsAngry() && chaser)
            {
                target = player;
                float dist = Vector2.Distance(rb2d.position, target.position);
                if (dist < nextWaypointCheck && (grounded || flying) && !attacking)
                    Combat();
                else if (!attacking)
                    Pathfinding();
            }
            else if (IsAngry() && !chaser)
            {

            }
            else
            {
                target = startPos;
                PathfindingIdle();
            }
        }
        else
        {
            if (flinchTime < 0.5f)
                flinchTime += Time.deltaTime;
            else
            {
                flinchTime = 0f;
                flinching = false;
            }
        }
    }

    protected void Combat()
    {
        //if close enough to the player, attack
        float dist = Vector2.Distance(rb2d.position, target.position);
        if (dist < 1.5f)
        {
            if (chaser)
            {
                attacking = true;
                //pick a random attack
                System.Random random = new System.Random();
                int num = random.Next(0, attacks.Length);
                //start timer to make the attack
                if (attacks[num].melee)
                    StartCoroutine(AttackDamageTimer(attacks[num]));
                //play the animation
            }
            else
            {

            }
            
        }


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

        if (hasFacing)
        {
            if (rb2d.velocity.x > 0.05f)
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (rb2d.velocity.x < -0.05f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
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
    public void TakeDamage(float damage, int type)
    {
        hp -= damage * damageMult[type];
        flinchCheck += damage * damageMult[type];
        if (flinchCheck >= flinchThreshold)
        {
            flinchCheck = 0f;
            flinching = true;
        }
    }
    protected void Countered()
    {
        flinching = true;
    }
    protected void Die()
    {
        dying = true;
        //play death animation
        //anim.Play("death");
        //destroy game object
        StopAllCoroutines();
        StartCoroutine(DeleteMe());
    }

    protected IEnumerator DeleteMe()
    {
        yield return new WaitForSeconds(deathAnimTime);
        Destroy(this.gameObject);
    }
    IEnumerator AttackDamageTimer(Attack attack)
    {
        
        Debug.Log("attack");
        //windUp is the amount of time between the start of the animation and when it should deal damage
        yield return new WaitForSeconds(attack.windUp);
        attacking = false;
        //deal damage to player in the radius
        Vector2 actualPos = transform.position;
        // circle for testing
        //GameObject circle = GameObject.Instantiate(attackCir, actualPos, attackCir.transform.rotation);
        //circle.transform.localScale = new Vector3(attack.rad, attack.rad, 1f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(actualPos, attack.rad, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerController pc = hit.gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(attack.damage, attack.attackType, transform.position, gameObject.name);
        }
    }
}
