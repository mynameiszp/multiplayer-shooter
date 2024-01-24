using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;

public class Player : NetworkBehaviour
{
    //[Inject] 
    private WeaponManager _weaponManager;

    private void Start()
    {
        _weaponManager = WeaponManager.Instance;
        GameObject weapon = _weaponManager.GetWeapon();
        weapon.SetActive(true);
        weapon.transform.parent = gameObject.transform;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (GetInput(out NetworkInputData data))
        {
            data.aimDirection.Normalize();
            gameObject.transform.position += (Vector3)data.aimDirection * Runner.DeltaTime;
        }
    }
}
