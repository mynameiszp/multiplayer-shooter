using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EnemiesManager : NetworkBehaviour
{
    [Networked] private TickTimer _spawnTimer { get; set; }
    [Networked] private TickTimer _waveTimer { get; set; }

    [SerializeField] private List<Vector2> _spawnPositions;
    [SerializeField] private int _firstWaveSpawnFrequency;
    [SerializeField] private int _goodsAmount;
    [SerializeField] private List<GameObject> _goodsPrefabs;
    private NetworkManager _networkManager;

    //private List<NetworkObject> _simpleZombies;
    //private List<NetworkObject> _updatedZombies;
    //private List<NetworkObject> _skeletons;
    //private List<NetworkObject> _goods;
    private List<NetworkObject> _players;

    private EnemyData _simpleZombieData;
    private EnemyData _upgradedZombieData;
    private EnemyData _skeletonData;
    private WavesData _wavesData;

    public override void Spawned()
    {
        base.Spawned();
    }
    public void Init(NetworkManager networkManager, WavesData wavesData, List<EnemyData> enemyDataList)
    {
        _wavesData = wavesData;
        _networkManager = networkManager;
        if (enemyDataList.Count == 3)
        {
            _simpleZombieData = enemyDataList[0];
            _upgradedZombieData = enemyDataList[1];
            _skeletonData = enemyDataList[2];
        }
        Initialize();
    }

    private void Initialize()
    {
        _players = _networkManager.GetComponent<NetworkManager>().GetSpawnedCharactersList();
        //_simpleZombies = new List<NetworkObject>();
        //_updatedZombies = new List<NetworkObject>();
        //_skeletons = new List<NetworkObject>();
        //_goods = new List<NetworkObject>();
        SpawnInFirstWave();
    }
    public void SpawnInFirstWave()
    {
        _waveTimer = TickTimer.CreateFromSeconds(Runner, _wavesData.firstWaveDuration);
    }
    public override void FixedUpdateNetwork()
    {
        if (!_waveTimer.Expired(Runner))
        {
            if (_spawnTimer.ExpiredOrNotRunning(Runner))
            {
                _spawnTimer = TickTimer.CreateFromSeconds(Runner, _firstWaveSpawnFrequency);
                SpawnEnemy(_simpleZombieData);
            }
        }
    }

    private NetworkObject SpawnEnemy(EnemyData enemyData)
    {
        Enemy enemy;
        int vectorIndex = Random.Range(0, _spawnPositions.Count);
        NetworkObject enemyNetworkObject = _networkManager.Runner.Spawn(enemyData.prefab, _spawnPositions[vectorIndex]);
        enemy = enemyNetworkObject.GetComponent<Enemy>();
        enemy.Init(_players);
        enemy.Damage = enemyData.damage;
        enemy.AttackFrequency = enemyData.attackFrequency;
        enemy.Speed = enemyData.speed;
        enemy.Health = enemyData.health;
        return enemyNetworkObject;
    }
}
