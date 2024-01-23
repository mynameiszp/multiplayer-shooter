using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Harm {  get; set; }
    public float AttackDistance {  get; set; }
    public float AttackingEnemyNumber {  get; set; }
    public void Attack()
    {

    }
    public void Initialize(float attackDistance, float harm, float attackingEnemyNumber)
    {
        Harm = harm;
        AttackDistance = attackDistance;
        AttackingEnemyNumber = attackingEnemyNumber;
    }
}
