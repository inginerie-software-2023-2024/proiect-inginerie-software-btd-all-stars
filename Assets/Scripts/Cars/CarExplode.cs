using UnityEngine;

public class CarExplode : MonoBehaviour, IOnHitSubscriber
{
    [SerializeField] float _damageThreshold;
    [SerializeField] Animator _carAnimator;
    public void OnHit(OnHitPayload payload)
    {
       if(payload.damage >= _damageThreshold)
       {
            GetComponent<BoxCollider2D>().enabled = false;
            _carAnimator.SetBool("exploding", true);
       }
    }
}
