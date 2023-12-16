using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    private Transform targetPlayer;

    [SerializeField]
    private float _treashold = 0.5f;
    public void Start()
    {
        targetPlayer = GameObject.FindWithTag("Player").transform;
    }
    //Controls layer priority so that the object will be behind the player
    //if the player is below it, or above otherwise
    protected void FixedUpdate()
    {
        if (targetPlayer.position.y - _treashold > transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }
}
