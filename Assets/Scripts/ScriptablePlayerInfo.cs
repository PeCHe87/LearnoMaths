using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Tools/PlayerInfo", order = 1)]
public class ScriptablePlayerInfo : ScriptableObject
{
    public string Name;
    public int CurrentDifficulty;
    public int CurrentLevel;
    public int CurrentExperience;
    public int[] LevelsExperience;
}