using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    [SerializeField]
    private LayerMask _includedLayers;
    [SerializeField]
    private float offset = 0f;
    private float lowerBound;

    private void Start()
    {
        lowerBound = GetComponent<Collider2D>().bounds.min.y + 0.6f;
    }

    //Controls layer priority so that the object will be behind the player
    //if the player is below it, or above otherwise
    protected void OnTriggerStay2D(Collider2D target)
    {
        if(_includedLayers == (_includedLayers | (1 << target.gameObject.layer)))
        {
            int targetSortingLayer;
            float targetLowerBound = target.bounds.min.y;
            SpriteRenderer localSpriteRenderer;
            if(GetComponent<SpriteRenderer>()) 
            {
                localSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            else
            {
                localSpriteRenderer = GetComponentInParent<SpriteRenderer>();
            }
            if (target.GetComponent<SpriteRenderer>())
            {
                targetSortingLayer = target.GetComponent<SpriteRenderer>().sortingOrder;
            }
            else
            {
                targetSortingLayer = target.GetComponentInParent<SpriteRenderer>().sortingOrder;
            }

            if (targetLowerBound >= lowerBound +  offset)
            {
                localSpriteRenderer.sortingOrder = targetSortingLayer + 1;
            }
            else
            {
                localSpriteRenderer.sortingOrder = targetSortingLayer - 1;
            }
        }
    }
}
