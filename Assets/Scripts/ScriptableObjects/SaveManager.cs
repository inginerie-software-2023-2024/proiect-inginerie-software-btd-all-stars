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
        public int health;
        public int coins;
        public int potions;
        public List<Vector3> pickups;
        public List<string> clearedRooms;
    }

    public GameState state;

    [SerializeField]
    private string _file;
    [SerializeField]
    private Inventory _inventory;

    public bool SaveExists { get => File.Exists(_file); }
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        state.playerPosition = player.transform.position;

        state.health = player.GetComponent<HealthManager>().CurrentHealth;
        state.coins = _inventory.coins;
        state.potions = _inventory.potions;
        
        var rooms = FindObjectsByType<EnemyHandler>(FindObjectsSortMode.None);
        state.clearedRooms.Clear();
        foreach(var room in rooms)
        {
            
            if (room.RoomCleared)
            {
                state.clearedRooms.Add(room.gameObject.name);
            }
        }

        WriteToSaveFile();

    }
    public void Load()
    {
        if (SaveExists)
        {
            LoadFromSaveFile();
        }
        else
        {
            Clear();
        }

    }
    private void WriteToSaveFile()
    {
        File.WriteAllText(_file, JsonUtility.ToJson(state));
    }

    private void LoadFromSaveFile()
    {
        state = JsonUtility.FromJson<GameState>(File.ReadAllText(_file));
        _inventory.coins = state.coins;
        _inventory.potions = state.potions;
        _inventory.upgrades.Clear();
    }

    public void AddPickup(Pickup pickup)
    {
        state.pickups.Add(pickup.transform.position);
    }
    public void Clear()
    {
        File.Delete(_file);

        state.playerPosition = new(-19, 1);
        state.health = 10;
        state.coins = 0;
        state.potions = 3;
        state.clearedRooms = new();
        state.pickups = new();

        _inventory.coins = state.coins;
        _inventory.potions = state.potions;
        _inventory.upgrades.Clear();
    }
}
