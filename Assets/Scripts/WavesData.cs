using UnityEngine;

[CreateAssetMenu(fileName = "WavesData", menuName = "ScriptableObjects/WavesData", order = 1)]
public class WavesData: ScriptableObject
{
    public float firstWaveDuration;
    public float secondWaveDuration;
    public float thirdWaveDuration;
    public float breakDuration;
}