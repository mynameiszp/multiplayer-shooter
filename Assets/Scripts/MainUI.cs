using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private GameObject skinsPanel;
    private NetworkRunner _runner;

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        //gameObject.AddComponent<RunnerSimulatePhysics2D>();

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom", //paste real name
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnHost()
    {
        StartGame(GameMode.Host);
    }
    public void OnJoin()
    {
        StartGame(GameMode.Client);
    }
    private void Awake()
    {
        skinsPanel.SetActive(false);
    }
    public void OnSkins()
    {
        skinsPanel.SetActive(true);
    }
    public void OnExit()
    {
        Application.Quit();
    }
    public void OnSaveSkins()
    {
        //save skin
        skinsPanel.SetActive(false);
    }

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector2 spawnPosition = new Vector2(0, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
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

    //public void OnEnable()
    //{
    //    _runner.AddCallbacks(this);
    //}

    //public void OnDisable()
    //{
    //    _runner.RemoveCallbacks(this);
    //}
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //var myInput = new NetworkInputData();

        //myInput.buttons.Set(InputButtons.Forward, Input.GetKey(KeyCode.W));
        //myInput.buttons.Set(InputButtons.Backward, Input.GetKey(KeyCode.S));
        //myInput.buttons.Set(InputButtons.Left, Input.GetKey(KeyCode.A));
        //myInput.buttons.Set(InputButtons.Right, Input.GetKey(KeyCode.D));

        //input.Set(myInput);
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.aimDirection += Vector2.up;

        if (Input.GetKey(KeyCode.S))
            data.aimDirection += Vector2.down;

        if (Input.GetKey(KeyCode.A))
            data.aimDirection += Vector2.left;

        if (Input.GetKey(KeyCode.D))
            data.aimDirection += Vector2.right;

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

}
