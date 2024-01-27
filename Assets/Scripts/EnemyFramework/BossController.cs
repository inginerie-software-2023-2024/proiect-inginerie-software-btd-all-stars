using UnityEngine;

public class BossController : GenericEnemyController
{
    protected Transform targetPlayer;
    public ProjectileBehaviour projectilePrefab;
    public Transform launchOffSet;
    public int bulletCount;
    public float fireInterval;
    public float fireCooldownDuration;
    public float meleeRadius;

    bool fireCooldown = true;
    bool doMelee = false;

    private Vector3 offset = new(0, -0.5f, 0);

    protected new void Start()
    {
        base.Start();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Helpers.SetTimer(fireCooldownDuration, ResetCooldown));
    }
    protected override void AttackSequence()
    {
        attackingDirection = (targetPlayer.position + offset
                             - transform.position).normalized;
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

    //Fires bulletCount bullets, with fireInterval pause inbetween
    protected void RangedAttack()
    {
        fireCooldown = true;
        enemyAnimator.SetBool("fire", true);
        StartCoroutine(Helpers.RepeatWithDelay(bulletCount, fireInterval, Fire));
        StartCoroutine(Helpers.SetTimer(bulletCount * fireInterval, ResetAttacking));
        StartCoroutine(Helpers.SetTimer(bulletCount * fireInterval + fireCooldownDuration, ResetCooldown));
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
        if ((targetPlayer.position - 
            transform.position).magnitude < meleeRadius)
            doMelee = true;
        return doMelee;
    }

    //When not attacking, the boss follows the player
    protected override void IdleBehaviour()
    {
        if (currentState == EnemyState.idle || currentState == EnemyState.moving)
        {
            movementDirection = (targetPlayer.position
                                 - transform.position).normalized;
            currentState = EnemyState.moving;
            enemyRigidbody.MovePosition(
                    transform.position + speed * Time.deltaTime * movementDirection);
        }
    }

    //instantiates and shoots a bullet towards the player
    //offset is to make up for the fact that the Hitbox is not centered on the player transform.
    protected void Fire()
    {
        attackingDirection = (targetPlayer.position + offset
                                      - transform.position).normalized;
        ProjectileBehaviour newBullet = Instantiate(projectilePrefab, launchOffSet.position, Quaternion.identity);
        newBullet.transform.right = attackingDirection;
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
        audioSource.Play();
        return;
    }
}
