using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRestore : MonoBehaviour
{
    public int restore;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision: " + collision.gameObject.name);
        if (collision.gameObject.name == "player")
        {
            collision.gameObject.GetComponent<PlayerController>().CurrHp += restore;
            StartCoroutine(PickedUp());
        }
    }

    private IEnumerator PickedUp()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
