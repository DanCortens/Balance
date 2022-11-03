using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void UnlockDelagate();

public class Upgrade : MonoBehaviour
{
    public string upName;
    public Upgrade prereq;
    public string descrip;
    public bool unlocked;

    public Sprite lockedImg;
    public Sprite unlockedImg;
    public Sprite activeImg;

    public UnlockDelagate ud;

    public Upgrade(string name, Upgrade prereq, string descrip, bool unlocked, Sprite lockedImg,
        Sprite unlockedImg, Sprite activeImg)
    {
        this.upName = name;
        this.prereq = prereq;
        this.descrip = descrip;
        this.unlocked = unlocked;
        this.lockedImg = lockedImg;
        this.unlockedImg = unlockedImg;
        this.activeImg = activeImg;
        ud = null;
        ud += new UnlockDelagate(Unlock);
    }
    public void Unlock()
    {
        this.unlocked = true;
    }
}
