using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [Header("Waves Duration (seconds)")]
    [Space]
    [SerializeField] private WavesData _wavesData;
    [Header("Zombies Characteristics")]
    [Space]
    [SerializeField] private EnemyData _simpleZombieData;
    [SerializeField] private EnemyData _upgradedZombieData;
    [SerializeField] private EnemyData _skeletonData;
    [Header("Weapons Characteristics")]
    [Space]
    [SerializeField] private WeaponData _gunData;
    [SerializeField] private WeaponData _shotgunData;
    [SerializeField] private WeaponData _rifleData;

    private NetworkObject _enemiesManager;
    

    private void Awake()
    {
        _weaponManager.Init(_bulletPrefab, new List<WeaponData>() { _gunData, _shotgunData, _rifleData });
        _networkManager.PlayersPresent += InitializeEnemyManager;
    }
    private void Start()
    {
        StartGame();
    }
    public bool HasStartedGame { get; set; }
    public void StartGame()
    {
        HasStartedGame = true;
    }
    public void FinishGame()
    {
        HasStartedGame = false;
    }
    private void InitializeEnemyManager()
    {
        _enemiesManager = _networkManager.GetEnemiesManager();
        _enemiesManager.GetComponent<EnemiesManager>().Init(_networkManager, _wavesData, new List<EnemyData>() { _simpleZombieData, _upgradedZombieData, _skeletonData });
    }
}
