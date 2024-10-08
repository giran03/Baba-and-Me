using System.Collections;
using UnityEngine;

public interface IDamageable
{
    void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance);
}

public class DamageableStats
{
    int _health;
    GameObject _currentObect;
    public DamageMultiplier _damageMultiplier;
    float spriteColorTimer = .2f;
    bool isAttacked;
    bool IsDrop { get; set; }

    public enum DamageMultiplier    // 100 = 10% , 10 = 1% , 5 = 0.5%
    {
        Fragile = 100,
        Normal = 10,
        Tanky = 3,
    }


    public DamageableStats(GameObject currentObject, int health, DamageMultiplier damageMultiplier)
    {
        _damageMultiplier = damageMultiplier;
        _currentObect = currentObject;
        _health = health;
    }

    public void Hit(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        float multiplier = (float)_damageMultiplier * .1f;

        float random = Random.Range(0f, 1f);
        if (random <= weaponCriticalChance)
        {
            _health -= (int)(damageAmount * multiplier * weaponCriticalDamage);

            CriticalOccured();

            Debug.Log($"CRITICAL HIT!: {damageAmount * multiplier * weaponCriticalDamage}");
            Debug.Log($"random value of: {random} with chance: {weaponCriticalChance}");
        }
        else
            _health -= (int)(damageAmount * multiplier);

        Debug.Log($"Damage: {damageAmount * multiplier}");
        Debug.Log($"Multiplier: {multiplier}");
        Debug.Log($"Health: {_health}");

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
            Object.Destroy(_currentObect);
            _currentObect.SetActive(false);

            SpawnDrop();
        }
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


    void SpawnDrop()
    {
        var defaultSpawn = _currentObect.transform.position + Vector3.up * .5f;

        GameObject drop = Object.Instantiate(PlayerConfigs.Instance.dropPrefab, defaultSpawn, Quaternion.identity);

        drop.transform.LookAt(GetRandomPointInRadius(PlayerConfigs.Instance.dropRadius), Vector3.up);
        drop.GetComponent<Rigidbody>().AddForce(drop.transform.forward * 2.5f, ForceMode.Impulse);
    }

    public Vector3 GetRandomPointInRadius(float radius)
    {
        Vector3 pointAboveTerrain;
        RaycastHit hit;
        do
        {
            float randomX = Random.Range(-radius, radius);
            float randomZ = Random.Range(-radius, radius);

            pointAboveTerrain = _currentObect.transform.position + new Vector3(randomX, 100, randomZ);

        } while (Physics.Raycast(pointAboveTerrain, Vector3.down, out hit, 200f, LayerMask.GetMask("Terrain")) == false);

        Vector3 targetPoint = hit.point;
        targetPoint.y += 4f;   // offset from ground

        return targetPoint;
    }
}
