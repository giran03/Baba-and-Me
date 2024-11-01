using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeerHandler : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100;
    [SerializeField] GameObject customDrop;
    public Sound[] deerSFX;

    public GameObject attachedObject;

    GameObject _HealthBarImage;
    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, _health, DamageableStats.DamageMultiplier.Normal);
        _HealthBarImage = transform.GetChild(0).Find("Enemy Health").Find("HP Bar Border").gameObject;
        _HealthBarImage.SetActive(false);
    }

    private void OnDestroy()
    {
        if (attachedObject != null)
        {
            attachedObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f, ForceMode.Impulse);
            if (attachedObject.TryGetComponent<DropTrigger>(out var dropTrigger))
                dropTrigger.enabled = true;
        }
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        //sfx
        StartCoroutine(PlayWithCooldown(deerSFX[0]));
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, Random.Range(5, 8), _HealthBarImage, customDrop);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();

        if (attachedObject != null)
            attachedObject.transform.position = transform.position + Vector3.up * 1.7f;
    }

    IEnumerator PlayWithCooldown(Sound _sfx)
    {
        _sfx.Play(transform.position);
        yield return new WaitForSeconds(_sfx.Audio.length);
    }
}