using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Tools/Difficulty", order = 2)]
public class ScriptableDifficulty : ScriptableObject
{
    public string Name;
    public int ExperienceReward;
    public int[] AvailableOperands;
    public float TimePerOperation;
    public int AmountOfOptions;
}