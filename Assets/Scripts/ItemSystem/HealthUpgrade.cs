using UnityEngine;

public class HealthUpgrade : Pickup, IUpgrade
{
    [SerializeField]
    private int _healthUp;
    public GameObject target;

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    //Increases max health and fully heals player
    public void AddEffect()
    {
        HealthManager manager = target.GetComponent<HealthManager>();
        manager.maxHealth += _healthUp;
        manager.RestoreHealth(manager.maxHealth - manager.CurrentHealth);
    }

    //Increases max health and fully heals player
    public override void OnPickup(GameObject picker)
    {
        target = picker;
        AddEffect();
        inventory.upgrades.Add(this);
    }
}
