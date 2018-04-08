using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;

    [SerializeField] private Button _btnNext;
    [SerializeField] private float _timeToAllowButton = 0.75f;

    private GameController gameController;

    private void Start()
    {
        _btnNext.enabled = false;
    }

    private void Update()
    {
        if (_btnNext.enabled)
            return;

        if (Time.timeSinceLevelLoad - _timeToAllowButton > 0)
            _btnNext.enabled = true;
    }

    public void Next()
    {
        OnNext(GameController.Screen.MAIN_MENU);
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
