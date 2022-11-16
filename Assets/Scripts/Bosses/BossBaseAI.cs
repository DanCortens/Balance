using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseAI : MonoBehaviour
{
    [Header("Health")]
    private float _hp;
    public float hp
    {
        get { return _hp; }
        set
        {
            if (_hp == value) return;
            _hp = value;
            if (_hp <= 0f)
                Die();
            else if (_hp <= phaseTrigger[phase])
                ChangePhase();

        }
    }
    protected bool active;
    [SerializeField] protected float[] phaseTrigger;
    [SerializeField] protected int phase;
    [SerializeField] protected int totalPhases;
    [Header("Combat")]
    [SerializeField] protected BossAttackChain[] attacks;
    [SerializeField] protected bool attacking;
    [SerializeField] protected bool stunned;
    [SerializeField] protected bool dying;
    public string deathAnimBool;
    [SerializeField] protected int currAttack;

    protected LayerMask playerLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        phase = 0;
        attacking = false;
        stunned = false;
        active = false;
        InitBossVars();
    }
    protected abstract void InitBossVars();
    protected abstract void ChooseAttack();
    protected void Die()
    {

    }
    public void ChangePhase()
    {
        if (phase < totalPhases - 1)
            phase++;
    }

    protected void Update()
    {
        if (active)
            if (!attacking && !stunned && !dying)
            {
                attacking = true;
                ChooseAttack();
            }
    }

    public abstract void PlayIntro();
    
    protected abstract IEnumerator StartFight(float time);
    

    //called when player attempts to counter
    protected void Counter()
    {
        StopAllCoroutines();
        stunned = true;
        attacking = false;
        //switch to stunned animation
        StartCoroutine(StunRecover());

    }
    //called by attack patterns to initialize next attacks
    protected void PopNext()
    {
        if (!attacks[currAttack].UpdateCurrAttack())
            StartCoroutine(TriggerAttack());
        else
            Invoke("StopAttack", attacks[currAttack].cooldown);
    }
    protected void StopAttack()
    {
        attacking = false;
        attacks[currAttack].currentAttack = 0;
    }
    protected IEnumerator StunRecover()
    {
        yield return new WaitForSeconds(1.75f);
        stunned = false;
    }
    protected void RangedAttack(BossAttack currMove)
    {

    }
    protected abstract void MeleeAttack(BossAttack currMove);
    /*
    {
        Vector2 offset = currMove.attackOffset;
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
    }*/
    protected IEnumerator TriggerAttack()
    {
        BossAttack currMove = attacks[currAttack].GetCurrent();
        Debug.Log($"{attacks[currAttack].animBoolName} on {attacks[currAttack].currentAttack}");
        currMove.StartAttack(transform.position);
        yield return new WaitForSeconds(currMove.windUp);
        //if attack, start attack coroutine
        if (currMove.hasAttack)
            if (currMove.rangedAttack)
                RangedAttack(currMove);
            else
                MeleeAttack(currMove);
        //wait for total animation time
        float t = currMove.TotalTime();
        yield return new WaitForSeconds(t);
        PopNext();
    }
}
