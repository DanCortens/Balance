using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float _hp;
    public float hp
    {
        get { return _hp; }
        set
        {
            if (_hp == value) return;
            _hp = value;
            if (_hp <= phaseTrigger)
                ChangePhase();
        }
    }
    [SerializeField] private float phaseTrigger;
    [SerializeField] private int phase;
    [SerializeField] private bool attacking;
    [SerializeField] private int currAtt;
    [SerializeField] private bool counterable;
    [SerializeField] private bool stunned;
    [SerializeField] private int facing; //1 for right, -1 for left
    public GameObject projectile;
    
    private float[] damageMult;
    private BossAttack[] attacks = new BossAttack[4];
    private Vector2 moveBy;
    private Vector2 startPos;
    private float currTime;
    private float moveTime;
    private bool move;
    private LayerMask playerLayer;

    void Start()
    {
        
        phase = 0;
        facing = 1;
        attacking = false;
        counterable = false;
        stunned = false;
        playerLayer = LayerMask.GetMask("Player");
        InitBossVars();


    }
    private void InitBossVars()
    {
        hp = 1000f;
        phaseTrigger = 500f;
        damageMult = new float[] { 1f, 0.75f, 1.25f };
        //attacks
        //gorilla form:
        //attack 0: double slam, counterable
        attacks[0] = new BossAttack(new BossAttack.Move[] {
            new BossAttack.Swing(0.25f,0.25f,new Vector2(2f,0f), new Vector2(0.3f, -0.2f),0.8f,25f,0),
            new BossAttack.Swing(0.25f,0.25f,new Vector2(0f,0f), new Vector2(0.3f, -0.2f),0.8f,25f,0)},
            "doubleSlam", true);
        //attack 1: clap, counterable
        attacks[1] = new BossAttack(new BossAttack.Move[] {
            new BossAttack.Swing(0.4f,0.25f,new Vector2(0f,0f), new Vector2(0.3f, -0.2f),0.8f,60f,0),},
            "clap", true);
        //attack 2: pile driver
        //TEMP FOR TESTING//TEMP FOR TESTING//TEMP FOR TESTING//TEMP FOR TESTING
        attacks[2] = new BossAttack(new BossAttack.Move[] {
            new BossAttack.Swing(0.4f,0.25f,new Vector2(0f,0f), new Vector2(0.3f, 0f),0.8f,60f,0),},
            "pileDrive", false);
        //attack 3: jump attack
        //TEMP FOR TESTING//TEMP FOR TESTING//TEMP FOR TESTING//TEMP FOR TESTING
        attacks[3] = new BossAttack(new BossAttack.Move[] {
            new BossAttack.Swing(0.4f,0.25f,new Vector2(0f,0f), new Vector2(0.3f, 0f),0.8f,60f,0),},
            "jumpAttack", false);

        //spider form:
        //attack 0: forward slash, counterable
        //attack 1: quad slash, counterable
        //attack 2: spike shot
        //attack 3: rooftop drop
    }
    void Update()
    {
        if (!attacking && !stunned && hp > 0f)
        {
            attacking = true;
            currAtt = 0;
            int pick = (int)Random.Range(0f, 20f);
            switch (pick)
            {
                case int p when (p < 8):
                    StartCoroutine(MoveEvent(0));
                    break;
                case int p when (p >= 8 && p < 16):
                    StartCoroutine(MoveEvent(1));
                    break;
                case int p when (p >= 16 && p < 18):
                    StartCoroutine(MoveEvent(2));
                    break;
                case int p when (p >= 18):
                    StartCoroutine(MoveEvent(3));
                    break;
            }
        }
    }
    private void FixedUpdate()
    {
        if (move)
            Movement();
    }

    private void Movement()
    {
        currTime += Time.deltaTime;
        if (currTime >= moveTime)
        {
            move = false;
            transform.position = startPos + moveBy;
        }
        else
            transform.position = Vector2.Lerp(startPos, startPos + moveBy, currTime/moveTime);
    }

    //called when player attempts to counter
    public void Counter()
    {   
        StopAllCoroutines();
        stunned = true;
        attacking = false;
        counterable = false;
        //switch to stunned animation
        StartCoroutine(StunRecover());
        
    }

    public void ChangePhase()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("turn");
        facing *= -1;
    }

    //called by attack patterns to initialize next attacks
    private void PopNext(int choice)
    {
        currAtt++;
        if (currAtt < attacks[choice].moves.Length)
            StartCoroutine(MoveEvent(choice));
        else
            attacking = false;
    }

    private IEnumerator StunRecover()
    {
        yield return new WaitForSeconds(1.75f);
        stunned = false;
    }
    private void AttackEvent(BossAttack.Swing currMove)
    {
        Vector2 offset = currMove.offset;
        offset.x *= facing;
        Vector2 attackPos = (Vector2)transform.position + offset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, currMove.range, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerController pc = hit.gameObject.GetComponent<PlayerController>();
            if (pc.IsCountering() && counterable)
                Counter();
            else
                pc.TakeDamage(currMove.damage, currMove.type, attackPos, 2f);
        }
    }
    private IEnumerator MoveEvent(int choice)
    {
        BossAttack.Move currMove = attacks[choice].moves[currAtt];
        Debug.Log($"{attacks[choice].animName} on {currAtt}");
        //start animation
        yield return new WaitForSeconds(currMove.windup);
        //if attack, start attack coroutine
        if (currMove.isAttack)
            AttackEvent((BossAttack.Swing)currMove);
        //set movement
        move = true;
        currTime = 0f;
        moveTime = currMove.time;
        startPos = transform.position;
        moveBy = currMove.moveBy*facing;
        //wait for total animation time
        float t = currMove.TotalTime();
        yield return new WaitForSeconds(t);
        PopNext(choice);
    }
}
