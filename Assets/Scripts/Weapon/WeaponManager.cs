using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject hostWeapon;
    private GameObject clientWeapon;
    private Dictionary<GameObject, string> weapons;
    [SerializeField] private float _gunAttackDistance;
    [SerializeField] private float _gunHarm;
    [SerializeField] private float _gunAttackingEnemyNumber;
    [SerializeField] private float _shotgunAttackDistance;
    [SerializeField] private float _shotgunHarm;
    [SerializeField] private float _shotgunAttackingEnemyNumber;
    [SerializeField] private float _rifleAttackDistance;
    [SerializeField] private float _rifleHarm;
    [SerializeField] private float _rifleAttackingEnemyNumber;
    public void SelectWeapon(List<WeaponData> weaponDataList)
    {
        WeaponData hostWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        weaponDataList.Remove(hostWeaponData);
        WeaponData clientWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        hostWeapon = InstantiateWeapon(hostWeaponData);
        clientWeapon = InstantiateWeapon(clientWeaponData);
    }
    private GameObject InstantiateWeapon(WeaponData weaponData)
    {
        GameObject weapon = Instantiate(weaponData.prefab);
        Weapon weaponComponent = weapon.GetComponent<Weapon>();
        if (weaponComponent != null)
        {
            weaponComponent.Initialize(weaponData.attackDistance, weaponData.harm, weaponData.attackingEnemyNumber);
        }
        return weapon;
    }
}
