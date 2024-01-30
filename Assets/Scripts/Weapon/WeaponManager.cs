using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject _hostWeapon;
    private GameObject _clientWeapon;
    public NetworkPrefabRef Bullet { get; private set; }
    private List<Weapon> _weapons = new List<Weapon>();
    private List<WeaponData> _availableWeapons = new List<WeaponData>();
    public static WeaponManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init(NetworkPrefabRef bullet, List<WeaponData> weaponDataList)
    {
        Bullet = bullet;
        WeaponData hostWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        weaponDataList.Remove(hostWeaponData);
        WeaponData clientWeaponData = weaponDataList[Random.Range(0, weaponDataList.Count)];
        _availableWeapons.Add(hostWeaponData);
        _availableWeapons.Add(clientWeaponData);
    }

    public WeaponData GetWeapon()
    {
        var weapon = _availableWeapons[0];
        _availableWeapons.Remove(weapon);
        return weapon;
    }
    public void AddWeaponToList(Weapon obj)
    {
        _weapons.Add(obj);
    }

    public void StartPlayersAttack(Weapon weapon, Vector2 direction)
    {
        weapon.Attack(direction);
    }
}
