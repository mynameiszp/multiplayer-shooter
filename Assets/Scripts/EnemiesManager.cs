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
    [SerializeField] private NetworkManager _networkManager;

    private List<NetworkObject> _simpleZombies;
    private List<NetworkObject> _updatedZombies;
    private List<NetworkObject> _skeletons;
    private List<NetworkObject> _goods;
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
        _simpleZombies = new List<NetworkObject>();
        _updatedZombies = new List<NetworkObject>();
        _skeletons = new List<NetworkObject>();
        _goods = new List<NetworkObject>();
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
        enemy.Players = _players;
        enemy.Harm = enemyData.harm;
        enemy.AttackFrequency = enemyData.attackFrequency;
        return enemyNetworkObject;
    }

    //public void FixedUpdate()
    //{
    //    if (_manager.Runner.IsServer && _manager.GetComponent<NetworkManager>().GetSpawnedCharactersList().Count == 2)
    //    {
    //        _players = _manager.GetComponent<NetworkManager>().GetSpawnedCharactersList();
    //        SpawnInFirstWave();
    //    }
    //}
    //void Start()
    //{
    //        GameObject temp;
    //    for (int i = 0; i < objectsAmount; i++)
    //    {
    //        temp = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)]);
    //        temp.AddComponent<ObstacleMovement>();
    //        temp.SetActive(false);
    //        pooledObjects.Add(temp);
    //    }
    //}

    //public int GetPooledObjectIndex()
    //{
    //    for (int i = 0; i < pooledObjects.Count; i++)
    //    {
    //        if (!pooledObjects[i].activeInHierarchy)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

    //public List<GameObject> GetPooledObjects()
    //{
    //    return pooledObjects;
    //}

    //public GameObject GetPreviousObject(int currentIndex)
    //{
    //    if (currentIndex == 0) return pooledObjects[pooledObjects.Count - 1];
    //    if (currentIndex < pooledObjects.Count) return pooledObjects[currentIndex - 1];
    //    return null;
    //}

    //public void DeactivateObject(GameObject obj)
    //{
    //    if (pooledObjects.Contains(obj)) obj.SetActive(false);
    //}
    //public void DeactivateAll()
    //{
    //    foreach (GameObject obj in pooledObjects)
    //    {
    //        obj.SetActive(false);
    //    }
    //}

    //public void ActivateAll()
    //{
    //    foreach (GameObject obj in pooledObjects)
    //    {
    //        obj.SetActive(false);
    //    }
    //}
}
