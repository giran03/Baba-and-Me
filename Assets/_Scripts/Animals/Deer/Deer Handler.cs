using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerHandler : MonoBehaviour, IDamageable
{
    [SerializeField] int _health = 100;

    public DamageableStats.DamageMultiplier objectType;

    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, objectType);
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
