using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomStartCollider : MonoBehaviour
{
    public BossBaseAI boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.PlayIntro();
    }
}
