using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipCont : MonoBehaviour
{
    
    void Update()
    {
        UpdatePos();
    }
    public void UpdatePos()
    {
        float mouseX = (Input.mousePosition.x < Screen.width - 250f) ? 
            Input.mousePosition.x + 130f : Input.mousePosition.x - 130f;
        float mouseY = Input.mousePosition.y;
        transform.position = new Vector3(mouseX, mouseY);
    }
}
