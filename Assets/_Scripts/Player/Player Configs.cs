using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Player Sprite Animation
/// </summary>
public class PlayerConfigs : MonoBehaviour
{
    public static PlayerConfigs Instance;

    [Header("General Configs")]
    public Image playerHealthBarImage;
    public float playerHealth = 100;
    public GameObject ui_CriticalHitPrefab;
    public bool IsGameOver {get; set;} = false;

    [Header("Player Attack")]
    public List<AttackStats> attackList = new();

    [Header("Player Dash")]
    public float dashCooldown = 0.5f;
    public float dashSpeed = 20f;
    public float invincibilityDuration = .5f;

    [Header("Current Character")]
    public bool isMisha;

    [Header("Baba Animation")]
    public List<AnimatorController> playerAnimationList;

    [Header("Misha Animation")]
    public List<AnimatorController> MishaAnimationList;

    [Header("Drops")]
    public GameObject dropPrefab;
    public float dropRadius;
    public float dropDetectionRadius;

    // references
    public AttackTypes attackTypes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;

        ResetAllPrefs();
    }

    public AttackStats FindAttackObject(string attackName)
    {
        foreach (AttackStats attack in attackList)
        {
            if (attack.name == attackName)
                return attack;
        }
        return null;
    }

    public void ResetAllPrefs()
    {
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");
    }
}
