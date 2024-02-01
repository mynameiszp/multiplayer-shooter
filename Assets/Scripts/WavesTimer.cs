using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesTimer : NetworkBehaviour
{
    [Networked] public TickTimer WaveTimer { get; private set; }
    [Networked] public TickTimer BreakTimer { get; private set; }
    private int _amountOfBreaks;
    private int _waveNumber;
    private List<WavesData> _wavesData;
    public int WaveNumber => _waveNumber;
    public int AmountOfBreaks => _amountOfBreaks;
    public Action OnBreak;
    public Action<WavesData> OnWaveStart;
    public void Init(List<WavesData> wavesData)
    {
        _wavesData = wavesData;
    }
    public override void FixedUpdateNetwork()
    {
        if (WaveTimer.Expired(Runner) && BreakTimer.ExpiredOrNotRunning(Runner) && _waveNumber - _amountOfBreaks == 1)
        {
            StartBreak();
            _amountOfBreaks++;
            OnBreak?.Invoke();
        }
        else if (WaveTimer.ExpiredOrNotRunning(Runner) && _waveNumber == _amountOfBreaks && BreakTimer.ExpiredOrNotRunning(Runner) && _waveNumber < _wavesData.Count - 1)
        {
            StartNextWave(_wavesData[_waveNumber].waveDuration);
            OnWaveStart?.Invoke(_wavesData[_waveNumber]);
            _waveNumber++;
        }
    }
    private void StartNextWave(float duration)
    {
        WaveTimer = TickTimer.CreateFromSeconds(Runner, duration);
    }
    private void StartBreak()
    {
        BreakTimer = TickTimer.CreateFromSeconds(Runner, _wavesData[_wavesData.Count - 1].waveDuration);
    }
}
