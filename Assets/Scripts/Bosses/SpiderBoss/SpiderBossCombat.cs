using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossCombat : BossBaseAI
{
    
    [SerializeField] private int facing; //1 for right, -1 for left
    
    
    private float[] damageMult;
    
    public Vector2 anchors;
    
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
        }
        //start animation
        //anim.SetBool(attacks[currAttack].animBoolName, true);
        StartCoroutine(TriggerAttack());
    }

    protected void FixedUpdate()
    {
        if (attacking)
        {
            transform.position = attacks[currAttack].GetCurrent().movement.LerpedPos(facing);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("turn");
        facing *= -1;
    }

    protected new void MeleeAttack(BossAttack currMove)
    {
        Vector2 offset = currMove.attackOffset;
        offset.x *= facing;
        Vector2 attackPos = (Vector2)transform.position + offset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, currMove.radius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerController pc = hit.gameObject.GetComponent<PlayerController>();
            if (pc.IsCountering() && attacks[currAttack].counterable)
                Counter();
            else
                pc.TakeDamage(currMove.damage, currMove.type, attackPos, 2f);
        }
    }
}
