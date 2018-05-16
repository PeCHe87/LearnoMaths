using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Screen { SPLASH, MAIN_MENU, GAMEPLAY, RESULT, PLAYER_INFO};

    [SerializeField] private ScriptablePlayerInfo _playerInfo;
    [SerializeField] private Screen _currentScreen;
    [SerializeField] private GameObject _screenSplash, _screenMainMenu, _screenGameplay, _screenResult, _screenPlayerInfo;
    [SerializeField] private ScriptableDifficulty[] _gameDifficulties;
    [SerializeField] private int _currentGameDifficulty = -1;
    [SerializeField] private DoorsController _doorsController;

    public ScriptablePlayerInfo PlayerInfo
    {
        get { return _playerInfo; }
        set { _playerInfo = value; }
    }
    public DoorsController Doors { get { return _doorsController; } }

    private void Awake()
    {
        SplashController.OnNext += NextScreen;
        MainController.OnNext += NextScreen;
        MainController.OnSelectDifficulty += SelectDifficulty;
        PlayerInformationController.OnNext += NextScreen;
        GamePlayController.OnNext += NextScreen;
        GamePlayController.OnResult += UpdateSessionResult;
        ResultController.OnNext += NextScreen;
    }

    private void Start()
    {
        _currentScreen = Screen.SPLASH;

        _playerInfo.CurrentDifficulty = -1;

        _screenSplash.GetComponent<SplashController>().SetGameController(this);
        _screenMainMenu.GetComponent<MainController>().SetGameController(this);
        //_screenGameplay.GetComponent<GameplayController>().SetGameController(this);
        //_screenResult.GetComponent<ResultController>().SetGameController(this);
        _screenPlayerInfo.GetComponent<PlayerInformationController>().SetGameController(this);

        _doorsController.OpenDoors(true);

        ShowScreen();
    }

    private void ShowScreen()
    {
        switch (_currentScreen)
        {
            case Screen.SPLASH:
                _screenSplash.SetActive(true);
                _screenMainMenu.SetActive(false);
                _screenGameplay.SetActive(false);
                _screenResult.SetActive(false);
                _screenPlayerInfo.SetActive(false);
                break;

            case Screen.MAIN_MENU:
                _screenSplash.SetActive(false);
                _screenMainMenu.SetActive(true);
                _screenMainMenu.GetComponent<MainController>().InitScreen(); // _currentScreen != Screen.SPLASH);
                _screenGameplay.SetActive(false);
                _screenResult.SetActive(false);
                _screenPlayerInfo.SetActive(false);
                break;

            case Screen.GAMEPLAY:
                _screenSplash.SetActive(false);
                _screenMainMenu.SetActive(false);
                _screenGameplay.SetActive(true);
                _screenGameplay.GetComponent<GamePlayController>().InitSession(_gameDifficulties[_currentGameDifficulty], _playerInfo, this);
                _screenResult.SetActive(false);
                _screenPlayerInfo.SetActive(false);
                break;

            case Screen.RESULT:
                _screenSplash.SetActive(false);
                _screenMainMenu.SetActive(false);
               // _screenGameplay.SetActive(false);
                _screenResult.SetActive(true);
                _screenPlayerInfo.SetActive(false);
                break;

            case Screen.PLAYER_INFO:
                _screenSplash.SetActive(false);
                _screenMainMenu.SetActive(false);
                _screenGameplay.SetActive(false);
                _screenResult.SetActive(false);
                _screenPlayerInfo.SetActive(true);
                _screenPlayerInfo.GetComponent<PlayerInformationController>().UpdateInfo();
                break;
        }
    }

    private void NextScreen(Screen next)
    {
        _currentScreen = next;

        ShowScreen();
    }

    private void UpdateSessionResult(int result, ScriptableDifficulty difficulty)
    {
        _screenResult.GetComponent<ResultController>().SetResult(result, difficulty);
    }

    private void SelectDifficulty(int dif)
    {
        _currentGameDifficulty = dif;
    }

    private void OnDestroy()
    {
        SplashController.OnNext -= NextScreen;
        MainController.OnNext -= NextScreen;
        MainController.OnSelectDifficulty -= SelectDifficulty;
        PlayerInformationController.OnNext -= NextScreen;
        GamePlayController.OnNext -= NextScreen;
        GamePlayController.OnResult -= UpdateSessionResult;
        ResultController.OnNext -= NextScreen;
    }
}

