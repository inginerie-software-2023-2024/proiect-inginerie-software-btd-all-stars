using System.Collections;
using UnityEngine;

public class KnockbackManager : MonoBehaviour, IOnHitSubscriber
{
    public float knockbackStrength;

    private Rigidbody2D entityRigidbody;

    [SerializeField]
    private float _knockbackDuration = 0.32f;

    private void Start()
    {
        entityRigidbody = GetComponent<Rigidbody2D>();
    }

    //OnHit, applies a short impulse force, knocking the object back
    public void OnHit(OnHitPayload payload)
    {
        Vector3 hitDirection = transform.position - payload.position;
        Vector2 knockbackDireciton = hitDirection.normalized;
        GetComponent<Rigidbody2D>().AddForce(knockbackDireciton * knockbackStrength, ForceMode2D.Impulse);

        StartCoroutine(StopKnockback(_knockbackDuration));
    }

    private IEnumerator StopKnockback(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        entityRigidbody.velocity = Vector3.zero;
    }
}