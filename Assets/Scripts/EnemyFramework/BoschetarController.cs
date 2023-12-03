using UnityEngine;

public class BoschetarController : GenericEnemyController
{
    [SerializeField]
    private ProjectileBehaviour _projectilePrefab;

    [SerializeField]
    private Vector2 _movementDirection;

    [SerializeField] 
    private float _cooldownDuration;

    [SerializeField]
    private float _fearRadius;

    [SerializeField]
    private Transform _launchOffSet;

    private bool _cooldown;


    protected new void Start()
    {
        base.Start();
        attackingDirection = Vector2.Perpendicular(_movementDirection).normalized;
        Debug.Log(attackingDirection);
        target = GameObject.FindWithTag("Player").transform;
    }

    protected override void AttackSequence()
    {
        currentState = EnemyState.attacking;
    }

    protected override bool ConditionIsSatisfied()
    {
        Vector3 projectedPosition = Vector3.Project(transform.position, _movementDirection);
        Vector3 projectedTarget = Vector3.Project(target.position, _movementDirection);

        return (currentState == EnemyState.moving || currentState == EnemyState.idle)
            && !_cooldown 
            && (projectedTarget - projectedPosition).magnitude <= 0.1f;
    }

    protected override void IdleBehaviour()
    {
        if (currentState == EnemyState.idle || currentState == EnemyState.moving)
        {
            
            movementDirection = Vector3.Project(target.position - transform.position, _movementDirection);

            if (movementDirection.magnitude <= 0.1f)
            {
                currentState = EnemyState.idle;
                movementDirection = attackingDirection;
                return;
            }

            currentState = EnemyState.moving;

            if (TargetIsOnLine())
            {
                movementDirection *= -1;
            }

            enemyRigidbody.MovePosition(transform.position + speed * Time.deltaTime * movementDirection.normalized);
        }
    }

    public void Shoot()
    {
        ProjectileBehaviour newBullet = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
        newBullet.transform.right = attackingDirection;
        _cooldown = true;
        currentState = EnemyState.idle;
        StartCoroutine(Helpers.SetTimer(_cooldownDuration, () => _cooldown = false));
    }

    private bool TargetIsOnLine()
    {
        return Vector3.Project(target.position - transform.position, attackingDirection).magnitude <= _fearRadius; 
    }

    protected override void UpdateAnimation()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                enemyAnimator.SetBool("moving", false);
                enemyAnimator.SetBool("attacking", false);
                enemyAnimator.SetFloat("moveX", movementDirection.x);
                enemyAnimator.SetFloat("moveY", movementDirection.y);
                break;

            case EnemyState.moving:
                enemyAnimator.SetFloat("moveX", movementDirection.x);
                enemyAnimator.SetFloat("moveY", movementDirection.y);
                enemyAnimator.SetBool("moving", true);
                enemyAnimator.SetBool("attacking", false);
                break;

            case EnemyState.staggered:
                enemyAnimator.SetBool("moving", false);
                enemyAnimator.SetBool("attacking", false);
                break;
            case EnemyState.dying:
                enemyAnimator.SetBool("dying", true);
                break;
            case EnemyState.attacking:
                enemyAnimator.SetBool("attacking", true);
                enemyAnimator.SetFloat("moveX", attackingDirection.x);
                enemyAnimator.SetFloat("moveY", attackingDirection.y);

                break;
        }
    }

}
