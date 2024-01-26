using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody2D _rigidbody;
    [SerializeField] private float _speed = 1f;
    public List<NetworkObject> Players { get; set; }
    public bool IsServer { get; set; }
    public float Health { get; set; }
    private float _enemyFirstPlayerDistance;
    private float _enemySecondPlayerDistance;
    private NetworkObject _targetPlayer;
    public override void FixedUpdateNetwork()
    {
        if (IsServer)
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
            _rigidbody.Teleport(newPosition);
        }
    }

    public virtual void Attack()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //take health
        }
    }
}
