using UnityEngine;

public enum HitboxType
{
    Player,
    Enemy,
    Map
}
public class Hitbox : MonoBehaviour
{
    public HitboxType type;

    public bool Invulnerable { get; private set; }
    public void MakeInvulnerable()
    {
        Invulnerable = true;
    }

    public void MakeVulnerable()
    {
        Invulnerable = false;
    }

}
