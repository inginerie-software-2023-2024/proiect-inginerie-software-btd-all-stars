using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public int enemies = 0;
    public bool RoomCleared { get; set; }

    public Pickup RoomReward;

    [SerializeField]
    private SaveManager _saveManager;

    [SerializeField] 
    private GameObject _minimapTiles;
    
    [SerializeField] 
    private AudioSource _endSound;
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
            if (_minimapTiles != null) {
                _minimapTiles.SetActive(true);
            }
        }
    }
    //wakes up all child enemies
    public void WakeUpEnemies()
    {
        BroadcastMessage("WakeUp",null,SendMessageOptions.DontRequireReceiver);
        if(!RoomCleared)
        {
            //if room has not been cleared, enable the walls and disable the transitions
            BroadcastMessage("EnableWall", null, SendMessageOptions.DontRequireReceiver);
            BroadcastMessage("DisableTransition", null, SendMessageOptions.DontRequireReceiver);
        }

        if (_minimapTiles != null) {
                _minimapTiles.SetActive(true);
        }
    }

    //If there is at least one enemy, room has not been cleared
    public void RegisterEnemy()
    {
        ++enemies;
    }
    //If there are no more enemies, room is cleared, walls are disabled and transitions enabled
    public void UnregisterEnemy()
    {
        --enemies;
        if(enemies == 0)
        {
            RoomCleared = true;
            BroadcastMessage("DisableWall",null,SendMessageOptions.DontRequireReceiver);
            BroadcastMessage("EnableTransition", null, SendMessageOptions.DontRequireReceiver);
            _endSound.Play();
            if (RoomReward != null)
            {
                RoomReward.gameObject.SetActive(true);
            }
        }
    }

    public void ActicateMinimapRoom(){

    }
}
