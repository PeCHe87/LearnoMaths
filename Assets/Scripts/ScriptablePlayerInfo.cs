using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Tools/PlayerInfo", order = 1)]
public class ScriptablePlayerInfo : ScriptableObject
{
    public Data PlayerData;
    public int CurrentDifficulty;
    public int[] LevelsExperience;

    [System.Serializable]
    public struct Data
    {
        public string Name;
        public int CurrentLevel;
        public int CurrentExperience;
    }

    public int GetTotalExperienceForLevel(int level)
    {
        return LevelsExperience[level];
    }

    public void LoadData(string path)
    {
        if (!File.Exists(path))
            return;

        // Read the json from the file into a string
        string dataAsJson = File.ReadAllText(path);

        PlayerData = JsonUtility.FromJson<Data>(dataAsJson);
    }

    public void SaveData(string path)
    {
        string json = JsonUtility.ToJson(PlayerData, true);

        File.WriteAllText(path, json);
    }
}