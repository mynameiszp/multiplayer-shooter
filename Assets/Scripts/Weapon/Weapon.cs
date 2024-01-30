using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Networked] private TickTimer _attackTimer { get; set; }
    private float _attackFrequency = 0.5f;
    private NetworkPrefabRef _bullet;
    private List<NetworkObject> _bullets = new List<NetworkObject>();
    public List<NetworkObject> Bullets => _bullets;
    public float Damage {  get; set; }
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
            NetworkObject bulletNetworkObject = Runner.Spawn(_bullet, gameObject.transform.position);
            Bullet bullet = bulletNetworkObject.GetComponent<Bullet>();
            bullet.Damage = Damage;
            bullet.Disabled += DestroyBullet;
            _bullets.Add(bulletNetworkObject);
            bulletNetworkObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
        }
    }
    private void DestroyBullet(NetworkObject bullet)
    {
        if (!HasStateAuthority) return;
        _bullets.Remove(bullet);
        Runner.Despawn(bullet);
    }
    public override void FixedUpdateNetwork()
    {
        foreach (var item in _bullets)
        {
            item.transform.Translate(new Vector3(Runner.DeltaTime, 0, 0) * 8);
        }
    }

    public void Initialize(NetworkPrefabRef bullet, float attackDistance, float harm, float attackingEnemyNumber)
    {
        _bullet = bullet;
        Damage = harm;
        AttackDistance = attackDistance;
        AttackingEnemyNumber = attackingEnemyNumber;
    }
}
