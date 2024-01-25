using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Networked] private TickTimer _attackTimer { get; set; }
    private float _attackFrequency = 0.5f;
    private NetworkPrefabRef _bullet;
    //private bool _attacked;
    private List<NetworkObject> _bullets = new List<NetworkObject>(); 
    public float Harm {  get; set; }
    public float AttackDistance {  get; set; }
    public float AttackingEnemyNumber {  get; set; }
    public override void Spawned()
    {
        _attackTimer = TickTimer.CreateFromSeconds(Runner, _attackFrequency);
    }
    public void Attack(Vector2 direction)
    {
        if (_attackTimer.Expired(Runner) && direction.sqrMagnitude > 0)
        {
            _attackTimer = TickTimer.CreateFromSeconds(Runner, _attackFrequency);
            NetworkObject bullet = Runner.Spawn(_bullet, gameObject.transform.position);
            bullet.GetComponent<Bullet>().Harm = Harm;
            _bullets.Add(bullet);
            bullet.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
            //bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }
            //_attacked = true;
        
        //move it
    }
    public override void FixedUpdateNetwork()
    {
        foreach (var item in _bullets)
        {
            //item.transform.position += item.transform.forward * Runner.DeltaTime;
            item.transform.Translate(new Vector3(Runner.DeltaTime, 0, 0) * 8);
        }
        //if (_attacked)
        //{
        //    Debug.Log("Attaced");
        //    //Debug.Log(_attackTimer.RemainingTicks(Runner));
        //    Debug.Log(GetInput(out NetworkInputData data));
        //    if (_attackTimer.Expired(Runner) /*&& GetInput(out NetworkInputData data)*/)
        //    {
        //        Debug.Log(data.aim);
        //        Attack(data.aim);
        //    }
        //}        
    }

    public void Initialize(NetworkPrefabRef bullet, float attackDistance, float harm, float attackingEnemyNumber)
    {
        _bullet = bullet;
        Harm = harm;
        AttackDistance = attackDistance;
        AttackingEnemyNumber = attackingEnemyNumber;
    }
}
