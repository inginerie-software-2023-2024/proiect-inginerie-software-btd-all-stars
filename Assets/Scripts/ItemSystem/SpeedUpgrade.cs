using UnityEngine;

public class SpeedUpgrade : Pickup, IUpgrade
{
    [SerializeField]
    private int _speedUp;
    public GameObject target;

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    //Increases speed
    public void AddEffect()
    {
        PlayerController controller = target.GetComponent<PlayerController>();
        controller.speed += _speedUp;
    }

    public override void OnPickup(GameObject picker)
    {
        target = picker;
        AddEffect();
        inventory.upgrades.Add(this);
    }
}
