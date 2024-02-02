using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private NetworkManager _networkManager;
    //[SerializeField] private CameraManager _cameraManager;
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private TMP_Text _timerText;
    [Header("Waves Characteristics")]
    [Space]
    [SerializeField] private WavesData _firstWaveData;
    [SerializeField] private WavesData _secondWaveData;
    [SerializeField] private WavesData _thirdWaveData;
    [SerializeField] private WavesData _breakWaveData;
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
    private WavesTimer _timer;
    private List<WavesData> _wavesData;
    

    private void Awake()
    {
        _weaponManager.Init(_bulletPrefab, new List<WeaponData>() { _gunData, _shotgunData, _rifleData });
        _networkManager.PlayersPresent += InitializeManagers;
        _wavesData = new List<WavesData> { _firstWaveData, _secondWaveData, _thirdWaveData, _breakWaveData };
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
    private void InitializeManagers()
    {
        _enemiesManager = _networkManager.GetEnemiesManager();
        _enemiesManager.GetComponent<EnemiesManager>().Init(_networkManager, _wavesData, new List<EnemyData>() { _simpleZombieData, _upgradedZombieData, _skeletonData });
        //_cameraManager.Init(_networkManager);
        _timer = _networkManager.GetWavesTimer();
        _timer.Init(_timerText, _wavesData);
    }
}
