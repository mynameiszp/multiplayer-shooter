using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData: ScriptableObject
{
    public GameObject prefab;
    public float attackDistance;
    public float damage;
    public float attackingEnemyNumber;
}
