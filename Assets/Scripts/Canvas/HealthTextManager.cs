using UnityEngine;
using UnityEngine.UI;

public class HealthTextManager : MonoBehaviour
{
    public Image[] pills;
    public Sprite fullPill;
    public Sprite halfPill;
    public Sprite emptyPill;
    public HealthManager playerHealth;

    void Start()
    {
        InitHealth();
    }

    //Set active a specific number of pile objects
    public void InitHealth()
    {
        for(int i = 0; i < playerHealth.maxHealth / 2; i++)
        {
            pills[i].gameObject.SetActive(true);
            pills[i].sprite = fullPill;
        }
    }

    void Update()
    {
        UpdateHealth();
    }

    //Update displayed sprite for pills by a specific number which should represent player's current health
    public void UpdateHealth()
    {
        int auxHealth = playerHealth.CurrentHealth;
        for(int i = 0; i < playerHealth.maxHealth; i++)
        {
            if (!pills[i / 2].gameObject.activeSelf)
            {
                pills[i/2].gameObject.SetActive(true);
            }
            if( i < auxHealth)
            {
                pills[i/2].sprite = fullPill;
            }
            else if(i > auxHealth)
            {
                pills[i/2].sprite = emptyPill;
            }
            else
            {
                pills[i/2].sprite = halfPill;
            }
        }
    }
}
