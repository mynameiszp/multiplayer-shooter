using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;
using Zenject.SpaceFighter;

public class Player : NetworkBehaviour
{
    //[Inject] 
    private WeaponManager _weaponManager;
    public override void Spawned()
    {
        ConfigureWeapon();
        _weaponManager.StartPlayersAttack(); //not now
    }
    private void ConfigureWeapon()
    {
        _weaponManager = WeaponManager.Instance;
        WeaponData weaponData = _weaponManager.GetWeapon();

        Debug.Log(weaponData.attackingEnemyNumber);

        NetworkObject weapon = Runner.Spawn(weaponData.prefab);
        weapon.transform.parent = gameObject.transform;

        Debug.Log(weapon.transform.parent);

        _weaponManager.AddWeaponToList(weapon);
        weapon.GetComponent<Weapon>().Initialize(_weaponManager.Bullet, weaponData.attackDistance, weaponData.harm, weaponData.attackingEnemyNumber);
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            gameObject.transform.position += (Vector3)data.direction * Runner.DeltaTime;
        }
    }
}
