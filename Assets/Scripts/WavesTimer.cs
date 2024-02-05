using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavesTimer : NetworkBehaviour
{
    [Networked] public TickTimer WaveTimer { get; private set; }
    [Networked] public TickTimer BreakTimer { get; private set; }
    public int WaveNumber => _waveNumber;
    public int AmountOfBreaks => _amountOfBreaks;
    public Action OnBreak;
    public Action<WavesData> OnWaveStart;
    private int _amountOfBreaks;
    private int _waveNumber;
    private TMP_Text _timerText;
    private List<WavesData> _wavesData;
    private TickTimer _timer = TickTimer.None;
    public void Init(TMP_Text timerText, List<WavesData> wavesData)
    {
        _timerText = timerText;
        _wavesData = wavesData;
    }
    public override void FixedUpdateNetwork()
    {
        if (WaveTimer.Expired(Runner) && BreakTimer.ExpiredOrNotRunning(Runner) && _waveNumber - _amountOfBreaks == 1)
        {
            StartBreak();
            _timer = BreakTimer;
            _amountOfBreaks++;
            OnBreak?.Invoke();
        }
        else if (WaveTimer.ExpiredOrNotRunning(Runner) && _waveNumber == _amountOfBreaks && BreakTimer.ExpiredOrNotRunning(Runner) && _waveNumber < _wavesData.Count - 1)
        {
            StartNextWave(_wavesData[_waveNumber].waveDuration);
            _timer = WaveTimer;
            OnWaveStart?.Invoke(_wavesData[_waveNumber]);
            _waveNumber++;
        }
        else if (_waveNumber >= _wavesData.Count - 1 && _waveNumber == _amountOfBreaks)
        {
            _timer = TickTimer.None;
        }
        if (!_timer.Equals(TickTimer.None))
        {
            UpdateTimer();
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
    private void UpdateTimer()
    {
        float? remainingTime = _timer.RemainingTime(Runner);
        TimeSpan time = TimeSpan.FromSeconds((double)remainingTime);
        _timerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }
}
