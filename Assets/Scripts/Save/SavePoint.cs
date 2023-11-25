using UnityEngine;

public class SavePoint : Pickup
{
    [SerializeField]
    private SaveManager _manager;
    public override void OnPickup(GameObject picker)
    {
        if (picker.CompareTag("Player"))
        {
            _manager.Save();
        }
    }
}
