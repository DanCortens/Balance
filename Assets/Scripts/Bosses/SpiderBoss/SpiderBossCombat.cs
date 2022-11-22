using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossCombat : BossBaseAI
{
    public Vector2[] anchors;
    public Vector2 currAnchor;
    public Vector2 xMinMax;
    private Vector2 anchorStartPos;
    protected int chosenAnchor;
    protected float currTime;
    protected float duration;
    protected override void InitBossVars()
    {
        hp = 1000f;
        touchDamage = 10f;
        totalPhases = 2;
        phaseTrigger = 500f;
        damageMult = new float[] { 1f, 1.25f, 0.75f };
    }
    protected override void PhaseChangeEvent()
    {
        //play "spider emerging" anim
        Debug.Log("Phase changing");
        stunned = true;
        FindObjectOfType<CinematicController>().StartCinematic(
            new Vector2[] { transform.position }, new float[] { 3f });
        StartCoroutine(StunRecover(3.5f));

    }
    protected override void ChooseAttack()
    {
        
        float pick = Random.Range(0f,20f);
        switch (pick)
        {
            case float p when (p < 6f):
                currAttack = 0;
                break;
            case float p when (p < 12f):
                currAttack = 1;
                break;
            case float p when (p < 17f):
                currAttack = 2;
                break;
            case float p when (p >= 17f):
                currAttack = 3;
                break;
        }
        currAttack += phase * 4;
        //start animation
        //anim.SetBool(attacks[currAttack].animBoolName, true);
        
        if (currAttack % 4 != 3)
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
            BossMove movement = attacks[currAttack].GetCurrent().movement;
            if (movement is BossMoveSpline) 
                transform.position = new Vector3 (movement.startPos.x + ((BossMoveSpline)movement).LerpedPos(facing).x,
                                                  movement.startPos.y + ((BossMoveSpline)movement).LerpedPos(facing).y,
                                                  transform.position.z);
            else
                transform.position = movement.LerpedPos(facing);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMinMax.x, xMinMax.y), 
                transform.position.y, transform.position.z);
        }

    }
    public override void PlayIntro()
    {
        FindObjectOfType<CinematicController>().StartCinematic(
            new Vector2[] { transform.position }, new float[] { 3f });
        StartCoroutine(StartFight(1.5f));
    }

    protected override IEnumerator StartFight(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<BossHud>().StartBossHud(this);
        yield return new WaitForSeconds(time);
        active = true;
    }
    protected override void Die()
    {
        FindObjectOfType<BossHud>().StopBossHud();
        bossRoom.EnemyKilled();
    }

    protected void TravelToAnchor()
    {
        transform.position = Vector2.Lerp(anchorStartPos, currAnchor, currTime / duration);
        currTime += 0.05f;
        Debug.Log($"duration: {duration}   currTime: {currTime}");
        if (currTime >= duration)
        {
            transform.position = currAnchor;
            if (chosenAnchor == 0)                      //if at left anchor
                facing =  -1;                           //jump left to wall
            else                                        //if at right anchor
                facing = 1;                             //jump right to wall
            CancelInvoke("TravelToAnchor");
            StartCoroutine(TriggerAttack());
        }
        
    }
    protected void StartTravelToAnchor()
    {
        currTime = 0f;
        anchorStartPos = transform.position;
        chosenAnchor = (Vector2.Distance(transform.position, anchors[0]) < Vector2.Distance(transform.position, anchors[1])) ?
            0 : 1;
        currAnchor = anchors[chosenAnchor];
        duration = Vector2.Distance(anchorStartPos, currAnchor) /6f;
        
        facing = (transform.position.x < currAnchor.x) ? 1 : -1;
        InvokeRepeating("TravelToAnchor", 0f, 0.05f);
    }
 }
