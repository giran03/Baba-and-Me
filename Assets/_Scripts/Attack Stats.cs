using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Stats", menuName = "ScriptableObjects/Attack Stats")]
public class AttackStats : ScriptableObject
{
    public GameObject prefab;
    public float AttackSpeed;
    public float AttackDestroyTime;

    [MaxValue(10)]
    public float CriticalDamage;

    [MaxValue(1)]
    public float CriticalChance;
    public int Damage;
}