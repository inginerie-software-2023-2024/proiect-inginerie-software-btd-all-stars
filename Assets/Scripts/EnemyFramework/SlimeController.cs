using UnityEngine;

public class SlimeController : GenericEnemyController
{
    [SerializeField]
    private float _alertRadius = 20;

    [SerializeField]
    private float _slimesToSpawn = 2;

    [SerializeField]
    private float _splitsLeft = 2;

    [SerializeField]
    private float _splitShrink = 0.75f;

    private Vector3 _scale;

    protected new void Start()
    {
        base.Start();
        target = GameObject.FindWithTag("Player").transform;
        _scale = transform.localScale;
        var hitbox = GetComponent<Hitbox>();
        hitbox.MakeInvulnerable();
        currentState = EnemyState.staggered;
        StartCoroutine(Helpers.SetTimer(0.32f, () =>
        {
            hitbox.MakeVulnerable();
            currentState = EnemyState.idle;
        }));

    }

    private void LateUpdate()
    {
        transform.localScale = _scale;
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

    private void Scale()
    {
        transform.localScale *= _splitShrink;
        _scale = transform.localScale;

        var health = gameObject.GetComponent<HealthManager>();
        health.maxHealth = Mathf.RoundToInt(health.maxHealth * _splitShrink);
        var damage = gameObject.GetComponent<Hurtbox>();
        damage.attackDamage = Mathf.FloorToInt(damage.attackDamage * _splitShrink);
    }

    public override void DeathSequence()
    {
        base.DeathSequence();

        if (_splitsLeft == 0)
        { 
            return;
        }

        var sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = false;
        sprite.color = Color.white;

        for(int i = 0; i < _slimesToSpawn; ++i)
        {
            var newSlime = Instantiate(this, transform.parent);
            
            newSlime._splitsLeft = _splitsLeft- 1;
            newSlime.Scale();

            Disperse(newSlime);

            SendMessageUpwards("RegisterEnemy", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Disperse(SlimeController newSlime)
    {
        Vector3 force = new(Random.Range(-2,2), Random.Range(-2,2));
        newSlime.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
