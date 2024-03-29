using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private FixedJoystick _moveJoystick;
    [SerializeField] private FixedJoystick _shotJoystick;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _enemiesManagerPrefab;
    [SerializeField] private NetworkPrefabRef _wavesTimerPrefab;
    [SerializeField] private NetworkPrefabRef _itemsManagerPrefab;
    //[SerializeField] private NetworkPrefabRef _cameraManagerPrefab;
    private NetworkObject _enemiesManager;
    private WavesTimer _wavesTimer;
    private ItemsManager _itemsManager;
    //private NetworkObject _cameraManager;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters { get; set; }
    public NetworkRunner Runner { get; set; }
    public Action PlayersPresent;

    private void Awake()
    {
        StartGame();
    }
    //[ContextMenu("Start Session")]
    async void StartGame()
    {
        _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        // Create the Fusion runner and let it know that we will be providing user input
        //Runner = gameObject.AddComponent<NetworkRunner>();
        Runner = new GameObject()
        {
            name = "Runner"
        }.AddComponent<NetworkRunner>();
        Runner.gameObject.AddComponent<RunnerEnableVisibility>();
        Runner.AddCallbacks(this);
        Runner.ProvideInput = true;


        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "TestRoom", //paste real name
            //Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector2 spawnPosition = new Vector2(0, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            if(_spawnedCharacters.Count == 2)
            {
                _enemiesManager = runner.Spawn(_enemiesManagerPrefab);
                _wavesTimer = runner.Spawn(_wavesTimerPrefab).GetComponent<WavesTimer>();
                _itemsManager = runner.Spawn(_itemsManagerPrefab).GetComponent<ItemsManager>();
                PlayersPresent?.Invoke();
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData()
        {
            direction = _moveJoystick.Direction,
            aim = _shotJoystick.Direction
        };
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public List<NetworkObject> GetSpawnedCharactersList()
    {
        return new List<NetworkObject>(_spawnedCharacters.Values);
    }

    public NetworkObject GetEnemiesManager()
    {
        return _enemiesManager;
    }    
    public WavesTimer GetWavesTimer()
    {
        return _wavesTimer;
    }    
    public ItemsManager GetItemsManager()
    {
        return _itemsManager;
    }
}
