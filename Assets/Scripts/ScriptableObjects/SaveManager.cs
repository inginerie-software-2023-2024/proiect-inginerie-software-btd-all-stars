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
        public Vector2 PlayerPosition;
        public List<string> upgrades;
    }

    private GameState _state;

    [SerializeField]
    private string _file;
    [SerializeField]
    private Inventory _inventory;
    public void Save()
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        _state.PlayerPosition = player.transform.position;
        foreach(var upgrade in _inventory.upgrades)
        {
            _state.upgrades.Add(upgrade.GetType().Name);
        }

        WriteToSaveFile();

    }
    private void WriteToSaveFile()
    {
        File.WriteAllText(_file, JsonUtility.ToJson(_state));
    }
}
