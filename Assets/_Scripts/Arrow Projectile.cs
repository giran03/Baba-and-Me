using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Tooltip("Name of the ATTACK TYPE; Must be existing in Player Configs!")]
    public string attackName;
    public Sound arrowHitSFX;
    AttackStats attackStats;

    private void Start()
    {
        Debug.Log($"Starting ArrowProjectile: {attackName}");
        attackStats = PlayerConfigs.Instance.FindAttackObject(attackName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player Attacking: {other.name} using an arrow!");
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(attackStats.Damage, attackStats.CriticalDamage, attackStats.CriticalChance);
            GetComponent<BoxCollider>().enabled = false;

            Destroy(gameObject);
            // attach to object
            // var joint = gameObject.AddComponent<FixedJoint>();
            // joint.connectedBody = other.attachedRigidbody;
        }

        arrowHitSFX.Play(transform.position);
        Destroy(gameObject);
    }
}
