using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour, IOnHitSubscriber
{
    public int maxHealth = 10;
    public int CurrentHealth { get; set; }

    public UnityEvent deathSequence;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    //subtracts damage when receiving a OnHit message
    public void OnHit(OnHitPayload payload)
    {
        CurrentHealth -= payload.damage;
        if (CurrentHealth <= 0)
        {
            deathSequence.Invoke();
        }
    }

    public void RestoreHealth(int value){
        
        CurrentHealth += value;
    }
}