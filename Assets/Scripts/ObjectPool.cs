using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using Zenject;

public class ObjectPool : NetworkBehaviour
{
    [SerializeField] private List<Vector2> _spawnPositions;
    [SerializeField] private int _enemiesAmount;
    [SerializeField] private int _goodsAmount;
    [SerializeField] private GameObject _simpleZombiePrefab;
    [SerializeField] private GameObject _updatedZombiePrefab;
    [SerializeField] private GameObject _skeletonPrefab;
    [SerializeField] private List<GameObject> _goodsPrefabs;
    [SerializeField] private NetworkManager _manager;

    private List<NetworkObject> _simpleZombies;
    private List<NetworkObject> _updatedZombies;
    private List<NetworkObject> _skeletons;
    private List<NetworkObject> _goods;
    private List<NetworkObject> _players;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _players = _manager.GetComponent<NetworkManager>().GetSpawnedCharactersList();
        _simpleZombies = new List<NetworkObject>();
        _updatedZombies = new List<NetworkObject>();
        _skeletons = new List<NetworkObject>();
        _goods = new List<NetworkObject>();
        SpawnInFirstWave();
    }
    public void SpawnInFirstWave()
    {
        int vectorIndex;
        NetworkObject enemy;
        for (int i = 0; i < _enemiesAmount; i++)
        {
            vectorIndex = Random.Range(0, _spawnPositions.Count);
            enemy = _manager.Runner.Spawn(_simpleZombiePrefab, _spawnPositions[vectorIndex]);
            _simpleZombies.Add(enemy);
            //_spawnPositions[vectorIndex] = new Vector2(_spawnPositions[vectorIndex].x + 0.1f, _spawnPositions[vectorIndex].y + 0.1f).normalized;
            enemy.GetComponent<Enemy>().Players = _players;
            enemy.GetComponent<Enemy>().IsServer = _manager.Runner.IsServer;
        }
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
