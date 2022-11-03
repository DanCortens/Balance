using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public TMP_Text text;
    public GameObject hoverBG;
    public GameObject confirm;
    public UpgradeManager manager;
    public TMP_Text hoverText;
    [SerializeField]private bool selected;
    public Upgrade thisUpgrade;
    private int id;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        if (!(thisUpgrade.prereq is null))
        {
            thisUpgrade.prereq.ud += new UnlockDelagate (PrereqUnlocked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitUpgradeButton(int id)
    {
        this.id = id;
        if (thisUpgrade.unlocked)
        {
            button.image.sprite = thisUpgrade.activeImg;
            PlayerStats.UpdateStats(id, type);
            text.text = "Active";
        }
        else if (thisUpgrade.prereq is null || thisUpgrade.prereq.unlocked)
        {
            button.image.sprite = thisUpgrade.unlockedImg;
            text.text = "Spend Soul";
        }
            
        else
        {
            button.image.sprite = thisUpgrade.lockedImg;
            text.text = "Locked";
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBG.SetActive(true);
        hoverBG.GetComponent<TooltipCont>().UpdatePos();
        hoverText.text = $"{thisUpgrade.upName}\n{thisUpgrade.descrip}";
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBG.SetActive(false);
        selected = false;
        confirm.SetActive(false);
    }
    public void Clicked()
    {
        if (!thisUpgrade.unlocked)
        {
            if (selected)
            {
                Unlock();
            }
            else if (manager.bossSouls > 0)
            {
                selected = true;
                confirm.SetActive(true);
            }
        }
    }
    public void Unlock()
    {
        confirm.SetActive(false);
        PlayerStats.UpdateStats(id, type);
        thisUpgrade.ud();
        button.image.sprite = thisUpgrade.activeImg;
        manager.bossSouls--;
    }
    public void PrereqUnlocked()
    {
        button.image.sprite = thisUpgrade.unlockedImg;
        text.text = "Spend Soul";
    }
}
