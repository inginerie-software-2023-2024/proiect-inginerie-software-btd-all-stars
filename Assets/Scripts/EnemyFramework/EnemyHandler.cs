using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private int enemies = 0;
    public bool RoomCleared { get; set; }

    public Pickup rewardPrefab;

    //wakes up all child enemies
    public void WakeUpEnemies()
    {
        BroadcastMessage("WakeUp",null,SendMessageOptions.DontRequireReceiver);
        if(!RoomCleared)
        {
            //if room has not been cleared, enable the walls
            BroadcastMessage("EnableWall", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    //If there is at least one enemy, room has not been cleared
    public void RegisterEnemy()
    {
        ++enemies;
    }
    //If there are no more enemies, room is cleared and walls are disabled
    public void UnregisterEnemy()
    {
        --enemies;
        if(enemies == 0)
        {
            RoomCleared = true;
            BroadcastMessage("DisableWall",null,SendMessageOptions.DontRequireReceiver);
            if (rewardPrefab != null)
            {
                Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
