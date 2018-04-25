using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Tools/Difficulty", order = 2)]
public class ScriptableDifficulty : ScriptableObject
{
    public string Name;
    public int ExperienceRewardPerCorrectQuestion;
    public int[] AvailableOperands;
    public float TimePerOperation;
    public int AmountOfOptions;
    public int AmountOfQuestionsPerSession;
    public int MinCorrrectAnswersForThreeStars;
    public int MinCorrrectAnswersForTwoStars;
    public int MinCorrrectAnswersForOneStar;
}