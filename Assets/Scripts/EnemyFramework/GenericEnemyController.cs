using System.Collections;
using System.Linq;
using UnityEngine;

public enum EnemyState
{
    idle,
    moving,
    attacking,
    staggered,
    dying
}


public abstract class GenericEnemyController : MonoBehaviour, IOnHitSubscriber
{
    public float speed;
    public string enemyName;
    public EnemyState currentState;

    protected Rigidbody2D enemyRigidbody;
    protected Animator enemyAnimator;
    protected Vector3 movementDirection;
    protected Vector3 attackingDirection;
    
    protected Transform target;

    protected Coroutine stagger;

    [SerializeField]
    protected float _staggerTime = 0.32f;
    
    [SerializeField]
    private AudioSource _audioSource;
    //Initialization
    protected void Start()
    {
        currentState = EnemyState.idle;
        _audioSource.time = 0.1f;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    //Main Enemy Loop: Attack if condition is satisfied, else do IdleBehaviour

    protected void FixedUpdate()
    {
            if (ConditionIsSatisfied())
            {
                AttackSequence();
            }
            else
            {
                IdleBehaviour();
            }

            UpdateAnimation();
    }

    //This method updates the enemy animation
    //A default implementation is provided but it can be overridden
    protected virtual void UpdateAnimation()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                enemyAnimator.SetBool("moving", false);
                enemyAnimator.SetBool("attacking", false);
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

    //Each enemy inheriting from Generic Enemy has to implement these methods

    //This method returns true when the enemy can attack,
    //possible checks include proximity, timer, etc
    protected abstract bool ConditionIsSatisfied();

    //This method starts the attack sequence,
    //which could be a melee hit, a ranged attack and more, etc
    protected abstract void AttackSequence();
    
    //This method determines the enemy behaviour when not attacking,
    //such as moving randomly, chasing the player, staying still, etc
    protected abstract void IdleBehaviour();

    public virtual void DeathSequence()
    {
        if (currentState != EnemyState.dying)
        {
            currentState = EnemyState.dying;
            movementDirection = Vector3.zero;
            enemyRigidbody.totalForce = Vector3.zero;
            enemyRigidbody.velocity = Vector3.zero;
            var kill = GameObject.FindWithTag("Kill");
            var killSound = kill.GetComponent<AudioSource>();
            killSound.time = 0.2f;
            killSound.Play();
        }
    }

    //OnHit handler to stagger enemy for a short while
    //While staggered, it does not do anything
    public virtual void OnHit(OnHitPayload payload)
    {
        if (currentState == EnemyState.dying)
            return;

        movementDirection = Vector3.zero;
        if(currentState == EnemyState.staggered)
        {
            StopCoroutine(stagger);
        }
        
        stagger = StartCoroutine(Stagger(_staggerTime));
        _audioSource.Play();
    }

    protected IEnumerator Stagger(float seconds)
    {
        currentState = EnemyState.staggered;
        yield return new WaitForSeconds(seconds);
        currentState = EnemyState.idle;
    }

    //WakeUp method sends a message to the EnemyHandler to register this enemy
    protected void WakeUp()
    {
        this.enabled = true;
        SendMessageUpwards("RegisterEnemy");
    }

    protected void Despawn()
    {
        this.enabled = false;
        Destroy(gameObject);
    }

    protected static bool MovementIsHorizontal(Vector3 direction)
    {
        return Mathf.Abs(Vector3.Dot(direction, Vector3.right))
                >=
               Mathf.Abs(Vector3.Dot(direction, Vector3.up));
    }

}
