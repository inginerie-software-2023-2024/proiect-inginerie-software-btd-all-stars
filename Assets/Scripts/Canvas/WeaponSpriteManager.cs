using UnityEngine;
using UnityEngine.UI;

public class WeaponSpriteManager : MonoBehaviour
{
    public Image rightImage;
    public Image leftImage;
    public Inventory inventory;

    public void Start()
    {
        if (inventory.rightWeapon != null) 
        {
            rightImage.sprite = inventory.rightWeapon.weaponSprite;
            rightImage.color = Color.white;
        }
        else
        {
            rightImage.sprite = null;
            rightImage.GetComponent<Graphic>().color = new Color32(154, 120, 65, 255);
        }
        if (inventory.leftWeapon != null)
        {
            leftImage.sprite = inventory.leftWeapon.weaponSprite;
            leftImage.color = Color.white;
        }
        else
        {
            leftImage.sprite = null;
            leftImage.GetComponent<Graphic>().color = new Color32(154, 120, 65, 255);
        }
    }
    //Change right weapon image on HUD and the item from inventory
    public void ChangeRightWeapon(Weapon newweapon)
    {
        if (inventory.rightWeapon && inventory.rightWeapon.size == WeaponSize.doubleSlot)
        {
            inventory.leftWeapon = null;
            leftImage.sprite = null;
            leftImage.GetComponent<Graphic>().color = new Color32(154, 120, 65, 255);
        }

        rightImage.sprite = newweapon.weaponSprite;
        rightImage.color = Color.white;
        inventory.rightWeapon = newweapon;

        if (newweapon.size == WeaponSize.doubleSlot)
        {
            leftImage.sprite = newweapon.weaponSprite;
            inventory.leftWeapon = newweapon;
            leftImage.color = Color.white;
        }
    }
    //Change left weapon image on HUD and the item from inventory
    public void ChangeLeftWeapon(Weapon newweapon) 
    { 
        if(inventory.leftWeapon && inventory.leftWeapon.size == WeaponSize.doubleSlot)
        {
            inventory.rightWeapon = null;
            rightImage.sprite = null;
            rightImage.GetComponent<Graphic>().color = new Color32(154, 120, 65, 255);
        }

        leftImage.sprite = newweapon.weaponSprite;
        inventory.leftWeapon = newweapon;
        leftImage.color = Color.white;

        if(newweapon.size == WeaponSize.doubleSlot)
        {
            rightImage.sprite = newweapon.weaponSprite;
            inventory.rightWeapon = newweapon;
            rightImage.color = Color.white;
        }
    }
}
