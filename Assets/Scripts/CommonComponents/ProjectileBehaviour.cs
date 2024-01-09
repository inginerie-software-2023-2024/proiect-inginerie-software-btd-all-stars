using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 10f;

    [SerializeField]
    private LayerMask _excludedLayers;

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
    }
    //destoy object on contact with another object
    public void OnTriggerEnter2D(Collider2D other)
    {
        //elegant way of checking if layermask does not contain the layer
        if (_excludedLayers != (_excludedLayers | (1 << other.gameObject.layer)))
        {
            Destroy(gameObject);
        }   
    }
}

