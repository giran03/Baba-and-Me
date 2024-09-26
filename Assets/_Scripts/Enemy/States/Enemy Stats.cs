using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] int _health = 100;

    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, DamageableStats.DamageMultiplier.Normal);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        Debug.Log($"PLAYER IS ATTACKING ME!!!");
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();
    }
}
