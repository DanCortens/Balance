using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack 
{
    //each 
    public class Move
    {
        public float windup;    //time for the move to start
        public float time;      //time for the move to complete after windup
        public Vector2 moveBy;  //boss moves by how much
        public bool isAttack;   //bool for checking if move or attack
        public Move (float windup, float time, Vector2 moveBy)
        {
            this.windup = windup;
            this.time = time;
            this.moveBy = moveBy;
            isAttack = false;
        }
        public float TotalTime()
        {
            return windup + time;
        }
        
    }
    public class Swing : Move
    {
        public Vector2 offset;  //distance relative to boss that the swing hits
        public float range;     //radius of the swing
        public float damage;
        public int type;        //type of damage of the swing
        
        public Swing(float windup, float time, Vector2 moveBy, Vector2 offset, float range, float damage, int type) 
            : base (windup, time, moveBy)
        {

            this.offset = offset;
            this.range = range;
            this.damage = damage;
            this.type = type;
            isAttack = true;
        }
    }
    public Move[] moves;  //array of swings in the attack
    public string animName; //name of attack animation
    public bool counterable;//whether or not the attack is counterable

    public BossAttack(Move[] m, string an, bool c)
    {
        moves = m;
        animName = an;
        counterable = c;
    }
}
/*
 * public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
	}
 * 
 */