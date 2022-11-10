using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEffect : MonoBehaviour
{
    public float maxSize;
    public float growSpeed;
    public float rotSpeed;

    public void PlayEffect()
    {
        InvokeRepeating("Grow", 0f, 0.05f);
    }

    private void Grow()
    {
        if (transform.localScale.x > maxSize)
        {
            CancelInvoke("Grow");
            InvokeRepeating("Shrink", 0f, 0.05f);
        }
        else
        {
            transform.localScale += (new Vector3(1f,1f,1f) * growSpeed);
            transform.Rotate(new Vector3(0f, 0f, 1f), rotSpeed);
        }
    }
    private void Shrink()
    {
        if (transform.localScale.x <= 0f)
        {
            CancelInvoke("Shrink");
        }
        else
        {
            transform.localScale -= (new Vector3(1f, 1f, 1f) * growSpeed);
            transform.Rotate(new Vector3(0f, 0f, 1f), rotSpeed);
        }
    }
}
