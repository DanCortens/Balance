using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : LivingEntity
{
    /*prolly best to seperate stuff (attacking, stats, movement) into separate scripts later on*/

    //public variables
    //movement
    public Transform groundCheck;
    public Transform head;
    public Transform knees;
    //combat
    public Transform attacks;
    public Transform sAttack;
    public Transform lAttack;
    public Transform aoeAttack;

    //private variables
    
    [SerializeField] private bool grounded;
    private bool startJump;
    private bool stoppedJump;
    [SerializeField] private bool wallGrab;
    [SerializeField] private bool dashing;
    private bool pushing;
    private bool canAttack;
    private bool isAttacking;
    private bool countering;
    private bool takingDamage;
    private bool usingDark;
    private bool upHeld;
    private bool downHeld;
    private bool hooked;

    private float coyote;
    private float wallCoyote;
    private float wallDir;
    private float marioTime;
    private float sideJTime;
    private float horzM;
    private float balance;
    private float balance;
    private float darkAff;
    private float lightAff;
    private Rigidbody2D rb2d;
    private Animator anim;
    private LayerMask groundLayer;
    private LayerMask enemyLayer;
    private LayerMask interactLayer;
    private LayerMask hookLayer;
    private InputAction moveInput;

    [SerializeField]
    private int currHP;
    public int currHp
    {
        get { return currHp; }
        set
        {
            if (currHp == value) return;
            currHp = value;
            onHPChanged?.Invoke();
        }
    }

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
    private const int MAX_HP_BASE = 100;
    private const float FLINCH_DIST = 15f;
    private const float HOOK_DIST = 1f;

    private AttStackScript attStack;
    public EnemyPuppeteer puppeteer;
    private Transform nearestHook;
    private Transform hookPoint;
    
    Dictionary<string, PlayerAttacks.Attack> groundAttacks;
    Dictionary<string, PlayerAttacks.Attack> airAttacks;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attStack = new AttStackScript();
        startJump = false;
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
        setCurrHP((PlayerPrefs.HasKey("currHP")) ? PlayerPrefs.GetInt("currHP") : MAX_HP_BASE);

        //Event handling
        onHPChanged += CheckHP;
    }

    //new input listeners
    void OnHorizontal(InputValue value)
    {
        if (sideJTime <= 0f && !isAttacking)
            horzM = value.Get<float>();

    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && !isAttacking)
        {
            if ((anim.GetBool("grounded") || coyote < COYO_MAX))
            {
                coyote = COYO_MAX;
                startJump = true;
                stoppedJump = false;
                marioTime = 0f;
            }
            else if (wallGrab || wallCoyote < COYO_MAX)
            {
                rb2d.velocity = new Vector2(Mathf.Abs(horzM) * wallDir * SPEED, JUMP_SPD * 1.4f);
                wallGrab = false;
                sideJTime = 0.1f;
            }
            else if (PlayerStats.hasChain && (nearestHook = GetClosestHook())!= null)
            {
                hookPoint = nearestHook;
                hooked = true;
            }
        }
        else 
        {
            if (!stoppedJump)
                stoppedJump = true;
            if (hooked)
            {
                hooked = false;
                hookPoint = null;
            }
                
        }
    }
    private Transform GetClosestHook()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, HOOK_DIST, hookLayer);
        return null;
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
        if (currHP <= 0) 
        {
            //game over
        }

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if colliding with an enemy
        if (collision.collider.gameObject.layer == (enemyLayer.value) && !takingDamage)
        {
            TakeDamage(10f, 0, collision.gameObject.transform.position, collision.gameObject.name);
        }
    }

    private void Movement()
    {
        float d = (dashing) ? DASH : 0f;
        //horizontal
        if (sideJTime <= 0f && !isAttacking)
            rb2d.velocity = new Vector2(horzM * (SPEED + d), rb2d.velocity.y);
        else
            sideJTime -= Time.deltaTime;
        //force object to not drill horizontally
        //raycast for terrain
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
        if (rb2d.velocity.x > 0f)
        {
            //facing right
            attacks.transform.position = new Vector2(transform.position.x + 0.75f, attacks.transform.position.y);
            aoeAttack.transform.position = new Vector2(transform.position.x, aoeAttack.transform.position.y);
        }
        else if (rb2d.velocity.x < 0f)
        {
            //facing left
            attacks.position = new Vector2(transform.position.x - 0.75f, attacks.transform.position.y);
            aoeAttack.position = new Vector2(transform.position.x, aoeAttack.transform.position.y);
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
                countering = true;
            }
            else
            {
                for (int i = 0; i < groundAttacks[result].hits; i++)
                {
                    StartCoroutine(AttackDamageTimer(groundAttacks[result].windUp[i],
                                                groundAttacks[result].attackPos,
                                                groundAttacks[result].rad,
                                                groundAttacks[result].GetType(i,usingDark),
                                                groundAttacks[result].damage[i]));
                }
               
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

            }
            else
            {
                for (int i = 0; i < airAttacks[result].hits; i++)
                {
                    StartCoroutine(AttackDamageTimer(airAttacks[result].windUp[i],
                                                 airAttacks[result].attackPos,
                                                 airAttacks[result].rad,
                                                 airAttacks[result].GetType(i,usingDark),
                                                 airAttacks[result].damage[i]));
                }
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
    
    public bool IsCountering()
    {
        return countering;
    }
    public void TakeDamage(float damage, int type, Vector2 enemyDir)
    {
        if (!takingDamage)
        {
            ChangeHP((-damage * damageMult[type]));
            isAttacking = false;
            takingDamage = true;
            //play flinch animation
            Vector2 pushDir = ((Vector2)transform.position - enemyDir);
            rb2d.velocity = pushDir.normalized * FLINCH_DIST;
            StopAllCoroutines();
            StartCoroutine(Flinching());
        }
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

    private IEnumerator AttackDamageTimer(float windUp, Vector2 attackPos, float rad, int attackType, float damage)
    {
        //windUp is the amount of time between the start of the animation and when it should deal damage
        yield return new WaitForSeconds(windUp);
        //deal damage to all enemies in the radius
        Vector2 actualPos = transform.position;
        actualPos += attackPos;
        Collider2D[] hits = Physics2D.OverlapCircleAll(actualPos, rad, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("hit enemy!");
            hit.gameObject.GetComponent<EnemyAI>().TakeDamage(damage, attackType);
        }
    }
    private IEnumerator NotAttackingTimer(float wait)
    {
        yield return new WaitForSeconds(wait);
        isAttacking = false;
    }
    private IEnumerator Flinching()
    {
        yield return new WaitForSeconds(0.25f * (1f - PlayerStats.flinchMod));
        takingDamage = false;
    }


    [ContextMenu("deal 5 damage")]
    protected void damageTest()
    {
        CurrHp = CurrHp - 5;
    }
}
