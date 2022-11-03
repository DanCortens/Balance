using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<GameObject> neutralUps;
    public List<GameObject> darkUps;
    public List<GameObject> lightUps;

    public int bossSouls;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        int neut = 0;
        int dark = 0;
        int light = 0;
        foreach (GameObject u in neutralUps)
            if (u.GetComponent<UpgradeButton>().thisUpgrade.unlocked)
                neut++;
        foreach (GameObject u in darkUps)
            if (u.GetComponent<UpgradeButton>().thisUpgrade.unlocked)
                dark++;
        foreach (GameObject u in lightUps)
            if (u.GetComponent<UpgradeButton>().thisUpgrade.unlocked)
                light++;
        PlayerPrefs.SetInt("neutralUps", neut);
        PlayerPrefs.SetInt("darkUps", dark);
        PlayerPrefs.SetInt("lightUps", light);
    }
    public void InitializeUpgrades()
    {

        int neutUnlocked = (PlayerPrefs.HasKey("neutralUps")) ? PlayerPrefs.GetInt("neutralUps") : 0;
        int darkUnlocked = (PlayerPrefs.HasKey("darkUps")) ? PlayerPrefs.GetInt("darkUps") : 0;
        int lightUnlocked = (PlayerPrefs.HasKey("lightUps")) ? PlayerPrefs.GetInt("lightUps") : 0;

        for (int i = 0; i < neutralUps.Count; i++)
        {
            neutralUps[i].GetComponent<UpgradeButton>().thisUpgrade.unlocked = 
                (i < neutUnlocked)? true : false;
            neutralUps[i].GetComponent<UpgradeButton>().InitUpgradeButton(i);
        }
        for (int i = 0; i < darkUps.Count; i++)
        {
            darkUps[i].GetComponent<UpgradeButton>().thisUpgrade.unlocked =
                (i < darkUnlocked) ? true : false;
            darkUps[i].GetComponent<UpgradeButton>().InitUpgradeButton(i);
        }
        for (int i = 0; i < lightUps.Count; i++)
        {
            lightUps[i].GetComponent<UpgradeButton>().thisUpgrade.unlocked =
                (i < lightUnlocked) ? true : false;
            lightUps[i].GetComponent<UpgradeButton>().InitUpgradeButton(i);
        }
    }
}
