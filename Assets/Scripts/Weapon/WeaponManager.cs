using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject _hostWeapon;
    private GameObject _clientWeapon;
    public NetworkPrefabRef Bullet { get; set; }
    private List<NetworkObject> weapons = new List<NetworkObject>();
    private List<WeaponData> availableWeapons = new List<WeaponData>();
    public static WeaponManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this; //temporary
    }

    public void Init(NetworkPrefabRef bullet, List<WeaponData> weaponDataList)
    {
        Bullet = bullet;
        WeaponData hostWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        weaponDataList.Remove(hostWeaponData);
        WeaponData clientWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        //_hostWeapon = InstantiateWeapon(hostWeaponData);
        //_hostWeapon.SetActive(false);
        //_clientWeapon = InstantiateWeapon(clientWeaponData);
        //_clientWeapon.SetActive(false);
        availableWeapons.Add(hostWeaponData);
        availableWeapons.Add(clientWeaponData);
        //availableWeapons.AddRange(weapons);
    }
    //private GameObject InstantiateWeapon(WeaponData weaponData)
    //{
    //    GameObject _weapon = Instantiate(weaponData.prefab);
    //    Weapon weaponComponent = _weapon.GetComponent<Weapon>();
    //    if (weaponComponent != null)
    //    {
    //        weaponComponent.Initialize(Bullet, weaponData.attackDistance, weaponData.harm, weaponData.attackingEnemyNumber);
    //    }
    //    return _weapon;
    //}

    public WeaponData GetWeapon()
    {
        var weapon = availableWeapons[0];
        availableWeapons.Remove(weapon);
        return weapon;
    }    
    public void AddWeaponToList(NetworkObject obj)
    {
        weapons.Add(obj);
    }

    public void StartPlayersAttack(NetworkObject weapon, Vector2 direction)
    {
        //foreach (var weapon in weapons)
        //{
            weapon.GetComponent<Weapon>().Attack(direction);
        //}
    }
}
