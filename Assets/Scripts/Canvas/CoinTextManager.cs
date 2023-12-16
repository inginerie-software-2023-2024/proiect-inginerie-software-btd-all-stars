using UnityEngine;
using TMPro;

public class CoinTextManager : MonoBehaviour
{
    private TextMeshProUGUI coinCounter;
    public Inventory playerInventory;
    public void Start()
    {
        coinCounter = GameObject.Find("CoinCounter").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        playerInventory.CheckCoins();
        coinCounter.text = playerInventory.coins.ToString("000");
    }
}
