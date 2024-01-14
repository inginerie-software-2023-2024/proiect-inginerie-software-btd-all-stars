using UnityEngine;

public class CrowbarDamage : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;

    private Hurtbox _weaponHurtbox;
    void Awake()
    {
        _weaponHurtbox = GetComponent<Hurtbox>();
        SetDamage();
    }

    //This is a garbage implementation but whatever
    private void Update()
    {
        SetDamage();
    }

    public void SetDamage()
    {
        GetComponent<Hurtbox>().attackDamage = _weapon.damage;
    }
}
