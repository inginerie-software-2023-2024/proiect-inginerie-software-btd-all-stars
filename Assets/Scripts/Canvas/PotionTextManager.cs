using TMPro;
using UnityEngine;

public class PotionTextManager : MonoBehaviour
{
    private TextMeshProUGUI potionCounter;
    public Inventory playerInventory;
    public void Start()
    {
        potionCounter = GameObject.Find("PotionCounter").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        playerInventory.CheckPotions();
        potionCounter.text = playerInventory.potions.ToString("0");
    }
}
