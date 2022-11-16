using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossCombat : BossBaseAI
{
    
    [SerializeField] private int facing; //1 for right, -1 for left
    
    
    private float[] damageMult;
    public GameObject attackEffectVisualizer;
    public Vector2[] anchors;
    public Vector2 currAnchor;
    public Vector2 xMinMax;
    protected int chosenAnchor;
    protected float currTime;
    protected float duration;
    protected override void InitBossVars()
    {
        hp = 1000f;
        totalPhases = 2;
        phaseTrigger = new float[] { 500f };
        damageMult = new float[] { 1f, 1.25f, 0.75f };
        //attacks
        //gorilla form:
        /*
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
        */
        //spider form:
        //attack 0: forward slash, counterable
        //attack 1: quad slash, counterable
        //attack 2: spike shot
        //attack 3: rooftop drop
    }
    protected override void ChooseAttack()
    {
        /*
        float pick = Random.Range(0f,20f);
        switch (pick)
        {
            case float p when (p < 8f):
                currAttack = 0;
                break;
            case float p when (p < 16f):
                currAttack = 1;
                break;
            case float p when (p < 18f):
                currAttack = 2;
                break;
            case float p when (p >= 18f):
                currAttack = 3;
                break;
        }*/
        currAttack = 0;
        //start animation
        //anim.SetBool(attacks[currAttack].animBoolName, true);
        if (currAttack < 2)
        {
            facing = (GameObject.Find("player").transform.position.x >= transform.position.x) ? 1 : -1;
            StartCoroutine(TriggerAttack());
        }
        else
            StartTravelToAnchor();
    }

    protected void FixedUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x, facing, transform.localScale.z);
        if (attacking && !attacks[currAttack].IsDone())
        {
            transform.position = attacks[currAttack].GetCurrent().movement.LerpedPos(facing);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMinMax.x, xMinMax.y), 
                transform.position.y, transform.position.z);
        }

    }
    public override void PlayIntro()
    {
        FindObjectOfType<CinematicController>().StartCinematic(
            new Vector2[] { transform.position }, new float[] { 3f });
        StartCoroutine(StartFight(3f));
    }

    protected override IEnumerator StartFight(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
    }

    protected override void MeleeAttack(BossAttack currMove)
    {
        Vector2 offset = new Vector2(currMove.attackOffset.x * facing, currMove.attackOffset.y);
        Vector2 attackPos = (Vector2)transform.position + offset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, currMove.radius, playerLayer);
        //TESTING, GET RID OF THIS//
        GameObject effect = Instantiate(attackEffectVisualizer, attackPos, Quaternion.identity);
        effect.GetComponent<AttackEffectScript>().InitEffect(currMove.type, currMove.radius);
        foreach (Collider2D hit in hits)
        {
            PlayerController pc = hit.gameObject.GetComponent<PlayerController>();
            if (pc.IsCountering() && attacks[currAttack].counterable)
                Counter();
            else
                pc.TakeDamage(currMove.damage, currMove.type, attackPos, 2f);
        }
    }
    protected void TravelToAnchor()
    {
        while (currTime < duration)
        {
            transform.position = Vector2.Lerp(transform.position, currAnchor, currTime/duration);
            currTime += Time.deltaTime;
        }
        transform.position = currAnchor;
        if (chosenAnchor == 0)                      //if at left anchor
            facing = (currAttack == 2) ? -1 : 1;    //2: jump left to wall; 3: jump to center
        else                                        //if at right anchor
            facing = (currAttack == 2) ? 1 : -1;    //2: jump right to wall; 3: jump to center
        CancelInvoke("TravelToAnchor");
        StartCoroutine(TriggerAttack());
    }
    protected void StartTravelToAnchor()
    {
        currTime = 0f;
        chosenAnchor = (Vector2.Distance(transform.position, anchors[0]) < Vector2.Distance(transform.position, anchors[1])) ?
            0 : 1;
        currAnchor = anchors[chosenAnchor];
        duration = Vector2.Distance(transform.position, anchors[chosenAnchor])/10f;
        facing = (transform.position.x < currAnchor.x) ? 1 : -1;
        InvokeRepeating("TravelToAnchor", 0f, 0.05f);
    }
 }
