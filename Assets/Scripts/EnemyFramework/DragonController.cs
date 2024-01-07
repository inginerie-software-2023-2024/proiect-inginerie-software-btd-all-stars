using UnityEngine;
using UnityEngine.XR;

public class DragonController : GenericEnemyController
{
    [SerializeField]
    private ProjectileBehaviour _projectilePrefab;

    [SerializeField]
    private Transform _launchOffSet;

    [SerializeField]
    private float _fireCooldownDuration;

    [SerializeField]
    private float _fireballAngle;

    [SerializeField]
    private float _meleeRadius;

    [SerializeField]
    private GameObject _rightClaw;

    [SerializeField]
    private GameObject _leftClaw;

    private bool _fireCooldown = true;
    private bool _doMelee = false;
    private float _leftHandDistance;
    private float _handDistanceThird;
    private bool _hasRightClaw = true;
    private bool _hasLeftClaw = true;
    private Vector3 offset = new(0, -0.5f, 0);

    protected Transform targetPlayer;

    protected new void Start()
    {
        base.Start();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Helpers.SetTimer(_fireCooldownDuration, ResetCooldown));
        _handDistanceThird = Vector3.Distance(_leftClaw.transform.position, _rightClaw.transform.position) / 3f;
    }

    protected override void AttackSequence()
    {
        currentState = EnemyState.attacking;

        //depending on the flag, we decide upon a Melee or Ranged attack
        if (_doMelee)
        {
            _doMelee = false;
            MeleeAttack();
        }
        else
        {
            RangedAttack();
        }
    }

    protected void RangedAttack()
    {
        _fireCooldown = true;
        enemyAnimator.SetBool("fire", true);
        StartCoroutine(Helpers.SetTimer(_fireCooldownDuration, ResetCooldown));
    }

    protected void MeleeAttack()
    {
        if (_hasLeftClaw && _hasRightClaw)
        {
            if (_leftHandDistance > _handDistanceThird)
            {
                if (_leftHandDistance < 2 * _handDistanceThird)
                {
                    enemyAnimator.SetInteger("swipeVariant", 2);
                }
                else
                {
                    enemyAnimator.SetInteger("swipeVariant", 0);
                }
            }
            else
            {
                enemyAnimator.SetInteger("swipeVariant", 1);
            }
        }
        else if (_hasLeftClaw)
        {
            enemyAnimator.SetInteger("swipeVariant", 0);
        }
        else
        {
            enemyAnimator.SetInteger("swipeVariant", 1);
        }
        enemyAnimator.SetBool("melee", true);
    }

    //Enemy is corrently not attacking and either the ranged or melee conditions are satisfied
    protected override bool ConditionIsSatisfied()
    {
        return currentState != EnemyState.attacking &&
            (!_fireCooldown || MeleeCondition());
    }

    //targetPlayer should be close to the boss
    //also sets the melee flag
    private bool MeleeCondition()
    {
        _doMelee = (_hasLeftClaw || _hasRightClaw) &&
                   Vector2.Distance(new Vector2(0, _launchOffSet.transform.position.y),
                                    new Vector2(0, targetPlayer.position.y))
                   <= _meleeRadius;

        if (_doMelee)
        {
            _leftHandDistance = _hasLeftClaw ? Vector2.Distance(new Vector2(_leftClaw.transform.position.x, 0),
                                                                new Vector2(targetPlayer.position.x, 0))
                                             : -1;
        }

        return _doMelee;
    }

    protected override void IdleBehaviour()
    {
        if (currentState == EnemyState.idle)
        {
            enemyAnimator.SetInteger("idleVariant", Mathf.FloorToInt(Random.Range(0, 6)));
        }
    }

    //instantiates and shoots a fireball towards the player
    //offset is to make up for the fact that the Hitbox is not centered on the player transform.
    protected void Fire()
    {
        int attackType = Mathf.FloorToInt(Random.Range(0, 2));
        switch (attackType)
        {
            case 0:
                attackingDirection = (targetPlayer.position + offset
                              - transform.position).normalized;

                ProjectileBehaviour newFireball = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
                newFireball.transform.right = attackingDirection;
                break;

            case 1:
                
                ProjectileBehaviour leftFireBall = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
                leftFireBall.transform.right = Quaternion.AngleAxis(-_fireballAngle, Vector3.back) * Vector3.down;

                ProjectileBehaviour centerFireBall = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
                centerFireBall.transform.right = Vector3.down;

                ProjectileBehaviour rightFireBall = Instantiate(_projectilePrefab, _launchOffSet.position, Quaternion.identity);
                rightFireBall.transform.right = Quaternion.AngleAxis(_fireballAngle, Vector3.back) * Vector3.down;

                break;
        }

        currentState = EnemyState.idle;
        enemyAnimator.SetBool("fire", false);
    }

    protected void ResetCooldown()
    {
        _fireCooldown = false;
    }

    protected void ResetAttacking()
    {
        currentState = EnemyState.idle;
        enemyAnimator.SetBool("melee", false);
        enemyAnimator.SetBool("fire", false);
    }

    //Overriding the GenericEnemy OnHit handler which staggers on hit
    public override void OnHit(OnHitPayload payload)
    {
        return;
    }

    public void RightClawDeathStart()
    {
        _hasRightClaw = false;
        enemyAnimator.SetBool("rightClawDeath", true);
    }

    public void RightClawDeathFinish()
    {
        _rightClaw.SetActive(false);
        enemyAnimator.SetBool("rightClawDeath", false);
    }

    public void LeftClawDeathStart()
    {
        _hasLeftClaw = false;
        enemyAnimator.SetBool("leftClawDeath", true);
    }

    public void LeftClawDeathFinish()
    {
        _leftClaw.SetActive(false);
        enemyAnimator.SetBool("leftClawDeath", false);
    }
}