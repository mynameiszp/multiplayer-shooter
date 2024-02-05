using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private int _bulletCapacity;
    [Networked] private TickTimer _attackTimer { get; set; }
    private float _attackFrequency = 0.5f;
    private NetworkPrefabRef _bullet;
    private Dictionary<NetworkObject, Vector3> _bullets = new Dictionary<NetworkObject, Vector3>();
    private Stack<NetworkObject> _bulletsToDestroy = new Stack<NetworkObject>();
    private int _bulletsAmount;
    public float Damage { get; set; }
    public float AttackDistance { get; set; }
    public float AttackingEnemyNumber { get; set; }
    public override void Spawned()
    {
        _bulletsAmount = _bulletCapacity;
        _attackTimer = TickTimer.CreateFromSeconds(Runner, _attackFrequency);
    }
    public void Attack(Vector2 direction)
    {
        if (_attackTimer.Expired(Runner) && direction.sqrMagnitude > 0 && _bulletsAmount > 0)
        {
            _attackTimer = TickTimer.CreateFromSeconds(Runner, _attackFrequency);
            NetworkObject bulletNetworkObject = Runner.Spawn(_bullet, gameObject.transform.position);
            _bulletsAmount--;
            Bullet bullet = bulletNetworkObject.GetComponent<Bullet>();
            bullet.Damage = Damage;
            bullet.Disabled += DestroyBullet;
            _bullets.Add(bulletNetworkObject, bulletNetworkObject.transform.position);
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
        foreach (var bullet in _bullets.Keys)
        {
            bullet.transform.Translate(new Vector3(Runner.DeltaTime, 0, 0) * 8);
            if (Vector3.Distance(_bullets[bullet], bullet.transform.position) > AttackDistance)
            {
                _bulletsToDestroy.Push(bullet);
            }
        }
        while (_bulletsToDestroy.Count > 0)
        {
            var bullet = _bulletsToDestroy.Pop();
            DestroyBullet(bullet);
        }
    }

    public void Initialize(NetworkPrefabRef bullet, float attackDistance, float harm, float attackingEnemyNumber)
    {
        _bullet = bullet;
        Damage = harm;
        AttackDistance = attackDistance;
        AttackingEnemyNumber = attackingEnemyNumber;
    }

    public void RefillBullets()
    {
        _bulletsAmount = _bulletCapacity;
    }
}
