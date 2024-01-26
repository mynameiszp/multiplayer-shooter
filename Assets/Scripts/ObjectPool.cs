using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using Zenject;

public class ObjectPool : NetworkBehaviour
{
    private List<NetworkObject> _simpleZombies;
    private List<NetworkObject> _updatedZombies;
    private List<NetworkObject> _skeletons;
    private List<NetworkObject> _goods;
    [SerializeField] private List<Vector2> _spawnPositions;
    [SerializeField] private int enemiesAmount;
    [SerializeField] private int goodsAmount;
    [SerializeField] private GameObject _simpleZombiePrefab;
    [SerializeField] private GameObject _updatedZombiePrefab;
    [SerializeField] private GameObject _skeletonPrefab;
    [SerializeField] private List<GameObject> _goodsPrefabs;
    [SerializeField] private NetworkManager _manager;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _simpleZombies = new List<NetworkObject>();
        _updatedZombies = new List<NetworkObject>();
        _skeletons = new List<NetworkObject>();
        _goods = new List<NetworkObject>();
        SpawnInFirstWave(_manager.Runner);
    }
    public void SpawnInFirstWave(NetworkRunner runner)
    {

        for (int i = 0; i < enemiesAmount; i++)
        {
            _simpleZombies.Add(runner.Spawn(_simpleZombiePrefab, _spawnPositions[Random.Range(0, _spawnPositions.Count)]));
        }

    }
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
