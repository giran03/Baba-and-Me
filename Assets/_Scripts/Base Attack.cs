using UnityEngine;

public abstract class BaseAttack
{
    public GameObject Projectile { get; set; }
    public Transform FirePoint { get; set; }
    public float AttackSpeed { get; set; }
    public float NextAttackTime { get; set; }
    public float StartAttackTime { get; set; }
    public int Damage { get; set; }
    public float AttackRange { get; set; }
    public string AttackAnimation { get; set; }
    public float AttackRate { get; set; }
    public abstract void Attack();
}