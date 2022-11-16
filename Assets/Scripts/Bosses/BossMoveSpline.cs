using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveSpline : BossMove
{
    public BezierSpline spline;

    public new Vector3 LerpedPos(int facing)
    {
        progress += Time.deltaTime;
        Vector3 facingMult = new Vector3(facing, 1f, 1f);
        if (IsDone())
            return startPos + spline.GetPoint(1f)*facing;
        return startPos + spline.GetPoint(progress / duration)*facing;
    }
}
