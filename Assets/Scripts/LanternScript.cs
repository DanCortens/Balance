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
        float currBalance = puppeteer.worldBalance / 100f; //get world balance as percent
        Color currColor;
        Vector3 colorSlider;
        if (currBalance == 0f)
        {
            currColor = white;
            lanLight.intensity = 2f;
            lanDark.intensity = 2f;
        }
        else if (currBalance > 0f)
        {
            lanLight.intensity = 2f + (2f * currBalance);
            lanDark.intensity = 2f - (1f * currBalance);
            colorSlider = Vector3.Lerp(new Vector3(white.r, white.g, white.b),
                                       new Vector3(lightMax.r, lightMax.g, lightMax.b),
                                       currBalance);
            currColor = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
        }
        else
        {
            lanLight.intensity = 2f - (1f * currBalance);
            lanDark.intensity = 2f - (2f * currBalance);
            colorSlider = Vector3.Lerp(new Vector3(white.r, white.g, white.b),
                                       new Vector3(darkMax.r, darkMax.g, darkMax.b),
                                       -currBalance);
            currColor = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
        }
        lightGen.color = currColor;
    }
}
