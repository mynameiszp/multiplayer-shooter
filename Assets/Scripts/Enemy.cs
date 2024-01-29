using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody2D _networkRigidbody;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed = 1f;
    public List<NetworkObject> Players { get; set; }
    public float Harm { get; set; }
    public float AttackFrequency { get; set; }
    private float _health;
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
            Vector3 newPosition = transform.position + (_speed * Runner.DeltaTime * direction);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeHealth(Harm); ;
        }
    }
    public void TakeHealth(float damage)
    {
        _health -= damage;
    }

    public float GetHealth()
    {
        return _health;
    }
}
