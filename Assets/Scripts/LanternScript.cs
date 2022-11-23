using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternScript : MonoBehaviour
{
    private Transform swirly;
    public Light2D lanLight;
    public Light2D lanDark;
    public Light2D lightGen;
    private EnemyPuppeteer puppeteer;
    private Color lightMax = new Color(1f, 0.75f, 0f, 1f);
    private Color darkMax = new Color(1f, 0f, 0.75f, 1f);
    private Color white = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        swirly = transform.Find("Swirly");
        puppeteer = FindObjectOfType<EnemyPuppeteer>();
        puppeteer.onWorldBalanceChanged += SetColours;
    }

    // Update is called once per frame
    void Update()
    {
        swirly.Rotate(swirly.forward, -1f);
    }

    public void SetColours()
    {
        if (puppeteer.worldBalance > 0)
        {

        }
    }
}
