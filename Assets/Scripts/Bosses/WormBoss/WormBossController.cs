using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBossController : BossBaseAI
{
    

    protected override void ChooseAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitBossVars()
    {
        hp = 1200f;
        touchDamage = 25f;
        totalPhases = 4;
        phaseTrigger = 300f;
        damageMult = new float[] { 1f, 0.75f, 1.25f };
    }

    public override void PlayIntro()
    {
        Vector2 center = bossRoom.transform.position;
        Vector2 offset = bossRoom.dimensions / 2f;
        FindObjectOfType<CinematicController>().StartCinematic(
            new Vector2[] { center,
                            new Vector2(center.x + offset.x, center.y + offset.y),
                            new Vector2(center.x + offset.x, center.y - offset.y),
                            new Vector2(center.x - offset.x, center.y - offset.y),
                            new Vector2(center.x - offset.x, center.y + offset.y),
                            center}, 
            new float[] { 2f, 0.5f, 0.5f, 0.5f, 0.5f, 1f });
        StartCoroutine(StartFight(2.5f));
    }
    protected override IEnumerator StartFight(float time)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<BossHud>().StartBossHud(this);
        yield return new WaitForSeconds(time);
        active = true;
    }

    protected override void PhaseChangeEvent()
    {
        Debug.Log("Phase changing");
    }
    protected override void Die()
    {
        FindObjectOfType<BossHud>().StopBossHud();
        bossRoom.EnemyKilled();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
}
