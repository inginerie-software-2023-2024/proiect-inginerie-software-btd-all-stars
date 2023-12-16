using System.Collections;
using UnityEngine;


public class RatController : GenericEnemyController
{
    protected BoxCollider2D verticalCollider;
    protected BoxCollider2D horizontalCollider;

    private bool changeTime = true;

    protected new void Start()
    {
        base.Start();
        verticalCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        horizontalCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
    }

    //Rats only move and deal contact damage, no active attacking
    protected override bool ConditionIsSatisfied() 
    {
        return false;
    }

    protected override void AttackSequence()
    {
        return;
    }

    //Enemy should pick a direction and move towards it for 3 seconds, then stopping for another 3
    protected override void IdleBehaviour()
    {
        if (changeTime)
        {
            //If we're currently idle, pick a random direction and begin moving
            if (currentState == EnemyState.idle)
            {
                movementDirection = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)).normalized;
                currentState = EnemyState.moving;
            }
            //If we're currently moving, stop
            else if (currentState == EnemyState.moving)
            {
                movementDirection = Vector3.zero;
                currentState = EnemyState.idle;
            }

            //Perform the next movementDirection change after 3 seconds
            changeTime = false;
            StartCoroutine(SetTimer(Random.Range(1.5f,3.5f)));
        }

        //Sprite is not symmetrical so we require different HitBoxes
        if (currentState == EnemyState.moving)
        {
            if (MovementIsHorizontal(movementDirection))
            {
                verticalCollider.enabled = false;
                horizontalCollider.enabled = true;
            }
            else
            {
                verticalCollider.enabled = true;
                horizontalCollider.enabled = false;
            }
        }

        if(currentState != EnemyState.staggered)
            enemyRigidbody.MovePosition(
                transform.position + speed * Time.deltaTime * movementDirection);
    }

    //this coroutine sets changeTime to true after waiting the given seconds
    //by calling this asynchronously I can delay setting the flag.
    protected IEnumerator SetTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        changeTime = true;
    }
}
