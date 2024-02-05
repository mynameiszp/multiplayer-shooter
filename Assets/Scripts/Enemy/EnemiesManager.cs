using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EnemiesManager : NetworkBehaviour
{
    [Networked] private TickTimer _spawnTimer { get; set; }

    [SerializeField] private List<Vector2> _spawnPositions;
    [SerializeField] private int _firstWaveSpawnFrequency;
    [SerializeField] private int _goodsAmount;
    [SerializeField] private List<GameObject> _goodsPrefabs;
    private NetworkManager _networkManager;
    private int _amountOfBreaks;
    private int _waveNumber;
    private List<NetworkObject> _enemies = new List<NetworkObject>();
    //private List<NetworkObject> _updatedZombies;
    //private List<NetworkObject> _skeletons;
    //private List<NetworkObject> _goods;
    private List<PlayerController> _players = new List<PlayerController>();
    private WavesTimer _wavesTimer;
    private List<WavesData> _wavesData;
    private WavesData _waveData;
    private bool _waveStarted;

    public void Init(NetworkManager networkManager, List<WavesData> wavesDataList, List<EnemyData> enemyDataList)
    {
        //state authority?
        _wavesTimer = networkManager.GetWavesTimer();
        _wavesTimer.OnBreak += StartBreak;
        _wavesTimer.OnWaveStart += StartWave;
        _wavesData = wavesDataList;
        _networkManager = networkManager;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var item in _networkManager.GetComponent<NetworkManager>().GetSpawnedCharactersList())
        {
            _players.Add(item.GetComponent<PlayerController>());
        }
        //_simpleZombies = new List<NetworkObject>();
        //_updatedZombies = new List<NetworkObject>();
        //_skeletons = new List<NetworkObject>();
        //_goods = new List<NetworkObject>();
    }
    public void SpawnEnemies()
    {
        if (!HasStateAuthority) return;
        List<EnemyData> enemies = _waveData.enemies;
        if (_spawnTimer.ExpiredOrNotRunning(Runner) && enemies.Count > 0)
        {
            _spawnTimer = TickTimer.CreateFromSeconds(Runner, _firstWaveSpawnFrequency);
            SpawnEnemy(enemies[Random.Range(0, enemies.Count)]);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_waveStarted)
        {
            SpawnEnemies();
        }
    }

    private void StartBreak()
    {
        _waveStarted = false;
        for (int i = 0; i < _enemies.Count; i++)
        {
            Runner.Despawn(_enemies[i]);
        }
        _enemies.Clear();
    }

    private void StartWave(WavesData data)
    {
        _waveStarted = true;
        _waveData = data;
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
        enemy.OnDestroy += RemoveFromList;
        _enemies.Add(enemyNetworkObject);
        return enemyNetworkObject;
    }

    private void RemoveFromList(Enemy enemy)
    {
        _enemies.Remove(enemy.GetComponent<NetworkObject>());
    }
}
