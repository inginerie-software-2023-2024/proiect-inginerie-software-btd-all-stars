using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSaveManager", menuName = "Scriptable Objects/SaveManager")]

public class SaveManager : ScriptableObject
{
    [Serializable]
    public class GameState
    {
        public Vector2 playerPosition;
        public int coins;
        public int potions;
        public List<int> pickups;
        public List<int> clearedRooms;
    }

    public GameState state;

    [SerializeField]
    private string _file;
    [SerializeField]
    private Inventory _inventory;

    public bool saveExists = false;
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        state.playerPosition = player.transform.position;

        state.coins = _inventory.coins;
        state.potions = _inventory.potions;
        
        var rooms = FindObjectsByType<EnemyHandler>(FindObjectsSortMode.InstanceID);
        state.clearedRooms.Clear();
        for(int i = 0; i < rooms.Length; i++)
        {
            
            if (rooms[i].RoomCleared)
            {
                state.clearedRooms.Add(i);
            }
        }

        WriteToSaveFile();

    }
    public void Load()
    {
        LoadFromSaveFile();

        _inventory.coins = state.coins;
        _inventory.potions = state.potions;

    }
    private void WriteToSaveFile()
    {
        File.WriteAllText(_file, JsonUtility.ToJson(state));
    }

    private void LoadFromSaveFile()
    {
        state = JsonUtility.FromJson<GameState>(File.ReadAllText(_file));
    }
   
    public void CheckSaves()
    {
        saveExists = File.Exists(_file);
    }

    public void AddPickup(Pickup pickup)
    {
        state.pickups.Add(pickup.gameObject.GetInstanceID());
    }
    public void Clear()
    {
        state.playerPosition = new(-19, 1);
        state.coins = 0;
        state.potions = 3;
        state.clearedRooms = new();
        state.pickups = new();
    }
}
