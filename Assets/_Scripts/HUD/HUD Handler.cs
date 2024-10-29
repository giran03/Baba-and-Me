using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    public static HUDHandler Instance;

    [Header("Melee Attack")]
    [SerializeField] Image meleeAttackIcon;
    [SerializeField] Image meleeAttackCooldownIcon;

    [Header("Ranged Attack")]
    [SerializeField] Image rangedAttackIcon;
    [SerializeField] Image rangedAttackCooldownIcon;

    [Header("Dash")]
    [SerializeField] Image dashIcon;
    [SerializeField] Image dashCooldownIcon;

    [Header("Grab")]
    [SerializeField] Image grabIcon;
    [SerializeField] Image grabCooldownIcon;

    private bool _isFilling;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");
        PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "true");

        meleeAttackCooldownIcon.gameObject.SetActive(false);
        rangedAttackCooldownIcon.gameObject.SetActive(false);
        dashCooldownIcon.gameObject.SetActive(false);
        grabCooldownIcon.gameObject.SetActive(false);
    }

    public void StartIconCooldown(string iconName, float fillAmount)
    {
        switch (iconName)
        {
            case "Melee":
                FillIcon(meleeAttackCooldownIcon, fillAmount);
                break;

            case "Ranged":
                FillIcon(rangedAttackCooldownIcon, fillAmount);
                break;

            case "Dash":
                FillIcon(dashCooldownIcon, fillAmount);
                break;

            case "Grab":
                FillIcon(grabCooldownIcon, fillAmount);
                break;
        }
    }

    void FillIcon(Image icon, float fillAmount)
    {
        icon.gameObject.SetActive(true);
        icon.fillAmount = 1f;

        StartCoroutine(UpdateMeleeIconOverTime(icon, fillAmount));
    }


    IEnumerator UpdateMeleeIconOverTime(Image icon, float fillAmount)
    {
        _isFilling = true;
        float timeElapsed = 0;
        while (timeElapsed < fillAmount)
        {
            icon.fillAmount = 1f - (timeElapsed / fillAmount);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _isFilling = false;
        icon.gameObject.SetActive(false);
    }
}