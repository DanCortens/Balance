using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : LivingEntity
{
    public GameObject attackEffect;

    //public variables
    //movement
    public Transform groundCheck;
    public Transform head;
    public Transform knees;
    private float hookSpeed;
    private float hookSpeedMax;
    private float hookSpeedChange = 5f;
    //combat
    public Transform attacks;
    public Transform sAttack;
    public Transform lAttack;
    public Transform aoeAttack;
    private float facing;

    //private variables

    private bool startJump;
    private bool stoppedJump;
    private bool canDoubleJ;
    [SerializeField] private bool wallGrab;
    [SerializeField] private bool dashing;
    private bool pushing;
    private bool canAttack;
    private bool isAttacking;
    [SerializeField] private bool countering;
    private bool usingDark;
    private bool upHeld;
    private bool downHeld;
    public bool hooked;

    private float coyote;
    private float wallCoyote;
    private float wallDir;
    private float marioTime;
    private float sideJTime;
    private float horzM;
    private float balance;
    private float darkAff;
    private float lightAff;
    private Rigidbody2D rb2d;
    public Animator anim;
    private LayerMask groundLayer;
    private LayerMask enemyLayer;
    private LayerMask interactLayer;
    private LayerMask hookLayer;
    private InputAction moveInput;

    //constants
    private const float JUMP_SPD = 6.4f;
    private const float MARIO_MAX = 0.18f;
    private const float SPEED = 5f;
    private const float DASH = 2.5f;
    private const float COYO_MAX = 0.15f;
    private const float VERT_COLL_DIST = 0.15f;
    private const float HORIZ_COLL_DIST = 0.27f;
    private const float SMALL_ATTACK_RAD = 0.5f;
    private const float LARGE_ATTACK_RAD = 1f;
    private const float AOE_ATTACK_RAD = 1.5f;
    private const float INTERACT_RAD = 1f;
    private new const int MAX_HP_BASE = 100;
    public const float HOOK_DIST = 2f;

    private AttStackScript attStack;
    private EnemyPuppeteer puppeteer;
    private ChainHinge chainHinge;
    private Transform nearestHook;
    private Transform hookPoint;
    
    Dictionary<string, PlayerAttacks.Attack> groundAttacks;
    Dictionary<string, PlayerAttacks.Attack> airAttacks;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        attStack = new AttStackScript();
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start(){

        facing = 1f;
        puppeteer = GameObject.FindObjectOfType<EnemyPuppeteer>();
        chainHinge = GameObject.FindObjectOfType<ChainHinge>();
        startJump = false;
        canDoubleJ = false;
        dashing = false;
        stoppedJump = true;
        hooked = false;
        anim.SetBool("grounded", false);
        wallGrab = false;
        groundLayer = LayerMask.GetMask("Ground");
        enemyLayer = LayerMask.GetMask("Enemy");
        interactLayer = LayerMask.GetMask("Interact");
        hookLayer = LayerMask.GetMask("Hook");
        sideJTime = 0f;
        anim.SetBool("canAttack", true);
        isAttacking = false;
        countering = false;
        takingDamage = false;
        usingDark = false;
        if (PlayerPrefs.HasKey("worldBalance"))
            puppeteer.worldBalance = PlayerPrefs.GetFloat("worldBalance");
        else
            puppeteer.worldBalance = 0f;

        darkAff = 1f;
        lightAff = 1f;

        groundAttacks = PlayerAttacks.SetGroundAttacks();
        airAttacks = PlayerAttacks.SetAirAttacks();
        CurrHp = PlayerPrefs.HasKey("currHP") ? PlayerPrefs.GetInt("currHP") : MAX_HP_BASE;
    }

    //new input listeners
    void OnHorizontal(InputValue value)
    {
        if (value.Get<float>() > 0f)
            facing = 1f;
            
        else if (value.Get<float>() < 0f)
            facing = -1f;


        if (sideJTime <= 0f && !isAttacking)
        {
            horzM = value.Get<float>();

        }
        else horzM = 0f;
            

    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && !isAttacking)
        {
            //Calls jump audio
            if ((anim.GetBool("grounded") || coyote < COYO_MAX))
            {
                FindObjectOfType<AudioManager>().Play("Jump");
                anim.SetTrigger("jump");
                coyote = COYO_MAX;
                startJump = true;
                stoppedJump = false;
                marioTime = 0f;

            }
            else if (wallGrab || wallCoyote < COYO_MAX)
            {
                FindObjectOfType<AudioManager>().Play("Jump");
                rb2d.velocity = new Vector2(Mathf.Abs(horzM) * wallDir * SPEED, JUMP_SPD * 1.4f);
                wallGrab = false;
                sideJTime = 0.1f;
            }
            else if (PlayerStats.hasChain && (nearestHook = GetClosestHook())!= null)
            {
                FindObjectOfType<AudioManager>().Play("Chain");
                chainHinge.Hook(nearestHook);
                hookSpeed = rb2d.velocity.x;
                hookSpeedMax = Mathf.Abs(hookSpeed);
                hooked = true;
                canDoubleJ = true;
            }
            else if (PlayerStats.hasDoubleJ && canDoubleJ)
            {
                FindObjectOfType<AudioManager>().Play("Jump");
                coyote = COYO_MAX;
                startJump = true;
                stoppedJump = false;
                marioTime = 0f;
            }
        }
        else 
        {
            if (!stoppedJump)
                stoppedJump = true;

            if (hooked)
            {
                hooked = false;
                chainHinge.Unhook();
            }
                
        }
    }
    private Transform GetClosestHook()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, HOOK_DIST, hookLayer);
        Collider2D target = (hits.Length > 0) ? hits[0] : null;
        foreach (Collider2D h in hits)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, h.transform.position))
                target = h;
        }
        return (target is null) ? null : target.transform;
    }
    void OnDash(InputValue value)
    {
        if (value.isPressed && anim.GetBool("grounded"))
        {
            dashing = true;
            Debug.Log("dashing");
        }
        else if (!value.isPressed)
        {
            dashing = false;
        }
    }

    void OnSwap(InputValue value)
    {
        usingDark = (usingDark) ? false : true;
        //remove, replace with animation + ui change
        string say = (usingDark) ? "Switching to dark" : "Switching to light";
        Debug.Log(say);
    }

    void OnPhysical(InputValue value)
    {
        //Calls Strike audio
        attStack.stored[(int)(AttStackScript.Inputs.PAttack)] = true;
        if (rb2d.velocity.magnitude > 0f)
            attStack.stored[(int)(AttStackScript.Inputs.Forward)] = true;
        if (upHeld)
            attStack.stored[(int)(AttStackScript.Inputs.Up)] = true;
        else if (downHeld)
            attStack.stored[(int)(AttStackScript.Inputs.Down)] = true;
    }
    void OnMagical(InputValue value)
    {
        if (PlayerStats.hasLantern)
        {
            attStack.stored[(int)(AttStackScript.Inputs.MAttack)] = true;
            if (rb2d.velocity.magnitude > 0f)
                attStack.stored[(int)(AttStackScript.Inputs.Forward)] = true;
            if (upHeld)
                attStack.stored[(int)(AttStackScript.Inputs.Up)] = true;
            else if (downHeld)
                attStack.stored[(int)(AttStackScript.Inputs.Down)] = true;
        }
    }
    void OnCounter(InputValue value)
    {
        attStack.stored[(int)(AttStackScript.Inputs.Counter)] = true;
    }
    void OnVertical(InputValue value)
    {
        if (value.isPressed)
        {
            if (value.Get<float>() > 0)
            {
                upHeld = true;
                downHeld = false;
            }
                
            else
            {
                downHeld = true;
                upHeld = false;
            }
                
        }
            
        else
        {
            upHeld = false;
            downHeld = false;
        }
            

    }

    void OnInteract(InputValue val)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, INTERACT_RAD, interactLayer);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("interact with "+hit.gameObject.name);
            hit.GetComponent<Interactable>().Interact();
        }
    }

    private void Update()
    {
        /* austin: In general, this is an easy but unoptimized way of checking for player character death. We should make checks every time the player takes damage or changes HP via events
         * because constantly polling for things like this every frame has the potential to fuck framerates on lower spec devices. 
         */

        //if (true) //check for paused game state{}
        //else if (true) //check for cinematic game state{}
        //else
        {
            string result = attStack.Update(Time.deltaTime);
            //check for combat execution
            if (result != "waiting")
            {
                attStack.Reset();
                if (anim.GetBool("canAttack") && result != "none")
                {
                    Combat(result);
                }


            }
            //if the player is not in an attack or damage animation, can make a new attack
            if (!isAttacking && !takingDamage)
            {
                anim.SetBool("canAttack", true);
            }
        }
        
    }
    void FixedUpdate()
    {
        //if (true) //check for paused game state{}
        //else if (true) //check for cinematic game state{}
        //else
        if (!takingDamage)
        {
            Movement();
        }
    }
    private Vector2 SwingVel(Vector2 dirToPoint)
    {
        float angle = Vector2.Angle(dirToPoint, transform.up);
        Vector2 swing = rb2d.velocity;
        if (horzM == 0f)
        {
            swing = new Vector2(swing.x * 0.9f, swing.y + 0.1f);
        }
        else
        {
            Vector2 perpDir = (horzM > 0f) ? new Vector2(-dirToPoint.y, dirToPoint.x) :
                                             new Vector2(dirToPoint.y, -dirToPoint.x);
            Debug.DrawLine(transform.position, (Vector2)transform.position + perpDir);
            float slowByPercent = Mathf.Clamp((angle - 70f) / 90f, 0f, 1f);
            swing = perpDir * Mathf.Abs(horzM * SPEED);
            swing *= slowByPercent;
        }

        
            
        
        //Debug.Log(slowByPercent);
        //rb2d.velocity = new Vector2(horzM * SPEED * slowByPercent, rb2d.velocity.y / slowByPercent);

        return swing;
    }

    private void Movement()
    {
        float d = (dashing) ? DASH : 0f;

        if (horzM != 0) anim.SetBool("walk", true); else anim.SetBool("walk", false);
        //horizontal
        if (sideJTime <= 0f && !isAttacking && !hooked)
        {
            rb2d.velocity = new Vector2(horzM * (SPEED + d), rb2d.velocity.y);
        }
        else if (hooked)
        {
            chainHinge.UpdateChain();
            Vector2 dirToPoint = transform.position - nearestHook.position;
            rb2d.velocity = SwingVel(dirToPoint);
            if (dirToPoint.magnitude > HOOK_DIST)
            {
                transform.position = (Vector2)nearestHook.position + ((dirToPoint).normalized * HOOK_DIST);
            }
        }
        else
            sideJTime -= Time.deltaTime;

        //raycast for terrain for wall jumping
        Debug.DrawRay(head.position, transform.right * horzM, Color.red);
        RaycastHit2D hitH = Physics2D.Raycast(head.position, transform.right * horzM, HORIZ_COLL_DIST,groundLayer.value);
        RaycastHit2D hitK = Physics2D.Raycast(knees.position, transform.right * horzM, HORIZ_COLL_DIST, groundLayer.value);
        if (hitH || hitK)
        {       
            wallDir = (horzM > 0) ? -1f : 1f;
            wallGrab = true;
            wallCoyote = 0f;
        }
        else
            wallGrab = false;


        //vertical
        //check for floor
        anim.SetBool("grounded", Physics2D.OverlapCircle(groundCheck.position, VERT_COLL_DIST, groundLayer));
        
        //update coyote timer, can't wallgrab when on the ground
        if (anim.GetBool("grounded"))
        {
            coyote = 0f;
            wallGrab = false;
            canDoubleJ = true;
        }
            
        else
        {
            coyote = Mathf.Clamp(coyote + Time.deltaTime, 0f, COYO_MAX);
            wallCoyote = Mathf.Clamp(wallCoyote + Time.deltaTime, 0f, COYO_MAX);
            dashing = false;
        }
            
        if (!isAttacking)
        {
            //jump/"mario" jump
            if (startJump || !stoppedJump)
            {
                startJump = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, JUMP_SPD);
                //update "mario" jump time
                marioTime += Time.deltaTime;
                if (marioTime > MARIO_MAX)
                    stoppedJump = true;
            }
        }

        //facing stuff
        if (horzM > 0f)
        {
            //facing right
            /*attacks.transform.position = new Vector2(transform.position.x + 0.75f, attacks.transform.position.y);
            aoeAttack.transform.position = new Vector2(transform.position.x, aoeAttack.transform.position.y);*/
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x = 0.5f;
            gameObject.transform.localScale = newScale;
        }
        else if (horzM < 0f)
        {
            //facing left
            /*attacks.position = new Vector2(transform.position.x - 0.75f, attacks.transform.position.y);
            aoeAttack.position = new Vector2(transform.position.x, aoeAttack.transform.position.y);*/
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x = -0.5f;
            gameObject.transform.localScale = newScale;
        }

            
    }

    private void Combat(string result)
    {
        isAttacking = true;
        Debug.Log(result);
        if (anim.GetBool("grounded"))
        {
            if (result == "counter")
            {
                StartCoroutine(Countering());
            }
            else
            {
                
                StartCoroutine(AttackDamageTimer(0, groundAttacks[result]));
               
                StartCoroutine(NotAttackingTimer(groundAttacks[result].animTime));
                float change = (usingDark) ? groundAttacks[result].balChange * (darkAff + PlayerStats.damageDoneMod[1]) 
                                           : -groundAttacks[result].balChange * (lightAff + PlayerStats.damageDoneMod[2]);
                puppeteer.worldBalance = Mathf.Clamp(puppeteer.worldBalance + change, -100f, 100f);
                string animName = (groundAttacks[result].attackType[0] == 0) ? result : 
                                  (usingDark) ? result+ "Dark" : result+ "Light";
                anim.SetTrigger(animName);
            }
        }
        else
        {
            //not grounded
            if (result == "counter")
            {
                StartCoroutine(Countering());
            }
            else
            {
                StartCoroutine(AttackDamageTimer(0, airAttacks[result]));
                StartCoroutine(NotAttackingTimer(airAttacks[result].animTime));
                float change = (usingDark) ? airAttacks[result].balChange * (darkAff + PlayerStats.damageDoneMod[1])
                                           : -airAttacks[result].balChange * (lightAff + PlayerStats.damageDoneMod[2]);
                puppeteer.worldBalance = Mathf.Clamp(puppeteer.worldBalance + change, -100f, 100f);
                string animName = (airAttacks[result].attackType[0] == 0) ? result+ "Air" :
                                  (usingDark) ? result + "AirDark" : result + "AirLight";
                anim.SetTrigger(animName);
            }
        }
        
    }
    public new void TakeDamage(float damage, int type, Vector2 enemyDir, float pushForce)
    {
        base.TakeDamage(damage, type, enemyDir, pushForce);
        Vector2 pushDir = ((Vector2)transform.position - enemyDir);
        rb2d.velocity = pushDir.normalized * (FLINCH_DIST * pushForce);
        StartCoroutine(Flinching());
        FindObjectOfType<CameraCont>().StartShake(pushDir, pushForce * 0.2f);
    }

        public bool IsCountering()
    {
        return countering;
    }



    private void OnDrawGizmos()
    {
        //debugging gizmos; remove from final build?
        Gizmos.DrawSphere(groundCheck.position, VERT_COLL_DIST);
        Gizmos.DrawLine(head.position, head.position + (transform.right * HORIZ_COLL_DIST));
        Gizmos.DrawWireSphere(sAttack.position, SMALL_ATTACK_RAD);
        Gizmos.DrawWireSphere(lAttack.position, LARGE_ATTACK_RAD);
        Gizmos.DrawWireSphere(aoeAttack.position, AOE_ATTACK_RAD);
        
    }

    private IEnumerator AttackDamageTimer(int currHit, PlayerAttacks.Attack attack)
    {
        //windUp is the amount of time between the start of the animation and when it should deal damage
        yield return new WaitForSeconds(attack.windUp[currHit]);
        //deal damage to all enemies in the radius
        FindObjectOfType<AudioManager>().Play("Strike");
        Vector2 actualPos = transform.position;
        actualPos += new Vector2(attack.attackPos.x * facing, attack.attackPos.y);
        GameObject newEffect = Instantiate(attackEffect, actualPos, Quaternion.identity);
        newEffect.GetComponent<AttackEffectScript>().InitEffect(attack.GetType(currHit,usingDark), attack.rad);
        Collider2D[] hits = Physics2D.OverlapCircleAll(actualPos, attack.rad, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("hit enemy!");
            hit.gameObject.GetComponent<EnemyAI>().TakeDamage(attack.damage[currHit], attack.GetType(currHit, usingDark));
        }
        if (currHit < attack.hits - 1)
            StartCoroutine(AttackDamageTimer(currHit+1, attack));
    }
    private IEnumerator NotAttackingTimer(float wait)
    {

        yield return new WaitForSeconds(wait);
        isAttacking = false;
    }
    private IEnumerator Flinching()
    {
        isAttacking = false;
        yield return new WaitForSeconds(0.25f * (1f - PlayerStats.flinchMod));
        takingDamage = false;
    }
    private IEnumerator Countering()
    {
        countering = true;
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        countering = false;
        StartCoroutine(NotAttackingTimer(0.5f));
    }


    [ContextMenu("deal 5 damage")]
    protected void damageTest()
    {
        CurrHp = CurrHp - 5;
    }
    public bool CheckCounter()
    {
        return countering;
    }
    private void OnDestroy()
    {
        PlayerStats.ResetUnsavedUpgrades();
    }
}
