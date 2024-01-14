using UnityEngine;

public enum WeaponType
{
    glock,
    crowbar
}
public enum WeaponSize
{
    singleSlot,
    doubleSlot
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public WeaponType type;
    public WeaponSize size;
    public int damage;
    public float attackSpeed = 1;
}
