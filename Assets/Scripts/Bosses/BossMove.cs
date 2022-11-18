using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    //movement class for linear boss movement
    public float duration;
    public float progress;
    public Vector3 offset;
    protected Vector3 _startPos;
    public Vector3 startPos
    {
        get { return _startPos; }
        set { _startPos = value; }
    }
    public void StartMove(Vector3 startPos)
    {
        this.startPos = startPos;
        progress = 0f;
    }
    
    public bool IsDone()
    {
        return (progress >= duration) ? true : false;
    }
    public Vector3 LerpedPos(int facing)
    {
        Vector3 facedOffset = new Vector3(offset.x * facing, offset.y, offset.z);
        progress += Time.deltaTime;
        if (IsDone())
            return startPos + facedOffset;
        return Vector3.Lerp(startPos, startPos + facedOffset, progress / duration);
    }
}
