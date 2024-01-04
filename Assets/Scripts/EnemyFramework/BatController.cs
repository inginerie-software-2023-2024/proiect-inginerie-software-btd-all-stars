using System.Collections;
using UnityEngine;

namespace EnemyFramework
{
    public class BatController : GenericEnemyController
    {
        [SerializeField]
        private float _maxSpeed;
        protected new void Start()
        {
            base.Start();
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

                movementDirection = movementDirection.normalized;
                currentState = EnemyState.moving;
                enemyRigidbody.AddForce(speed * Time.deltaTime * movementDirection, ForceMode2D.Impulse);
                
                if(enemyRigidbody.velocity.magnitude > _maxSpeed)
                {
                    enemyRigidbody.velocity = enemyRigidbody.velocity.normalized * _maxSpeed;
                }

            }
        }
    }
}