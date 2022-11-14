using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private int hpWidthMax;
    [SerializeField] private int hpHeightMax;
    [SerializeField] private int balanceWidthMax;
    [SerializeField] private int balanceHeightMax;
    private Color lightMax = new Color(1f, 0.75f, 0f, 1f);
    private Color darkMax = new Color(1f, 0f, 0.75f, 1f);
    private Color white = new Color(1f, 1f, 1f, 1f);
    private float maxHpBase;
    public GameObject canvas;

    private PlayerController player;
    private EnemyPuppeteer puppeteer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        puppeteer = FindObjectOfType<EnemyPuppeteer>();
        maxHpBase = (float)player.getMaxHp();
        hpWidthMax = player.getMaxHp();
        player.onHPChanged += setCurrHPBar;
        player.onHPMaxChanged += SetMaxHPBar;
        puppeteer.onWorldBalanceChanged += SetBalanceBar;
        SetBalanceBar();
        canvas.transform.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(hpWidthMax, hpHeightMax);
        canvas.transform.Find("Balance").GetComponent<RectTransform>().sizeDelta = new Vector2(balanceWidthMax, balanceHeightMax);
        canvas.transform.Find("Balance").gameObject.SetActive(PlayerStats.hasLantern);
    }

    void setCurrHPBar()
    {
        float currhp = (float)player.CurrHp;
        float sliderValue = (currhp / maxHpBase);
        canvas.transform.Find("Health").transform.GetComponent<Slider>().value = sliderValue;
        Debug.Log(sliderValue);
    }
    void SetMaxHPBar()
    {
        hpWidthMax = player.getMaxHp();
        canvas.transform.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(hpWidthMax, hpHeightMax);
    }

    void SetBalanceBar()
    {
        float currBalance = puppeteer.worldBalance/100f; //get world balance as percent
        Color currColor;
        Vector3 colorSlider;
        if (currBalance == 0f)
        {
            currColor = white;
        }
        else if (currBalance > 0f)
        {
            colorSlider = Vector3.Lerp(new Vector3(white.r, white.g, white.b),
                                       new Vector3(lightMax.r, lightMax.g, lightMax.b),
                                       currBalance);
            currColor = new Color(colorSlider.x,colorSlider.y,colorSlider.z,1f);
        }
        else
        {
            colorSlider = Vector3.Lerp(new Vector3(white.r, white.g, white.b),
                                       new Vector3(darkMax.r, darkMax.g, darkMax.b),
                                       -currBalance);
            currColor = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
        }
        canvas.transform.Find("Balance").transform.Find("balanceBarCurrent").GetComponent<Image>().color = currColor;
        canvas.transform.Find("Balance").GetComponent<Slider>().value = Mathf.Clamp(currBalance, -0.95f, 0.95f);
            
    }

    public void ActivateBalanceBar()
    {
        canvas.transform.Find("Balance").gameObject.SetActive(PlayerStats.hasLantern);
    }

}
