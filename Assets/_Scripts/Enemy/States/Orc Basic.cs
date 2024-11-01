using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrcBasic : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100;
    [SerializeField] GameObject customDrop;
    public Sound[] orcBasicSFX;
    public DamageableStats.DamageMultiplier damageMultiplier = DamageableStats.DamageMultiplier.Normal;

    public GameObject attachedObject;
    public bool isBoss;
    public string musicName;

    GameObject _HealthBarImage;
    DamageableStats damageableStats;

    private void Start()
    {
        damageableStats = new(gameObject, _health, _health, damageMultiplier);
        _HealthBarImage = transform.GetChild(0).Find("Enemy Health").Find("HP Bar Border").gameObject;
        _HealthBarImage.SetActive(false);
    }

    private void Update()
    {
        damageableStats.UpdateDamage();

        if (attachedObject != null)
            attachedObject.transform.position = transform.position + Vector3.up * 1f;
    }

    private void OnDestroy()
    {
        if (isBoss)
            MusicManager.Instance.PlayMusic(musicName);

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
        StartCoroutine(PlayWithCooldown(orcBasicSFX[0]));
        damageableStats.Hit(damageAmount, weaponCriticalDamage, weaponCriticalChance, Random.Range(2, 5), _HealthBarImage, customDrop);
    }


    IEnumerator PlayWithCooldown(Sound _sfx)
    {
        _sfx.Play(transform.position);
        yield return new WaitForSeconds(_sfx.Audio.length);
    }
}
