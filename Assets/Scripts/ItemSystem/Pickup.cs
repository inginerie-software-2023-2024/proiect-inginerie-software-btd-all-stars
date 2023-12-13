using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private int _price;
    [SerializeField]
    private bool _reusable;
    [SerializeField]
    protected Inventory inventory;

    //Method to implement for any class extending pickup
    public abstract void OnPickup(GameObject picker);

    //The pickup mechanic itself
    //Non-shop items should have price = 0 and no price display
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (inventory.coins >= _price && other.gameObject.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
            inventory.coins -= _price;
            if (!_reusable) {
                Destroy(gameObject);
            }
            
        }
    }
}
