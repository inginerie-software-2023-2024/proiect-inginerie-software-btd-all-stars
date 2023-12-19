using System.Collections;
using UnityEngine;

namespace EnemyFramework
{
    public class BatController : GenericEnemyController
    {
        [SerializeField]
        private float _alertRadius = 0.2f;

        [SerializeField]
        private Vector2 _movementDirection;
        protected new void Start()
        {
            base.Start();
            attackingDirection = Vector2.Perpendicular(_movementDirection).normalized;
            target = GameObject.FindWithTag("Player").transform;
        }
        protected override void AttackSequence()
        {
            return;
        }

        protected override bool ConditionIsSatisfied()
        {
            return false;
        }
        protected override void IdleBehaviour()
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.moving)
            {
                movementDirection = (target.transform.position - transform.position);

                if (movementDirection.magnitude > _alertRadius)
                    return;

                movementDirection = movementDirection.normalized;
                currentState = EnemyState.moving;
                enemyRigidbody.MovePosition(transform.position + speed * Time.deltaTime * movementDirection);

            }
        }
    }
}