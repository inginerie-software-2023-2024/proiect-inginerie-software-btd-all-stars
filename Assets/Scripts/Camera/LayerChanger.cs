using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    [SerializeField]
    private LayerMask _includedLayers;

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

            if (target.GetComponent<SpriteRenderer>())
            {
                targetSortingLayer = target.GetComponent<SpriteRenderer>().sortingOrder;
            }
            else
            {
                targetSortingLayer = target.GetComponentInParent<SpriteRenderer>().sortingOrder;
            }

            if (targetLowerBound >= lowerBound)
            {
                GetComponent<SpriteRenderer>().sortingOrder = targetSortingLayer + 1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = targetSortingLayer - 1;
            }
        }
    }
}
