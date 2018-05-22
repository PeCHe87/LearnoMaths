using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;

    [SerializeField] private TextMeshProUGUI _textResult;
    [SerializeField] private GameObject[] _stars;

    public void SetResult(int correctQuestions, ScriptableDifficulty difficulty)
    {
        if (correctQuestions >= difficulty.MinCorrrectAnswersForThreeStars)
            ShowStars(3);
        else if (correctQuestions >= difficulty.MinCorrrectAnswersForTwoStars)
            ShowStars(2);
        else if (correctQuestions >= difficulty.MinCorrrectAnswersForOneStar)
            ShowStars(1);
        else
            ShowStars(0);
    }

    private void ShowStars(int stars)
    {
        //Show stars based on current result and difficulty
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].SetActive(i < stars);
        }

        //Update text based on result
        if (stars == 3)
            _textResult.text = "EXCELENTE!";
        else if (stars == 2)
            _textResult.text = "MUY BIEN";
        else if (stars == 1)
            _textResult.text = "BIEN...";
        else
            _textResult.text = "SEGUI INTENTANDO";
    }

    public void Replay()
    {
        OnNext(GameController.Screen.GAMEPLAY);
    }
}