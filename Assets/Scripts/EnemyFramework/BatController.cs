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
            currentState = EnemyState.attacking;
            StartCoroutine(AttackRoutine());
        }

        protected override bool ConditionIsSatisfied()
        {

            return (currentState == EnemyState.moving || currentState == EnemyState.idle) 
                   && (target.position - transform.position).magnitude > _alertRadius ;

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

        protected IEnumerator AttackRoutine()
        {
            movementDirection = target.transform.position - transform.position;
            yield return new WaitForSeconds(0.001f);
            movementDirection = movementDirection.normalized;
            currentState = EnemyState.attacking;
            enemyRigidbody.MovePosition(transform.position + speed * Time.deltaTime * movementDirection);
        }
    }
}