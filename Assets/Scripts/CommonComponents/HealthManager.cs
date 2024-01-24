using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour, IOnHitSubscriber
{
    public int maxHealth = 10;

    [field: SerializeField]
    public int CurrentHealth { get; set; }

    [field: SerializeField]
    public int Armor { get; set; } = 0;

    public UnityEvent deathSequence;
    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    //subtracts damage when receiving a OnHit message
    public void OnHit(OnHitPayload payload)
    {
        CurrentHealth -= Math.Max(payload.damage - Armor, 0);
        if (CurrentHealth <= 0)
        {
            deathSequence.Invoke();
        }
    }

    public void RestoreHealth(int value){
        
        CurrentHealth += value;
    }
}