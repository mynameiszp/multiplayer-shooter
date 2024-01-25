using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;
using Zenject.SpaceFighter;

public class Player : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;
    //[Inject] 
    private WeaponManager _weaponManager;
    private NetworkObject _weapon;
    public override void Spawned()
    {
        ConfigureWeapon();
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
        if (!HasStateAuthority) return;
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            gameObject.transform.position += (Vector3)data.direction * Runner.DeltaTime * speed;
            _weaponManager.StartPlayersAttack(_weapon, data.aim); //not now
        }
    }
}
