using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    [Range(0,999)]
    public int coins;
    [Range(0,9)]
    public int potions;
    
    public List<IUpgrade> upgrades = new();
#nullable enable
    public Weapon? leftWeapon, rightWeapon;
    //Limit the number of coins
    public void CheckCoins()
    {
        coins = Mathf.Clamp(coins, 0, 999);
    }
    //Limit the number of potions
    public void CheckPotions()
    {
        potions = Mathf.Clamp(potions, 0, 9);
    }
}
