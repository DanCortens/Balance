using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float[] damageTakenMod = { 0f, 0f, 0f };
    public static float[] damageDoneMod = { 0f, 0f, 0f };
    public static float flinchMod = 0f;
    public static float attackSpeedMod = 0f;
    public static float enemyFlinchMod = 0f;

    private static bool _hasLantern;
    public static bool hasLantern
    {
        get { return _hasLantern; } set { _hasLantern = value; }
    }

    private static bool _hasChain;
    public static bool hasChain
    {
        get { return _hasChain; }
        set { _hasChain = value; }
    }

    private static bool _hasDoubleJ;
    public static bool hasDoubleJ
    {
        get { return _hasDoubleJ; }
        set { _hasDoubleJ = value; }
    }

    private static bool _hasPoison;
    public static bool hasPoison
    {
        get { return _hasPoison; }
        set { _hasPoison = value; }
    }

    private static bool _hasFlame;
    public static bool hasFlame
    {
        get { return _hasFlame; }
        set { _hasFlame = value; }
    }

    public static void Load()
    {
        if (PlayerPrefs.HasKey("hasLantern"))
            hasLantern = PlayerPrefs.GetInt("hasLantern") != 0;
        else
            hasLantern = false;

        if (PlayerPrefs.HasKey("hasChain"))
            hasLantern = PlayerPrefs.GetInt("hasChain") != 0;
        else
            hasLantern = false;

        if (PlayerPrefs.HasKey("hasDoubleJ"))
            hasDoubleJ = PlayerPrefs.GetInt("hasDoubleJ") != 0;
        else
            hasDoubleJ = false;

        if (PlayerPrefs.HasKey("hasPoison"))
            hasPoison = PlayerPrefs.GetInt("hasPoison") != 0;
        else
            hasPoison = false;

        if (PlayerPrefs.HasKey("hasFlame"))
            hasFlame = PlayerPrefs.GetInt("hasFlame") != 0;
        else
            hasFlame = false;
    }
    public static void Save()
    {
        PlayerPrefs.SetInt("hasLantern", (hasLantern) ? 1 : 0);
        PlayerPrefs.SetInt("hasChain", (hasLantern) ? 1 : 0);
        PlayerPrefs.SetInt("hasDoubleJ", (hasLantern) ? 1 : 0);
        PlayerPrefs.SetInt("hasPoison", (hasLantern) ? 1 : 0);
        PlayerPrefs.SetInt("hasFlame", (hasLantern) ? 1 : 0);
    }
    public static void UpdateStats(int id, int type)
    {
        if (type == 0) //neutral upgrades
        {
            //
            switch (id)
            {
                case 0: //will - flinch time reduced
                    flinchMod += 0.2f;
                    break;
                case 1: //focus - attack speed increased
                    attackSpeedMod += 0.1f;
                    break;
                case 2: //??? - do more take less phys
                    damageDoneMod[0] += 0.1f;
                    damageTakenMod[0] -= 0.1f;
                    break;
                case 3: //still mind - flinch time reduced, attack speed increased
                    flinchMod += 0.2f;
                    attackSpeedMod += 0.1f;
                    break;
                case 4: //stone body - do more take less, increase flich time of monsters
                    damageDoneMod[0] += 0.1f;
                    damageTakenMod[0] -= 0.1f;
                    enemyFlinchMod += 0.25f;
                    break;
            }
        }
        else if (type == 1) //dark upgrades
        {
            //
            switch (id)
            {
                case 0://shadowed - do more, take less dark; take more light
                    damageDoneMod[1] += 0.1f;
                    damageTakenMod[1] -= 0.05f;
                    damageTakenMod[2] += 0.05f;
                    break;
                case 1:
                    damageDoneMod[1] += 0.15f;
                    damageTakenMod[1] -= 0.05f;
                    damageTakenMod[2] += 0.05f;
                    break;
                case 2:
                    break;
                case 3:
                    damageDoneMod[1] += 0.25f;
                    damageTakenMod[1] -= 0.05f;
                    damageTakenMod[2] += 0.1f;
                    break;
                case 4://umbral soul - attacks inflict poisoned (slowed)
                    hasPoison = true;
                    break;
            }
        }
        else if (type == 2) //light upgrades
        {
            //
            switch (id)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4://burning soul - attacks inflict burning (dot)                                      
                    hasFlame = true;
                    break;
            }
        }
    }
}
