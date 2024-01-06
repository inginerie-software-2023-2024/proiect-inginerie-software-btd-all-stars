using UnityEngine;

public class DragonController : GenericEnemyController
{
    
    [SerializeField]
    private ProjectileBehaviour projectilePrefab;
    [SerializeField] 
    private Transform launchOffSet;
    
    [SerializeField]
    public float fireCooldownDuration;
    
    [SerializeField]
    public float meleeRadius;

    bool fireCooldown = true;
    bool doMelee = false;

    protected Transform targetPlayer;
    private Vector3 offset = new(0, -0.5f, 0);

    protected new void Start()
    {
        base.Start();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Helpers.SetTimer(fireCooldownDuration, ResetCooldown));
    }
    protected override void AttackSequence()
    {
        currentState = EnemyState.attacking;

        //depending on the flag, we decide upon a Melee or Ranged attack
        if (doMelee)
        {
            doMelee = false;
            MeleeAttack();
        }
        else
        {
            RangedAttack();
        }
    }

    protected void RangedAttack()
    {
        fireCooldown = true;
        enemyAnimator.SetBool("fire", true);
        StartCoroutine(Helpers.SetTimer(fireCooldownDuration, ResetCooldown));
    }

    //Smashes ground with weapon (attack is implemented in the Animation Clip)
    protected void MeleeAttack()
    {
        enemyAnimator.SetBool("melee", true);
        StartCoroutine(Helpers.SetTimer(1.683f, ResetAttacking));

    }

    //Enemy is corrently not attacking and either the ranged or melee conditions are satisfied
    protected override bool ConditionIsSatisfied()
    {
        return currentState != EnemyState.attacking &&
            (!fireCooldown || MeleeCondition());
    }

    //targetPlayer should be close to the boss
    //also sets the melee flag
    private bool MeleeCondition()
    {
        return false;
    }

    protected override void IdleBehaviour()
    {
        if (currentState == EnemyState.idle)
        {
            enemyAnimator.SetFloat("idleVariant", Random.Range(0, 1));
        }
    }

    //instantiates and shoots a bullet towards the player
    //offset is to make up for the fact that the Hitbox is not centered on the player transform.
    protected void Fire()
    {
        attackingDirection = (targetPlayer.position + offset
                              - transform.position).normalized;
        
        ProjectileBehaviour newFireball = Instantiate(projectilePrefab, launchOffSet.position, Quaternion.identity);
        newFireball.transform.right = attackingDirection;
        currentState = EnemyState.idle;
        enemyAnimator.SetBool("fire", false);
    }
    protected void ResetCooldown()
    {
        fireCooldown = false;
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
}
