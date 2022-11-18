using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossHud : MonoBehaviour
{
    [SerializeField] private int hpWidthMax;
    [SerializeField] private int hpHeightMax;
    private BossBaseAI boss;
    private float maxHpBase;
    public GameObject canvas;
    public Animator anim;

    public void StartBossHud(BossBaseAI boss)
    {
        this.boss = boss;
        maxHpBase = boss.hp;
        SetCurrHPBar();
        boss.onHPChanged += SetCurrHPBar;
        canvas.transform.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(hpWidthMax, hpHeightMax);
        anim.SetTrigger("fadeIn");
    }
    public void StopBossHud()
    {
        boss.onHPChanged -= SetCurrHPBar;
        anim.SetTrigger("fadeOut");
    }

    public void SetCurrHPBar()
    {
        float currhp = (float)boss.hp;
        float sliderValue = (currhp / maxHpBase);
        canvas.transform.Find("Health").transform.GetComponent<Slider>().value = sliderValue;
    }
}
