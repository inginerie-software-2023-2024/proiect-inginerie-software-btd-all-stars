using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Scriptable Objects/GameState")]

public class SaveManager : ScriptableObject
{
    [Serializable]
    class GameState
    {
        public Vector2 playerPosition;
        public List<string> upgrades;
        public List<int> clearedRooms;
    }

    private GameState _state;

    [SerializeField]
    private string _file;
    [SerializeField]
    private Inventory _inventory;
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        _state.playerPosition = player.transform.position;
        
        _state.upgrades.Clear();
        foreach(var upgrade in _inventory.upgrades)
        {
            _state.upgrades.Add(upgrade.GetType().Name);
        }
        var rooms = FindObjectsByType<EnemyHandler>(FindObjectsSortMode.InstanceID);
        _state.clearedRooms.Clear();
        for(int i = 0; i < rooms.Length; i++)
        {
            
            if (rooms[i].RoomCleared)
            {
                _state.clearedRooms.Add(i);
            }
        }

        WriteToSaveFile();

    }
    private void WriteToSaveFile()
    {
        File.WriteAllText(_file, JsonUtility.ToJson(_state));
    }
}
