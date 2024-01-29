using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private NetworkManager _networkManager;
    [Header("Waves Duration (seconds)")]
    [SerializeField] private float _firstWaveDuration;
    [SerializeField] private float _secondWaveDuration;
    [SerializeField] private float _thirdWaveDuration;
    [SerializeField] private float _breakDuration;
    [Space]
    [Header("Zombies Characteristics")]
    [SerializeField] private EnemyData _simpleZombieData;
    [SerializeField] private EnemyData _upgradedZombieData;
    [SerializeField] private EnemyData _skeletonData;
    //[Header("Simple zombie")]
    //[SerializeField] private GameObject _simpleZombiePrefab;
    //[SerializeField] private float _simpleZombieHarm;
    //[SerializeField] private float _simpleZombieAttackFrequency;    
    //[Header("Upgraded zombie")]
    //[SerializeField] private GameObject _upgradedZombiePrefab;
    //[SerializeField] private float _upgradedZombieHarm;
    //[SerializeField] private float _upgradedZombieAttackFrequency;    
    //[Header("Skeleton")]
    //[SerializeField] private GameObject _skeletonPrefab;
    //[SerializeField] private float _skeletonHarm;
    //[SerializeField] private float _skeletonAttackFrequency;
    [Space]
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [Space]
    [Header("Weapons Characteristics")]

    [SerializeField] private WeaponData _gunData;
    [SerializeField] private WeaponData _shotgunData;
    [SerializeField] private WeaponData _rifleData;
    //[Header("Gun")]
    //[SerializeField] private GameObject _gunPrefab;
    //[SerializeField] private float _gunAttackDistance;
    //[SerializeField] private float _gunHarm;
    //[SerializeField] private float _gunAttackingEnemyNumber;    
    //[Header("Shotgun")]
    //[SerializeField] private GameObject _shotgunPrefab;
    //[SerializeField] private float _shotgunAttackDistance;
    //[SerializeField] private float _shotgunHarm;
    //[SerializeField] private float _shotgunAttackingEnemyNumber;    
    //[Header("Rifle")]
    //[SerializeField] private GameObject _riflePrefab;
    //[SerializeField] private float _rifleAttackDistance;
    //[SerializeField] private float _rifleHarm;
    //[SerializeField] private float _rifleAttackingEnemyNumber;
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
        //_enemiesManager.SetActive(true);
        _enemiesManager = _networkManager.GetEnemiesManager();
        _enemiesManager.GetComponent<EnemiesManager>().Init(_networkManager, new List<EnemyData>() { _simpleZombieData, _upgradedZombieData, _skeletonData });
    }
}
