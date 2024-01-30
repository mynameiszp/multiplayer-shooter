using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody2D _networkRigidbody;
    [SerializeField] private Rigidbody2D _rigidbody;
    public List<NetworkObject> Players { get; set; }
    public float Damage { get; set; }
    public float AttackFrequency { get; set; }
    public float Speed { get; set; }
    public float Health { get; set; }
    private float _enemyFirstPlayerDistance;
    private float _enemySecondPlayerDistance;
    private NetworkObject _targetPlayer;
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            _enemyFirstPlayerDistance = Vector3.Distance(gameObject.transform.position, Players[0].transform.position);
            _enemySecondPlayerDistance = Vector3.Distance(gameObject.transform.position, Players[1].transform.position);
            if (_enemyFirstPlayerDistance <= _enemySecondPlayerDistance)
            {
                _targetPlayer = Players[0];
            }
            else
            {
                _targetPlayer = Players[1];
            }
            Vector3 direction = (_targetPlayer.transform.position - gameObject.transform.position).normalized;
            Vector3 newPosition = transform.position + (Speed * Runner.DeltaTime * direction);
            //_networkRigidbody.Teleport(newPosition);
            _rigidbody.MovePosition(newPosition);
        }
    }

    public virtual void Attack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!HasStateAuthority) return;
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.TakeHealth(Damage);
        }
    }
    public void TakeHealth(float damage)
    {
        Health -= damage;
        Debug.Log(Health);
    }

    public float GetHealth()
    {
        return Health;
    }
}
