using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private int enemies = 0;
    public bool RoomCleared { get; set; }

    public Pickup RoomReward;

    [SerializeField]
    private SaveManager _saveManager;


    public void Awake()
    {
        if (_saveManager.state.clearedRooms.Contains(name))
        {
            RoomCleared = true;
            BroadcastMessage("Despawn", null, SendMessageOptions.DontRequireReceiver);
            BroadcastMessage("DisableWall", null, SendMessageOptions.DontRequireReceiver);
            if (RoomReward != null)
            {
                RoomReward.gameObject.SetActive(true);
            }
        }
    }
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
            if (RoomReward != null)
            {
                RoomReward.gameObject.SetActive(true);
            }
        }
    }
}
