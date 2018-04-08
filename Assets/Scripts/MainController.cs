using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;

    [SerializeField] private GameObject[] _optionsToSelect;
    [SerializeField] private GameObject _txtDifficultError;

    private int optionSelected = -1;
    private GameController gameController;

    private void Start()
    {
        InitScreen();
    }

    private void InitScreen()
    {
        //Hide options selected
        for (int i = 0; i < _optionsToSelect.Length; i++)
        {
            _optionsToSelect[i].SetActive(false);
        }

        optionSelected = -1;

        _txtDifficultError.SetActive(false);
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
        //Hide options and show only the selected
        for (int i = 0; i < _optionsToSelect.Length; i++)
        {
            _optionsToSelect[i].SetActive(option == i);            
        }

        //Update option selected
        optionSelected = option;

        //Hide error label
        _txtDifficultError.SetActive(false);
    }

    public void SeePlayerInfo()
    {
        OnNext(GameController.Screen.PLAYER_INFO);
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }
}
