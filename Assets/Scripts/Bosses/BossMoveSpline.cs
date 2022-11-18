using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveSpline : BossMove
{
    public BezierSpline spline;

    public new Vector3 LerpedPos(int facing)
    {
        progress += Time.deltaTime / duration;
        Vector3 facedSpline;
        if (IsDone())
            facedSpline = spline.GetPoint(1f);
        else
            facedSpline = spline.GetPoint(progress);
        facedSpline.x *= facing;
        
        return facedSpline;
    }
}
