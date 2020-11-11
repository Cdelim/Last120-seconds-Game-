using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty 
{
    static float maxDifSecond = 90f;
    public static float GetDif() {
        //Debug.Log(Mathf.Clamp01(Time.time / maxDifSecond));
        return Mathf.Clamp01(Time.time / maxDifSecond);
    }
}
