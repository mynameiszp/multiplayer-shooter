using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Networked] private TickTimer _attackTimer { get; set; }
    private float _attackFrequency;
    private NetworkPrefabRef _bullet;
    private bool _attacked;
    public float Harm {  get; set; }
    public float AttackDistance {  get; set; }
    public float AttackingEnemyNumber {  get; set; }
    public void Attack()
    {
        _attacked = true;
        _attackTimer = TickTimer.CreateFromSeconds(Runner, AttackDistance);
        Runner.Spawn(_bullet, gameObject.transform.position); //move it
    }
    public override void FixedUpdateNetwork()
    {
        if (_attacked)
        {
            if (_attackTimer.Expired(Runner))
            {
                Attack();
            }
        }
    }

    public void Initialize(NetworkPrefabRef bullet, float attackDistance, float harm, float attackingEnemyNumber)
    {
        _bullet = bullet;
        Harm = harm;
        AttackDistance = attackDistance;
        AttackingEnemyNumber = attackingEnemyNumber;
    }
}
