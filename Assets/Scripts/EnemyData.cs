using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData: ScriptableObject
{
    public GameObject prefab;
    public float damage;
    public float health;
    public float speed;
    public float attackFrequency;
}
