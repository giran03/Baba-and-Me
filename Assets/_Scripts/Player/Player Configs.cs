using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Handles the Player Sprite Animation
/// </summary>
public class PlayerConfigs : MonoBehaviour
{
    public int PlayerHealth = 100;
    public static PlayerConfigs Instance;
    public List<AnimatorController> playerAnimationList;

    public int Health { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;
    }

    private void Start()
    {
        Health = PlayerHealth;
    }

    public void Damage()
    {
        Health-= 10;
        Debug.Log($"Damaged player! Health: {Health}");
    }
}
