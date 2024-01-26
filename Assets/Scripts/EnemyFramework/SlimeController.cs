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
        StartCoroutine(Helpers.SetTimer(0.5f, () =>
        {
            hitbox.MakeVulnerable();
            currentState = EnemyState.idle;
        }));

    }

    private void LateUpdate()
    {
        transform.localScale = _scale;
        if (currentState == EnemyState.dying)
        {
            transform.localScale = _scale / 2.5f;
        }
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
        if (currentState != EnemyState.dying)
        {
            base.DeathSequence();

            if (_splitsLeft == 0)
            {
                return;
            }

            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.white;

            for (int i = 0; i < _slimesToSpawn; ++i)
            {
                var newSlime = Instantiate(this, transform.parent);

                newSlime._splitsLeft = _splitsLeft - 1;
                newSlime.Scale();

                newSlime.GetComponent<Animator>().GetBehaviour<Death>().reward = false;
              
                Disperse(newSlime);

                SendMessageUpwards("RegisterEnemy", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void Disperse(SlimeController newSlime)
    {
        var hitDirection = (transform.position - target.position).normalized;
        var knockbackDirection = new Vector2(hitDirection.x, hitDirection.z).normalized;
        var perpendicular = Vector2.Perpendicular(knockbackDirection).normalized;

        knockbackDirection *= Random.Range(3, 6);
        perpendicular *= Random.Range(-6, 6);

        var force = knockbackDirection + perpendicular;

        newSlime.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
