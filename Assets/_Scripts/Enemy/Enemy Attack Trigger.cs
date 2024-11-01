using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [Tooltip("Name of the ATTACK TYPE; Must be existing in Player Configs!")]
    public AttackStats attackStatsObject;
    private bool _canDamagePlayer = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (_canDamagePlayer)
            {
                StartCoroutine(DamageCooldown());
                Debug.Log($"Attacking Player: {other.name}!");
                damageable.Damage(attackStatsObject.Damage, attackStatsObject.CriticalDamage, attackStatsObject.CriticalChance);
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        _canDamagePlayer = false;
        yield return new WaitForSeconds(1f);
        _canDamagePlayer = true;
    }
}
