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

        PlayerConfigs.Instance.normalSlashSFX[Random.Range(0, PlayerConfigs.Instance.normalSlashSFX.Length)].Play(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            damageable.Damage(attackStats.Damage, attackStats.CriticalDamage, attackStats.CriticalChance);
    }
}