using UnityEngine;

public class Rocks : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100;

    public DamageableStats.DamageMultiplier objectType;

    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, _health, objectType);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, Random.Range(1,4));
    }

    private void Update()
    {
        damageableStats.UpdateDamage();
    }
}
