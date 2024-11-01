using UnityEngine;

public class MishaUltimate : MonoBehaviour
{
    // attack speed
    float _attackSpeedIncrease = 0.1f;
    float _defaultAttackSpeed;

    // crit chance
    float _critChanceIncrease = 1f;
    float _defaultCritChance;

    void Start()
    {
        // attack speed
        _defaultAttackSpeed = PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").AttackSpeed;
        PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").AttackSpeed = _attackSpeedIncrease;

        // crit chance
        _defaultCritChance = PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").CriticalChance;
        PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").CriticalChance = _critChanceIncrease;
    }

    private void OnDestroy()
    {
        PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").AttackSpeed = _defaultAttackSpeed;
        PlayerConfigs.Instance.attackList.Find(x => x.name == "Ranged Attack").CriticalChance = _defaultCritChance;
    }
}
