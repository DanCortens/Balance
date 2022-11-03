using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    //events
    public event System.Action onHPChanged;
    public event System.Action onDeath;
    //bool
    protected bool takingDamage;
    //constants
    protected float[] damageMult = { 1f, 1f, 1f };//subject to change
    protected const int MAX_HP_BASE = 100;
    protected const float FLINCH_DIST = 10f;
    //stats
    [SerializeField]
    private int currHp;
    //properties
    public int CurrHp
    {
        get { return currHp; }
        set
        {
            if (currHp == value) return;
            currHp = value;
            onHPChanged?.Invoke();
        }
    }

    void Start()
    {
        CurrHp = MAX_HP_BASE;
    }

    public void TakeDamage(float damage, int type, Vector2 enemyDir, float pushForce)
    {
        if (!takingDamage)
        {
            CurrHp += (int)(-damage * damageMult[type]);
            if (currHp <= 0)
            {
                onDeath?.Invoke();
            }
            takingDamage = true;
            //play flinch animation
            Vector2 pushDir = transform.position;
            pushDir -= enemyDir;
            StopAllCoroutines();
        }
    }

    /* 
     * returns max hp as int
     */
    public int getMaxHp()
    {
        return MAX_HP_BASE;
    }
}
