using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformationController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;

    [SerializeField] private Text _txtName, _txtExperience, _txtLevel;
    [SerializeField] private InputField _ifName;

    private GameController gameController;

    private void Awake()
    {
        _txtExperience.text = "No Exp";
        _txtLevel.text = "No Lvl";
        _txtName.text = "NO name";
    }

    public void UpdateInfo()
    {
        if (gameController)
        {
            ScriptablePlayerInfo info = gameController.PlayerInfo;

            _txtExperience.text = info.CurrentExperience.ToString();
            _txtLevel.text = info.CurrentLevel.ToString();
            _txtName.text = info.Name;
            _ifName.text = info.Name;
        }
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }

    public void Close()
    {
        gameController.PlayerInfo.Name = _txtName.text;

        OnNext(GameController.Screen.MAIN_MENU);
    }
}
