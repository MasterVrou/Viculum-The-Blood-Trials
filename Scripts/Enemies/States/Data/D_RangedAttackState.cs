using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]
public class D_RangedAttackState : ScriptableObject
{
    public GameObject projectile;

    public float projectileDamage = 10;
    public float projectileSpeed = 12;
    public float projectileTravelDistance = 8;
}
