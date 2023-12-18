using UnityEngine;

public class SavePoint : Pickup
{

    public override void OnPickup(GameObject picker)
    {
        if (picker.CompareTag("Player"))
        {
            _saveManager.Save();
        }
    }
}
