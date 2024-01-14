using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeDamage : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;

    void Awake()
    {
        GetComponent<Hurtbox>().attackDamage = _weapon.damage;
    }
}
