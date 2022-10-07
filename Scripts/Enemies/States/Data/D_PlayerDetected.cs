using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/State Data/Player Detected State")]
public class D_PlayerDetected : ScriptableObject
{
    //the time before entering charge state
    public float longRangeActionTime = 1.5f;
    //the time before entering attack state
    public float coolDown = 0.5f;
}
