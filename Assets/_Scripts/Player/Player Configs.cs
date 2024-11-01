using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class PlayerConfigs : MonoBehaviour
{
    public static PlayerConfigs Instance;

    [Header("General Configs")]
    public float playerHealth = 100;
    public float playerPower = 0;
    public GameObject ui_CriticalHitPrefab;
    public bool IsGameOver { get; set; } = false;
    public float spriteDistanceToUpdate = 20;
    public bool CanMove { get; set; } = true;

    [Header("HUD")]
    public GameObject keyIconHolder;
    public GameObject KeyIconPrefab;
    public GameObject heartIconHolder;
    public GameObject heartIconPrefab;
    public Image playerHealthBarImage;
    public Image playerPowerBarImage;

    public int _livesRemaining = 3;


    [Header("Player Attack")]
    public List<AttackStats> attackList = new();
    List<AttackStats> defaultAttackStats = new();
    public GameObject[] ultimateAttackPrefab;

    [Header("Player Dash")]
    public float dashCooldown = 0.5f;
    public float dashSpeed = 20f;
    public float invincibilityDuration = .5f;

    [Header("Current Character")]
    public bool isMisha;

    [Header("Baba Animation")]
    public List<RuntimeAnimatorController> playerAnimationList;

    [Header("Misha Animation")]
    public List<RuntimeAnimatorController> MishaAnimationList;

    [Header("Drops")]
    public GameObject[] dropPrefab;
    public float dropRadius;
    public float dropDetectionRadius;

    //Scoring
    int playerScore = 0;


    [Header("SFX")]
    // player Death
    public Sound[] deathSFX;

    // attack
    public Sound[] bowSFX;
    public Sound[] normalSlashSFX;
    public Sound[] criticalSlashSFX;

    // puzzle
    public Sound[] boulderSFX;

    //movement
    public Sound[] dashSFX;
    public Sound gravelFootstepsSFX;

    // drops
    public Sound[] dropSFX;

    // references
    public AttackTypes attackTypes;
    private bool _isPlayingHealSFX;
    private bool _isPlayingPowerSFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;
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

    private void OnEnable()
    {
        defaultAttackStats = new List<AttackStats>(attackList);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "true");
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");

        foreach (AttackStats attack in attackList)
        {
            attack.name = defaultAttackStats.Find(x => x.name == attack.name).name;
            attack.AttackSpeed = defaultAttackStats.Find(x => x.name == attack.name).AttackSpeed;
            attack.AttackDestroyTime = defaultAttackStats.Find(x => x.name == attack.name).AttackDestroyTime;
            attack.arrowForce = defaultAttackStats.Find(x => x.name == attack.name).arrowForce;
            attack.prefab = defaultAttackStats.Find(x => x.name == attack.name).prefab;
        }
    }

    public void Heal(Collider other, float amount)
    {
        var playerHP = other.GetComponent<PlayerStateMachine>().playerHealth;
        playerHP = Mathf.Clamp(playerHP + amount, 0, 100);
        playerHealthBarImage.fillAmount = playerHP / 100f;
        other.GetComponent<PlayerStateMachine>().playerHealth = playerHP;

        if (!_isPlayingHealSFX)
            StartCoroutine(PlayHealSFX(other));
    }

    public void PowerIncrease(Collider other, float amount)
    {
        var playerPow = other.GetComponent<PlayerStateMachine>().playerPower;
        playerPow = Mathf.Clamp(playerPow + amount, 0, 100);
        playerPowerBarImage.fillAmount = playerPow / 100f;
        other.GetComponent<PlayerStateMachine>().playerPower = playerPow;
        other.GetComponent<PlayerStateMachine>().UpdatePowerBar();

        if (!_isPlayingPowerSFX)
            StartCoroutine(PlayPowerSFX(other));
    }

    IEnumerator PlayHealSFX(Collider other)
    {
        _isPlayingHealSFX = true;
        dropSFX[0].Play(other.transform.position);
        yield return new WaitForSeconds(1f);
        _isPlayingHealSFX = false;
    }

    IEnumerator PlayPowerSFX(Collider other)
    {
        _isPlayingPowerSFX = true;
        dropSFX[1].Play(other.transform.position);
        yield return new WaitForSeconds(1f);
        _isPlayingPowerSFX = false;
    }

    public void IncreaseScore(int amount)
    {
        playerScore += amount;
    }

    public void AddKeyIcon() => Instantiate(KeyIconPrefab, keyIconHolder.transform);

    public void RemoveKeyIcon()
    {
        if (keyIconHolder.transform.childCount > 0)
            Destroy(keyIconHolder.transform.GetChild(0).gameObject);
    }

    public void RemoveHeartIcon()
    {
        if (heartIconHolder.transform.childCount > 0)
            if (_livesRemaining > 0)
            {
                _livesRemaining--;
                Destroy(heartIconHolder.transform.GetChild(0).gameObject);
            }
    }
}