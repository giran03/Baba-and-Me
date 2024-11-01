using System.Collections;
using UnityEngine;

public class BearHandler : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100;
    [SerializeField] GameObject customDrop;

    public Sound[] bearSFX;

    GameObject _HealthBarImage;
    DamageableStats damageableStats;


    private void Start()
    {
        damageableStats = new(gameObject, _health, _health, DamageableStats.DamageMultiplier.Tanky);
        _HealthBarImage = transform.GetChild(0).Find("Enemy Health").Find("HP Bar Border").gameObject;
        _HealthBarImage.SetActive(false);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        //sfx
        StartCoroutine(PlayWithCooldown(bearSFX[0]));
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, Random.Range(20, 50), _HealthBarImage);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();
    }

    IEnumerator PlayWithCooldown(Sound _sfx)
    {
        _sfx.Play(transform.position);
        yield return new WaitForSeconds(_sfx.Audio.length);
    }
}