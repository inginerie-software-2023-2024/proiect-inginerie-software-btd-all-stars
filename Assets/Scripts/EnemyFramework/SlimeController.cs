
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
    private SlimeController _slimePrefab;

    [SerializeField]
    private LayerMask _tilemapCollision;

    private Vector3 _scale;
    private float _size = 1;

    protected new void Start()
    {
        base.Start();
        target = GameObject.FindWithTag("Player").transform;
        _scale = transform.localScale;

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
        transform.localScale *= _size * 3 / 2;
        _scale = transform.localScale;

        var health = gameObject.GetComponent<HealthManager>();
        health.maxHealth = Mathf.RoundToInt(health.maxHealth * _size * 3 / 2);
        var damage = gameObject.GetComponent<Hurtbox>();
        damage.attackDamage = Mathf.RoundToInt(damage.attackDamage * _size * 3 / 2);
    }

    public override void DeathSequence()
    {
        _scale /= 2;

        if (_splitsLeft == 0)
        {
            base.DeathSequence();
            return;
        }

        for(int i = 0; i < _slimesToSpawn; ++i)
        {
            var newSlime = Instantiate(this, transform.parent);
            newSlime._size = _size / 2;
            newSlime._splitsLeft = _splitsLeft- 1;
            newSlime.Scale();

            Vector3 offset = new(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            newSlime.transform.position =  transform.position + offset;

            if(PositionIsInvalid(newSlime)) {
                newSlime.gameObject.SetActive(false);
                Destroy(newSlime);
                --i;
                continue;
            }

            SendMessageUpwards("RegisterEnemy");
        }

        base.DeathSequence();
    }

    bool PositionIsInvalid(SlimeController slime)
    {
        var slimeCollider = slime.GetComponent<BoxCollider2D>();
        return Physics.CheckBox(slimeCollider.bounds.center, 1.5f * slimeCollider.bounds.extents, Quaternion.identity, _tilemapCollision);
    }

}
