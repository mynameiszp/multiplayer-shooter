using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject hostWeapon;
    private GameObject clientWeapon;
    private List<GameObject> availableWeapons = new List<GameObject>();
    public static WeaponManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this; //temporarys
    }

    public void Init(List<WeaponData> weaponDataList)
    {
        WeaponData hostWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        weaponDataList.Remove(hostWeaponData);
        WeaponData clientWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        hostWeapon = InstantiateWeapon(hostWeaponData);
        hostWeapon.SetActive(false);
        clientWeapon = InstantiateWeapon(clientWeaponData);
        clientWeapon.SetActive(false);
        availableWeapons.Add(hostWeapon);
        availableWeapons.Add(clientWeapon);
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

    public GameObject GetWeapon()
    {
        var weapon = availableWeapons[0];
        availableWeapons.Remove(weapon);
        return weapon;
    }
}
