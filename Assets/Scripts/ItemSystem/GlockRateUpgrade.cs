using UnityEngine;

public class GlockRateUpgrade : Pickup, IUpgrade
{
    [SerializeField]
    private float _rateUp;
    [SerializeField]
    private Weapon _glock;

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    //Increases damage
    public void AddEffect()
    {
        _glock.attackSpeed += _rateUp;
    }

    public override void OnPickup(GameObject picker)
    {
        AddEffect();
        inventory.upgrades.Add(this);
    }
}
