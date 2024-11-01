using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WolfHandler : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100;
    public Sound[] wolfSFX;

    GameObject _HealthBarImage;
    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, _health, DamageableStats.DamageMultiplier.Normal);
        _HealthBarImage = transform.GetChild(0).Find("Enemy Health").Find("HP Bar Border").gameObject;
        _HealthBarImage.SetActive(false);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        //sfx
        StartCoroutine(PlayWithCooldown(wolfSFX[0]));
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, Random.Range(5, 11), _HealthBarImage);
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