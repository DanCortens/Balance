using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseAI : MonoBehaviour
{
    public event System.Action onHPChanged;

    [Header("Health")]
    private float _hp;
    public float hp
    {
        get { return _hp; }
        set
        {
            if (_hp == value) return;
            _hp = value;
            onHPChanged?.Invoke();
            if (_hp <= 0f)
                Die();
        }
    }
    public bool active;
    [SerializeField] protected float phaseTrigger;
    [SerializeField] protected float phaseProg;
    [SerializeField] protected int phase;
    [SerializeField] protected int totalPhases;
    [Header("Combat")]
    [SerializeField] protected BossAttackChain[] attacks;
    [SerializeField] protected float[] damageMult;
    [SerializeField] protected bool attacking;
    [SerializeField] protected bool stunned;
    [SerializeField] protected bool dying;
    public string deathAnimBool;
    [SerializeField] protected int currAttack;
    public GameObject splineAnchor;
    protected GameObject player;
    protected LayerMask playerLayer;
    public RoomControl bossRoom;

    void Start()
    {
        player = GameObject.Find("player");
        playerLayer = LayerMask.GetMask("Player");
        phase = 0;
        phaseProg = 0f;
        currAttack = -1;    
        attacking = false;
        stunned = false;
        active = false;
        InitBossVars();
    }
    protected abstract void InitBossVars();
    protected abstract void ChooseAttack();
    protected abstract void Die();
    
    public void ChangePhase(float damage)
    {
        phaseProg += damage;
        
        if (phaseProg >= phaseTrigger)
        {
            phase++;
            phaseProg = 0f;
            PhaseChangeEvent();
        }
            
    }
    protected abstract void PhaseChangeEvent();

    protected void Update()
    {
        if (active)
            if (currAttack < 0 && !stunned && !dying)
            {
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
        StopAttack();
        //switch to stunned animation
        StartCoroutine(StunRecover(2.5f));

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
        currAttack = -1;
    }
    protected IEnumerator StunRecover(float time)
    {
        yield return new WaitForSeconds(time);
        stunned = false;
    }
    protected abstract void RangedAttack(BossAttack currMove);
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
        attacking = true;
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
    public void TakeDamage(float damage, int type)
    {
        float actualDamage = damage * damageMult[type];
        hp -= (actualDamage > hp) ? hp : actualDamage;
        if (phase < totalPhases - 1)
            ChangePhase(actualDamage);
    }
}