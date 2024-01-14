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
        public float playerSpeed;
        public int playerArmor;
        public int health;
        public int coins;
        public int potions;
        public int crowbarDamage;
        public float glockFiring;
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

    public bool SaveExists { get => File.Exists(_file); }
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        state.playerPosition = player.transform.position;

        var health = player.GetComponent<HealthManager>();
        state.health = health.CurrentHealth;
        state.playerArmor = health.Armor;
        state.coins = _inventory.coins;
        state.potions = _inventory.potions;
        state.glockFiring = _glock.attackSpeed;
        state.crowbarDamage = _crowbar.damage;
        state.playerSpeed = player.GetComponent<PlayerController>().speed;
        
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
        _crowbar.damage = state.crowbarDamage;
        _glock.attackSpeed = state.glockFiring;

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
        state.health = 6;
        state.coins = 0;
        state.potions = 3;
        state.crowbarDamage = 5;
        state.glockFiring = 0.8f;
        state.playerSpeed = 5f;
        state.playerArmor = 0;
        state.clearedRooms = new();
        state.pickups = new();
        state.minCameraBound = new Vector2(-20.33f, -2.97f);
        state.maxCameraBound = new Vector2(-19.57f, -1.07f);

        _inventory.coins = state.coins;
        _inventory.potions = state.potions;
        _crowbar.damage = state.crowbarDamage;
        _glock.attackSpeed = state.glockFiring;

        _inventory.upgrades.Clear();
    }
}
