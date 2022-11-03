using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private int hpWidthMax;
    [SerializeField] private int hpHeightMax;
    private float maxHpBase;
    public GameObject canvas;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        maxHpBase = (float)player.getMaxHp();
        player.onHPChanged += setCurrHPBar;
        canvas.transform.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(hpWidthMax, hpHeightMax);
    }

    void setCurrHPBar()
    {
        float currhp = (float)player.CurrHp;
        float sliderValue = (currhp / maxHpBase);
        canvas.transform.Find("Health").transform.GetComponent<Slider>().value = sliderValue;
        Debug.Log(sliderValue);
    }

    // Update is called once per frame
    //not neccessary since this'll rely on events
    /*void Update()
    {
        
    }*/
}
