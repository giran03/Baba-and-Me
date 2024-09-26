using NaughtyAttributes;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Tooltip("Name of the ATTACK TYPE; Must be existing in Player Configs!")]
    public string attackName;
    AttackStats attackStats;

    private void Start()
    {
        attackStats = PlayerConfigs.Instance.FindAttackObject(attackName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player Attacking: {other.name}!");
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(attackStats.Damage, attackStats.CriticalDamage, attackStats.CriticalChance);
        }
    }
}