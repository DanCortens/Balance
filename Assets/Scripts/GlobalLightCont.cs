using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightCont : MonoBehaviour
{
    public Light2D skyboxLights;
    public Light2D backgroundLights;
    public Light2D foregroundLights;

    [SerializeField] private RoomSetting setting;
    private float progress;
    private float duration = 3f;

    private Color slight = new Color(0.9f, 0.9f, 0.9f, 1f);
    private Color medium = new Color(0.7f, 0.7f, 0.7f, 1f);
    private Color dark = new Color(0.4f, 0.4f, 0.4f, 1f);
    private Color golden0 = new Color(1f, 0.7f, 0f, 1f);
    private Color golden1 = new Color(1f, 0.8f, 0.5f, 1f);
    private Color spooky0 = new Color(0.95f, 0.47f, 0.55f, 1f);
    private Color spooky1 = new Color(1f, 0.4f, 0.4f, 1f);
    private Color[] startColor = new Color[3];
    private Color[] tarColor = new Color[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NewRoom(RoomSetting newSetting)
    {
        if (setting == newSetting) return;
        CancelInvoke();
        progress = 0f;
        setting = newSetting;
        startColor[0] = skyboxLights.color;
        startColor[1] = backgroundLights.color;
        startColor[2] = foregroundLights.color;
        SetTargetCols();
        InvokeRepeating("AdjustLights", 0f, 0.1f);
    }
    private void SetTargetCols()
    {
        switch (setting)
        {
            case RoomSetting.General:
                tarColor[0] = Color.white;
                tarColor[1] = Color.white;
                tarColor[2] = Color.white;
                break;
            case RoomSetting.IndoorsBright:
                tarColor[0] = Color.white;
                tarColor[1] = medium;
                tarColor[2] = slight;
                break;
            case RoomSetting.IndoorsDark:
                tarColor[0] = dark;
                tarColor[1] = dark;
                tarColor[2] = medium;
                break;
            case RoomSetting.OutdoorsBright:
                tarColor[0] = Color.white;
                tarColor[1] = Color.white;
                tarColor[2] = slight;
                break;
            case RoomSetting.OutdoorsDark:
                tarColor[0] = dark;
                tarColor[1] = medium;
                tarColor[2] = medium;
                break;
            case RoomSetting.OutdooorsGolden:
                tarColor[0] = golden0;
                tarColor[1] = golden1;
                tarColor[2] = golden1;
                break;
            case RoomSetting.OutdoorsSpooky:
                tarColor[0] = spooky0;
                tarColor[1] = spooky1;
                tarColor[2] = spooky1;
                break;
        }
    }
    private void AdjustLights()
    {
        progress += 0.1f;
        Vector3 colorSlider;
        if (progress >= duration)
        {
            skyboxLights.color = tarColor[0];
            backgroundLights.color = tarColor[1];
            foregroundLights.color = tarColor[2];
            CancelInvoke();
        }
        else
        {
            colorSlider = Vector3.Lerp(new Vector3(startColor[0].r, startColor[0].g, startColor[0].b),
                                       new Vector3(tarColor[0].r, tarColor[0].g, tarColor[0].b), 
                                       progress / 2f);
            skyboxLights.color = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
            colorSlider = Vector3.Lerp(new Vector3(startColor[1].r, startColor[1].g, startColor[1].b),
                                       new Vector3(tarColor[1].r, tarColor[1].g, tarColor[1].b),
                                       progress / 2f);
            backgroundLights.color = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
            colorSlider = Vector3.Lerp(new Vector3(startColor[2].r, startColor[2].g, startColor[2].b),
                                       new Vector3(tarColor[2].r, tarColor[2].g, tarColor[2].b),
                                       progress / 2f);
            foregroundLights.color = new Color(colorSlider.x, colorSlider.y, colorSlider.z, 1f);
        }
    }
}
