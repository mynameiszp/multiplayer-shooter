using Cinemachine;
using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Networked] public bool IsDead { get; set; }
    [Networked] public Vector3 NetworkedScale { get; set; }
    public Action<PlayerController> PlayerDead;
    [SerializeField] private Canvas _deathCanvasPrefab;
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private float _health;
    [SerializeField] private NetworkMecanimAnimator _animator;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Rigidbody2D _rigidbody;
    private Canvas _deathCanvas;
    private float _initialHealth;
    private WeaponManager _weaponManager;
    private CinemachineVirtualCamera _camera;
    private Weapon _weapon;

    public override void Spawned()
    {
        _initialHealth = _health;
        if (HasInputAuthority)
        {
            _deathCanvas = Instantiate(_deathCanvasPrefab);
            _deathCanvas.enabled = false;
            _camera = Instantiate(_cameraPrefab).GetComponent<CinemachineVirtualCamera>();
            SetCameraTarget(this);
        }
        if (!HasStateAuthority) return;
        ConfigureWeapon();
        PlayerDead += Die;
        PlayerDead += DestroyWeapon;
    }
    public void TakeHealth(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            PlayerDead?.Invoke(this);
        }
    }

    private void ConfigureWeapon()
    {
        _weaponManager = WeaponManager.Instance;
        WeaponData weaponData = _weaponManager.GetWeapon();
        _weapon = Runner.Spawn(weaponData.prefab).GetComponent<Weapon>();
        _weapon.transform.parent = gameObject.transform;
        _weaponManager.AddWeaponToList(_weapon);
        _weapon.Initialize(_weaponManager.Bullet, weaponData.attackDistance, weaponData.damage, weaponData.attackingEnemyNumber);
    }
    public override void FixedUpdateNetwork()
    {
        //if (HasInputAuthority && IsDead)
        //{
        //    TurnOnObserverMode();
        //}
        var input = GetInput(out NetworkInputData data);
        if (!Runner.IsForward) return;
        if (!IsDead)
        {
            if (data.direction.magnitude > 0)
            {
                data.direction.Normalize();
                //gameObject.transform.position += Runner.DeltaTime * _speed * (Vector3)data.direction;
                _rigidbody.MovePosition(transform.position + Runner.DeltaTime * _speed * (Vector3)data.direction);
                if (data.direction.x > 0)
                {
                    NetworkedScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (data.direction.x < 0)
                {
                    NetworkedScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                transform.localScale = NetworkedScale;
                Run();
            }
            if (data.aim.magnitude > 0) _weaponManager.StartPlayersAttack(_weapon, data.aim); //not now
            if (data.direction.magnitude == 0) Idle();
        }
        else
        {
            _animator.Animator.SetBool(AnimationVariables.PLAYER_DEAD_ANIMATION, true);
            if (HasInputAuthority)
            {
                TurnOnObserverMode();
            }
        }
    }
    private void Run()
    {
        _animator.Animator.SetBool(AnimationVariables.PLAYER_RUN_ANIMATION, true);
    }
    private void Idle()
    {
        _animator.Animator.SetBool(AnimationVariables.PLAYER_RUN_ANIMATION, false);
    }
    private void Die(PlayerController player)
    {
        IsDead = true;
    }
    private void DestroyWeapon(PlayerController player)
    {
        Runner.Despawn(_weapon.GetComponent<NetworkObject>());
    }

    private void TurnOnObserverMode()
    {
        _deathCanvas.enabled = true;
    }

    public void SetCameraTarget(PlayerController player)
    {
        _camera.Follow = player.transform;
    }
    public void RecoverHealth()
    {
        _health = _initialHealth;
    }
}
