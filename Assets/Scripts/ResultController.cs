using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;

    [SerializeField] private TextMeshProUGUI _textResult;

    public void SetResult(bool result)
    {
        if (result)
            _textResult.text = "GANASTE!";
        else
            _textResult.text = "PERDISTE!";
    }

    public void Replay()
    {
        OnNext(GameController.Screen.GAMEPLAY);
    }
}