using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hurtbox : MonoBehaviour
{

    public int attackDamage;
    public List<HitboxType> targets;

    //4 different methods to handle different types of collission
    void OnCollisionStay2D(Collision2D other)
    {
        GameObject target = other.gameObject;
        HandleCollision(target);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        HandleCollision(target);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject target = other.gameObject;
        HandleCollision(target);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        HandleCollision(target);
    }

    //test if the other object has a HitBox component
    //if yes, check it's type against this HurtBox's targets
    //if there's a match, send OnHit messages to all subscribers on the target
    private void HandleCollision(GameObject target)
    {
        target.TryGetComponent(out Hitbox hitbox);
        if (hitbox == null)
            return;

        if (targets.Contains(hitbox.type) && !hitbox.Invulnerable)
        {
            OnHitPayload payload = new(attackDamage, transform.position);
            ExecuteEvents.Execute<IOnHitSubscriber>(target, null, (handler, _) => handler.OnHit(payload));
        }
    }

}
