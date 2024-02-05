using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : NetworkBehaviour
{
    [Networked] private TickTimer _spawnTimer { get; set; }
    [SerializeField] private List<Vector2> _spawnPositions;
    [SerializeField] private float _spawnFrequency;
    private List<NetworkPrefabRef> _itemsInWave;
    private WavesTimer _wavesTimer;

    public void Init(NetworkManager networkManager)
    {
        _wavesTimer = networkManager.GetWavesTimer();
        _wavesTimer.OnWaveStart += StartWave;
    }
    public override void FixedUpdateNetwork()
    {
        if (_spawnTimer.ExpiredOrNotRunning(Runner))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (!HasStateAuthority) return;
        _spawnTimer = TickTimer.CreateFromSeconds(Runner, _spawnFrequency);
        if (_itemsInWave.Count > 0)
        {
            Runner.Spawn(_itemsInWave[Random.Range(0, _itemsInWave.Count)], _spawnPositions[Random.Range(0, _spawnPositions.Count)]);
        }
    }

    private void StartWave(WavesData data)
    {
        _itemsInWave = data.items;
    }
}