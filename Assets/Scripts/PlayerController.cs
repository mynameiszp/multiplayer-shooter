using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;
using Zenject.SpaceFighter;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkMecanimAnimator animator;
    [SerializeField] private float speed = 3f;
    [Inject] private PlayerAnimation _animation;
    private WeaponManager _weaponManager;
    private NetworkObject _weapon;
    public override void Spawned()
    {
        ConfigureWeapon();
        Debug.Log(_animation);
    }
    private void ConfigureWeapon()
    {
        _weaponManager = WeaponManager.Instance;
        WeaponData weaponData = _weaponManager.GetWeapon();
        _weapon = Runner.Spawn(weaponData.prefab);
        _weapon.transform.parent = gameObject.transform;
        _weaponManager.AddWeaponToList(_weapon);
        _weapon.GetComponent<Weapon>().Initialize(_weaponManager.Bullet, weaponData.attackDistance, weaponData.harm, weaponData.attackingEnemyNumber);
    }
    public override void FixedUpdateNetwork()
    {
        //if (!HasStateAuthority) return;
        var input = GetInput(out NetworkInputData data);
        if (Runner.IsForward)
        {
            if (data.direction.magnitude > 0)
            {
                data.direction.Normalize();
                gameObject.transform.position += Runner.DeltaTime * speed * (Vector3)data.direction;
                Run();
            }
            if (data.aim.magnitude > 0) _weaponManager.StartPlayersAttack(_weapon, data.aim); //not now
            if (data.direction.magnitude == 0) Idle();
        }
        
    }
    public void Run()
    {
        animator.Animator.SetBool("isRunning", true);
    }
    public void Idle()
    {
        animator.Animator.SetBool("isRunning", false);
    }
    public void Die()
    {
        animator.Animator.SetTrigger("Dead");
    }
}
