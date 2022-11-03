using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectScript : MonoBehaviour
{
    protected SpriteRenderer mat;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Boom());
    }

    public void InitEffect(int t, float size)
    {
        mat = gameObject.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(size, size, 1f);
        if (t == 0)
            mat.color = new Color(0.4f, 0.8f, 0.8f, 0.75f);
        else if (t == 1)
            mat.color = new Color(0.8f, 0.2f, 1f, 0.75f);
        else
            mat.color = new Color(1f, 0.8f, 0.2f, 0.75f);
    }
    private IEnumerator Boom()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
