using Fusion;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesData", menuName = "ScriptableObjects/WavesData", order = 1)]
public class WavesData: ScriptableObject
{
    public float waveDuration;
    public List<EnemyData> enemies;
    public List<NetworkPrefabRef> items;
}