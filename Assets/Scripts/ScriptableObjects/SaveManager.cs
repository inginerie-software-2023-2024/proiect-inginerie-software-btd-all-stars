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
        public Vector2 minCameraBound;
        public Vector2 maxCameraBound;
        public int health;
        public int coins;
        public int potions;
        public List<Vector3> pickups;
        public List<string> clearedRooms;
    }

    public GameState state;

    [SerializeField]
    private Weapon _glock;

    [SerializeField]
    private Weapon _crowbar;

    [SerializeField]
    private string _file;

    [SerializeField]
    private Inventory _inventory;

    private class DefaultValues
    {
        public Vector2 playerPosition = new(-19, 1);
        public Vector2 minCameraBound = new(-20.33f, -2.97f);
        public Vector2 maxCameraBound = new(-19.57f, -1.07f);
        public int health = 6;
        public int coins = 0;
        public int potions = 3;
        public int crowbarDamage = 5;
        public float glockSpeed = 0.8f;
    };

    [SerializeField]
    private DefaultValues _defaults = new();

    public bool SaveExists { get => File.Exists(_file); }
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        state.playerPosition = player.transform.position;

        var health = player.GetComponent<HealthManager>();
        state.health = health.CurrentHealth;
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

        var camera  = GameObject.FindGameObjectsWithTag("MainCamera").FirstOrDefault().GetComponent<CameraMovement>();
        state.minCameraBound = camera.minPosition;
        state.maxCameraBound = camera.maxPosition;

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
        _crowbar.damage = _defaults.crowbarDamage;
        _glock.attackSpeed = _defaults.glockSpeed;

        _inventory.upgrades.Clear();
    }

    public void AddPickup(Pickup pickup)
    {
        state.pickups.Add(pickup.transform.position);
    }

    public void Clear()
    {
        File.Delete(_file);

        state.playerPosition = _defaults.playerPosition;
        state.health = _defaults.health;
        state.coins = _defaults.coins;
        state.potions = _defaults.potions;
        state.clearedRooms = new();
        state.pickups = new();
        state.minCameraBound = _defaults.minCameraBound;
        state.maxCameraBound = _defaults.maxCameraBound;

        _inventory.coins = state.coins;
        _inventory.potions = state.potions;
        _crowbar.damage = _defaults.crowbarDamage;
        _glock.attackSpeed = _defaults.glockSpeed;

        _inventory.upgrades.Clear();
    }
}
