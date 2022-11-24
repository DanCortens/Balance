using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{
void OnTriggerEnter2D(Collider2D other)
{
FindObjectOfType<AudioManager>().Play("temple");
FindObjectOfType<AudioManager>().Pause("Start");
GetComponent<Collider2D>().enabled = false;
}
}
