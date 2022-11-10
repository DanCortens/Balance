using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Interactable
{
    public int id;
    private bool destroying;
    public override void Interact()
    {
        if (!destroying)
        {
            PlayerPrefs.SetInt("hpUpgrade" + id, id);
            PlayerStats.maxHPMod += 10;
            PlayerStats.unsavedHPUps.Add(id);

            destroying = true;
            //play pick up anim + sound
            StartCoroutine(Pickup());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetSaved();
        destroying = false;
        if (PlayerPrefs.HasKey("hpUpgrade" + id))
            Destroy(gameObject);
    }
    public void ResetSaved()
    {
        PlayerPrefs.DeleteKey("hpUpgrade" + id);
    }

    private IEnumerator Pickup()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
