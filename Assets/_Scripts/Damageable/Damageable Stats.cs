using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance);
}

public class DamageableStats
{
    float _health;
    float _maxHealth;
    GameObject _currentObect;
    public DamageMultiplier _damageMultiplier;
    float spriteColorTimer = .2f;
    bool isAttacked;
    DropSpawner dropSpawner = new();

    public enum DamageMultiplier    // 100 = 10% , 10 = 1% , 5 = 0.5%
    {
        Fragile = 100,
        Normal = 10,
        Tanky = 3,
    }

    public DamageableStats(GameObject currentObject, float health, float maxHealth, DamageMultiplier damageMultiplier)
    {
        _damageMultiplier = damageMultiplier;
        _currentObect = currentObject;
        _health = health;
        _maxHealth = maxHealth;
    }

    public void Hit(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance, int dropCount, GameObject hpBar = null, GameObject customDrop = null)
    {
        float multiplier = (float)_damageMultiplier * .1f;

        float random = Random.Range(0f, 1f);
        if (random <= weaponCriticalChance)
        {
            _health -= (int)(damageAmount * multiplier * weaponCriticalDamage);

            CriticalOccured();

            // Debug.Log($"CRITICAL HIT!: {damageAmount * multiplier * weaponCriticalDamage}");
            // Debug.Log($"random value of: {random} with chance: {weaponCriticalChance}");

            PlayerConfigs.Instance.criticalSlashSFX[Random.Range(0, PlayerConfigs.Instance.criticalSlashSFX.Length)]
                        .Play(GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().CurrentState.CurrentContext.transform.position);
        }
        else
            _health -= (int)(damageAmount * multiplier);

        // Debug.Log($"Damage: {damageAmount * multiplier}");
        // Debug.Log($"Multiplier: {multiplier}");
        // Debug.LogError($"current object {_currentObect.name} | Health: {_health}");

        if (_currentObect != null)
            if (_currentObect.transform.childCount > 0)
            {
                _currentObect.transform.GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer);
                if (spriteRenderer != null)
                {
                    spriteRenderer.material.color = Color.red;
                    isAttacked = true;
                }
            }

        if (_health <= 0)
        {
            // spawn drops at destroy
            if (_currentObect != null)
                dropSpawner.SpawnClusterDrop(_currentObect, dropCount, customDrop);

            Object.Destroy(_currentObect);
        }

        if (hpBar != null)
            UpdateHealthBar(hpBar);
    }

    void CriticalOccured()
    {
        var criticalObject = Object.Instantiate(PlayerConfigs.Instance.ui_CriticalHitPrefab, _currentObect.transform.position + Vector3.up * 1.5f, Quaternion.identity);

        PlayerConfigs.Instance.StartCoroutine(DestroyCriticalObject(criticalObject));

        static IEnumerator DestroyCriticalObject(GameObject criticalObject)
        {
            yield return new WaitForSeconds(1f);
            Object.Destroy(criticalObject);
        }
    }

    void UpdateHealthBar(GameObject hpBar)
    {
        hpBar.SetActive(true);
        hpBar.transform.Find("Health Bar").GetComponent<Image>().fillAmount = _health / _maxHealth;
    }

    //TODO: CHANGE COLOR OR DO SOMETHING WHEN HIT!
    public void UpdateDamage()
    {
        if (!isAttacked) return;

        if (spriteColorTimer < 0)
        {
            _currentObect.GetComponentInChildren<SpriteRenderer>().material.color = Color.white;
            spriteColorTimer = .2f;
            isAttacked = false;
        }
        else
            spriteColorTimer -= Time.deltaTime;
    }
}
