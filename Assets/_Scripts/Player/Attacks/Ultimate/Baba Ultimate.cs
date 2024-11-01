using System.Collections;
using UnityEngine;

public class BabaUltimate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(50, 0, 0);
            StartCoroutine(DamageOverTime(damageable, 75, 0.9f));
        }
    }

    IEnumerator DamageOverTime(IDamageable damageable, int damageAmount, float timeBetweenTicks)
    {
        while (damageable != null)
        {
            yield return new WaitForSeconds(timeBetweenTicks);
            damageable.Damage(damageAmount, 0, 0);
        }
    }
}