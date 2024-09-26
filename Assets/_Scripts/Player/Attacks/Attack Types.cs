using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTypes 
{
    public AttackTypes() { }

    public Attack CurrentAttack { get; set; }

    public enum Attack
    {
        BasicAttack,
        HeavyAttack,
        SpecialAttack,
        RangedAttack
    }
}