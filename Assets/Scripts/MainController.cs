using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public static Action<GameController.Screen> OnNext;
    public static Action<int> OnSelectDifficulty;

    [SerializeField] private Image[] _optionsToSelect, _spriteLights;
    [SerializeField] private GameObject _optionsDoors;
    [SerializeField] private GameObject _mainDoors;
    [SerializeField] private GameObject _doorSelectedContent;
    [SerializeField] private GameObject _txtDifficultError;
    [SerializeField] private Button _btnDifficulty1, _btnDifficulty2, _btnDifficulty3;
    [SerializeField] private Sprite[] _spriteButtonsOn, _spriteButtonsOff;
    [SerializeField] private Sprite _spriteLightOn, _SpriteLightOff;
    [SerializeField] private Sprite _spriteButtonStartOn, _SpriteButtonStartOff;
    [SerializeField] private Image _imgButtonStart;
    [SerializeField] private float _timeToShowGameplay;

    private int optionSelected = -1;
    private GameController gameController;

    private void Start()
    {
        InitScreen();
    }

    public void InitScreen()
    {
        _imgButtonStart.sprite = _SpriteButtonStartOff;

        //Hide options selected
        for (int i = 0; i < _optionsToSelect.Length; i++)
        {
            _optionsToSelect[i].sprite = _spriteButtonsOff[i];
            _spriteLights[i].sprite = _SpriteLightOff;
        }

        optionSelected = -1;

        //Disabled buttons
        _btnDifficulty1.enabled = false;
        _btnDifficulty2.enabled = false;
        _btnDifficulty3.enabled = false;

        //Deactivate difficult options
        _doorSelectedContent.SetActive(false);

        if (gameController)
            gameController.Doors.OpenDoors(true);

        //Show main doors
        _optionsDoors.SetActive(true);

        _txtDifficultError.SetActive(false);
    }

    public void InitGamePlay()
    {
        //yield return new WaitForSeconds(_timeToShowGameplay);

        if (optionSelected == -1)
            return;

        gameController.Doors.CloseDoors(ShowGamePlay);
    }

    private void ShowGamePlay()
    {
        Debug.Log("ShowGamePlay");

        //Hide difficulty options
        _doorSelectedContent.SetActive(false);

        //Show gameplay screen
        Next();

        //Open doors to reveal the gameplay scene
        gameController.Doors.OpenDoors();
    }

    public void Next()
    {
        if (optionSelected == -1)
            _txtDifficultError.SetActive(true);
        else
        {
            //Update difficult selected
            gameController.PlayerInfo.CurrentDifficulty = optionSelected;

            Debug.Log("Difficulty selected: " + optionSelected);

            OnNext(GameController.Screen.GAMEPLAY);
        }
    }

    public void SelectOption(int option)
    {
        if (optionSelected == option)
            return;

        //Hide options and show only the selected
        for (int i = 0; i < _optionsToSelect.Length; i++)
        {
            //Change sprite
            if (option == i)
            {
                _optionsToSelect[i].sprite = _spriteButtonsOn[i];
                _spriteLights[i].sprite = _spriteLightOn;
            }
            else
            {
                _optionsToSelect[i].sprite = _spriteButtonsOff[i];
                _spriteLights[i].sprite = _SpriteLightOff;
            }
        }

        //Update option selected
        optionSelected = option;

        OnSelectDifficulty(option);

        //Hide error label
        _txtDifficultError.SetActive(false);

        //StartCoroutine(InitGamePlay());

        _imgButtonStart.sprite = _spriteButtonStartOn;
    }

    public void SelectDoor(int option)
    {
        if (option == 0)
        {
            //TODO: update doors' operator symbol

            //Start animation to close doors
            gameController.Doors.CloseDoors(ActivateOptions);
        }
        else
        {
            //TODO: for the rest of the operations
        }
    }

    private void ActivateOptions()
    {
        Debug.Log("ActivateOptions");

        //Activate difficult options
        _doorSelectedContent.SetActive(true);

        //Hide main doors
        _optionsDoors.SetActive(false);

        //Start animation open doors (When animation of open finishes enable buttons of difficulty)
        gameController.Doors.OpenDoors(false, EnableDifficultyButtons);
    }

    private void EnableDifficultyButtons()
    {
        _btnDifficulty1.enabled = true;
        _btnDifficulty2.enabled = true;
        _btnDifficulty3.enabled = true;
    }

    public void SeePlayerInfo()
    {
        OnNext(GameController.Screen.PLAYER_INFO);
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }

    public void CleanPlayerProgress()
    {
        gameController.PlayerInfo.CurrentExperience = 0;
        gameController.PlayerInfo.CurrentLevel = 0;
        gameController.PlayerInfo.CurrentDifficulty = -1;
    }
}
