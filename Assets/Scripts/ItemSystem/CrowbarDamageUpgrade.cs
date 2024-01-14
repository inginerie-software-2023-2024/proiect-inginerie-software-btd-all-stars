using UnityEngine;

public class CrowbarDamageUpgrade : Pickup, IUpgrade
{
    [SerializeField]
    private int _damageUp;

    [SerializeField]
    private Weapon _crowbar;

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    //Increases damage
    public void AddEffect()
    {
        _crowbar.damage += _damageUp;
    }

    public override void OnPickup(GameObject picker)
    {
        AddEffect();
        inventory.upgrades.Add(this);
    }
}
