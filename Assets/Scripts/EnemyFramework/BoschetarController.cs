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

    private Vector3 _shootDirection;
    private bool _cooldown;


    protected new void Start()
    {
        base.Start();
        _shootDirection = Vector2.Perpendicular(_movementDirection).normalized;
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
            
            Vector3 moveDirection = Vector3.Project(target.position - transform.position, _movementDirection);

            if (moveDirection.magnitude <= 0.1f)
                return;

            currentState = EnemyState.moving;

            if (TargetIsOnLine())
            {
                moveDirection *= -1;
            }
            

            enemyRigidbody.MovePosition(transform.position + speed * Time.deltaTime * moveDirection.normalized);
        }
    }

    public void Shoot()
    {
        
        ProjectileBehaviour newBullet = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
        newBullet.transform.right = _shootDirection;
        _cooldown = true;
        currentState = EnemyState.idle;
        StartCoroutine(Helpers.SetTimer(_cooldownDuration, () => _cooldown = false));
    }

    private bool TargetIsOnLine()
    {
        return Vector3.Project(target.position - transform.position, _shootDirection).magnitude <= _fearRadius; 
    }
}
