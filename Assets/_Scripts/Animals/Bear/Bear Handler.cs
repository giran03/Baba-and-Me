using UnityEngine;
using UnityEngine.UI;

public class BearHandler : MonoBehaviour, IDamageable
{
    [SerializeField] int _health = 100;

    GameObject _HealthBarImage;
    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, DamageableStats.DamageMultiplier.Normal);
        _HealthBarImage = transform.GetChild(0).Find("Enemy Health").Find("HP Bar Border").gameObject;
        _HealthBarImage.SetActive(false);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        Debug.Log($"PLAYER IS ATTACKING ME!!!");
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, _HealthBarImage);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();
    }
}