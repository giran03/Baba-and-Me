using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour, IDamageable
{
    [SerializeField] int _health = 100;

    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, DamageableStats.DamageMultiplier.Fragile);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();
    }
}
