using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody2D _networkRigidbody;
    [SerializeField] private Rigidbody2D _rigidbody;
    public float Damage { get; set; }
    public float AttackFrequency { get; set; }
    public float Speed { get; set; }
    public float Health { get; set; }
    private float _enemyFirstPlayerDistance;
    private float _enemySecondPlayerDistance;
    private NetworkObject _targetPlayer;
    private List<NetworkObject> _players;
    public void Init(List<NetworkObject> players)
    {
        _players = players;
        foreach (var player in _players)
        {
            player.GetComponent<PlayerController>().PlayerDead += ForgetPlayer;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            float distance = 0f;
            _targetPlayer = null;
            foreach (var player in _players)
            {
                if(Vector3.Distance(gameObject.transform.position, player.transform.position) >= distance)
                {
                    _targetPlayer = player;
                }
            }
            if (_targetPlayer != null)
            {
                Vector3 direction = (_targetPlayer.transform.position - gameObject.transform.position).normalized;
                Vector3 newPosition = transform.position + (Speed * Runner.DeltaTime * direction);
                _rigidbody.MovePosition(newPosition);
            }
        }
    }

    public virtual void Attack()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
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
    }

    public float GetHealth()
    {
        return Health;
    }

    private void ForgetPlayer(PlayerController player)
    {
        _players.Remove(player.gameObject.GetComponent<NetworkObject>());
    }
}
