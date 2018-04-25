using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;
    public static System.Action<int, ScriptableDifficulty> OnResult;

    [SerializeField] private TargetObject[] _targets;
    [SerializeField] private Image[] _colorGuides;
    [SerializeField] private DraggableObject[] _dragables;
    [SerializeField] private Color[] _objectColors;
    [SerializeField] private float _distanceAllow;
    [SerializeField] private TextMeshProUGUI _textResult;
    [SerializeField] private Slider _sliderProgress;
    [SerializeField] private float _speedTimeProgress;
    [SerializeField] private float _delayToShowNextQuestion = 1;
    [SerializeField] private float _delayToShowResult = 1;
    [SerializeField] private Text _txtQuestions;
    [SerializeField] private Text _txtQuestionResult;
    [SerializeField] private Text _txtExperience;

    int result = 0;
    int firstOperand = 0;
    int secondOperand = 0;
    int incorrectOperand = 0;
    ScriptableDifficulty currentDifficulty = null;
    private bool isPlaying = false;
    private float currentTimeProgress = 0;
    private int amountOfQuestions = 0;
    private int currentExperience = 0;
    private int currentQuestion = 0;
    private int correctQuestions = 0;

    #region Private methods
    private void Awake()
    {
        DraggableObject.OnFinishedDrag += DraggableObjectFinishedDrag;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        currentTimeProgress -= Time.deltaTime * _speedTimeProgress;

        float progress = currentTimeProgress / currentDifficulty.TimePerOperation;

        _sliderProgress.value = progress;

        if (currentTimeProgress <= 0)
            EndOperationByTimeout();
    }

    ///Check if it near to the allow distance to one of the posible targets availables
    private void DraggableObjectFinishedDrag(DraggableObject obj)
    {
        TargetObject target;
        float distance;
        Vector3 objPos, targetPos;

        objPos = obj.transform.position;

        for (int i = 0; i < _targets.Length; i++)
        {
            target = _targets[i];

            targetPos = target.transform.position;

            if (target.Available)
            {
                distance = Mathf.Sqrt((Mathf.Pow(objPos.x - targetPos.x, 2) + Mathf.Pow(objPos.y - targetPos.y, 2)));

                if (distance <= _distanceAllow)
                {
                    target.Available = false;

                    obj.transform.position = targetPos;

                    obj.CanDrag = false;

                    target.Answer = (obj.ContentValue == target.CorrectNumber);     // (obj.ContentValue == target.CorrectNumber && obj.ColorBackground == target.ColorBackground);

                    Debug.Log("Finish drag -> correct value: " + target.Answer);

                    //TODO: show a feedback sound/vfx of answer

                    //Control if operation has finished
                    CheckFinishOperation();

                    return;
                }
            }
        }

        obj.SetAtOrigin();  //obj.BackOriginalPosition();
    }

    private void OnDestroy()
    {
        DraggableObject.OnFinishedDrag -= DraggableObjectFinishedDrag;
    }

    private void GenerateOperation()
    {
        int minOperand = currentDifficulty.AvailableOperands[0];
        int maxOperand = currentDifficulty.AvailableOperands[currentDifficulty.AvailableOperands.Length-1];
        
        //Get first operand
        firstOperand = Random.Range(minOperand, maxOperand+1);

        //Get second operand
        secondOperand = Random.Range(minOperand, maxOperand+1);

        //Get result of the operation based on two operands
        result = firstOperand * secondOperand;

        Debug.Log("first operand: " + firstOperand + ", second: " + secondOperand + ", result: " + result);
    }

    private void LoadDragables()
    {
        DraggableObject obj;
        bool next = false;

        List<DraggableObject> dragablesToLoad = new List<DraggableObject>();

        //Make a copy of dragables to assign content
        for (int i = 0; i < _dragables.Length; i++)
        {
            _dragables[i].SetContentValue(-1);

            _dragables[i].SetAtOrigin();

            //Check if it is needed based on current difficulty
            if (i < currentDifficulty.AmountOfOptions)
            {
                _dragables[i].gameObject.SetActive(true);
                dragablesToLoad.Add(_dragables[i]);
            }
            else
            {
                _dragables[i].gameObject.SetActive(false);
            }
            
        }

        Debug.Log("Dragables to load: " + dragablesToLoad.Count);

        //Get incorrect operands based on amount 
        int amountOfIncorrectOperands = currentDifficulty.AvailableOperands.Length - 2;
        List<int> operands = new List<int>(); //{ firstOperand, secondOperand, incorrectOperand };
        operands.Add(firstOperand);
        operands.Add(secondOperand);

        int minOperand = currentDifficulty.AvailableOperands[0];
        int maxOperand = currentDifficulty.AvailableOperands[currentDifficulty.AvailableOperands.Length - 1];
        bool incorrectOperandIsOk = false;

        for (int i = 0; i < amountOfIncorrectOperands; i++)
        {
            incorrectOperandIsOk = false;

            while (!incorrectOperandIsOk)
            {
                incorrectOperand = Random.Range(minOperand, maxOperand+1);

                //Check if incorrect operand can get the correct result with one of the right operands
                if (incorrectOperand * firstOperand == result || incorrectOperand * secondOperand == result)
                    incorrectOperandIsOk = false;
                else if (operands.Contains(incorrectOperand))
                    incorrectOperandIsOk = false; // incorrectOperand != firstOperand && incorrectOperand != secondOperand;
                else
                    incorrectOperandIsOk = true;
            }

            Debug.Log("Incorrect operand [" + i + "] =  " + incorrectOperand);

            operands.Add(incorrectOperand);
        }
        
        int operandIndex = 0;
        Color color;
        List<Color> colorsUsed = new List<Color>();
        int index = -1;

        while (dragablesToLoad.Count > 0)
        {
            next = false;

            while (!next)
            {
                index = Random.Range(0, dragablesToLoad.Count);

                obj = dragablesToLoad[index];

                if (obj.ContentValue == -1)
                {
                    obj.SetContentValue(operands[operandIndex]);

                    Debug.Log("Operand: " + operandIndex + ", value: " + operands[operandIndex] + ", is <color=" + ((operandIndex>1)?"red>incorrect":"green>correct") + "</color>");

                    //Set color of dragable object
                    color = _objectColors[Random.Range(0, _objectColors.Length)];

                    while (colorsUsed.Contains(color))
                    {
                        color = _objectColors[Random.Range(0, _objectColors.Length)];
                    }

                    colorsUsed.Add(color);
                    obj.SetColor(color);

                    //If operand is first or second then load target places with their values and colors
                    if (operandIndex < 2)
                    {
                        _targets[operandIndex].Init();
                        _targets[operandIndex].SetAnswer(color, obj.ContentValue);

                        //Set color of guide
                        _colorGuides[operandIndex].color = color;
                    }

                    operandIndex++;
                    next = true;
                    dragablesToLoad.Remove(obj);
                }
            }
        }
    }

    private void LoadResult()
    {
        _textResult.text = string.Format("{00:0}", result);
    }

    private void CheckFinishOperation()
    {
        int amount = 0;
        int currentResult = 1;
        for (int i = 0; i < _dragables.Length; i++)
        {
            if (!_dragables[i].CanDrag)
            {
                currentResult = currentResult * _dragables[i].ContentValue;

                amount++;

                if (amount == 2)
                {
                    CheckResult(currentResult);
                    return;
                }
            }
        }
    }

    private void CheckResult(int res)
    {
        isPlaying = false;

        bool finalAnswer = result == res;

        if (finalAnswer)
        {
            for (int i = 0; i < _targets.Length; i++)
            {
                if (!_targets[i].Answer)
                {
                    finalAnswer = false;
                    break;
                }
            }
        }

        string sResult = "<color=";

        if (finalAnswer)
            sResult += "green";
        else
            sResult += "red";

        sResult += ">RESULT</color>";

        Debug.Log(sResult);

        CheckIfSessionHasFinished(result == res, finalAnswer);
    }

    private void CheckIfSessionHasFinished(bool operationResult, bool finalAnswer)
    {
        //Show question result
        _txtQuestionResult.text = (operationResult) ? "CORRECTA" : "INCORRECTA";

        //Increment current experience based on question result
        currentExperience += (operationResult) ? currentDifficulty.ExperienceRewardPerCorrectQuestion : 0;

        if (operationResult)
            correctQuestions++;

        //Update experience text
        _txtExperience.text = "EXP: " + currentExperience;

        //Increment question
        currentQuestion++;

        //Check if has to show next question or final result
        if (currentQuestion < amountOfQuestions)
            StartCoroutine(NextQuestion());
        else
        {
            OnResult(correctQuestions, currentDifficulty);

            StartCoroutine(LoadScreenResult());
        }
    }

    private void EndOperationByTimeout()
    {
        isPlaying = false;

        string sResult = "<color=red>RESULT</color> by <b>TIMEOUT</b>";

        Debug.Log(sResult);

        CheckIfSessionHasFinished(false, false);
    }

    private IEnumerator LoadScreenResult()
    {
        yield return new WaitForSeconds(_delayToShowResult);

        OnNext(GameController.Screen.RESULT);
    }

    private IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(_delayToShowNextQuestion);

        GenerateQuestion();
    }

    private void GenerateQuestion()
    {
        //Generate another question
        GenerateOperation();

        LoadDragables();

        LoadResult();

        //Reset slider progress
        _sliderProgress.value = 1;

        //Reset time to answer the new question
        currentTimeProgress = currentDifficulty.TimePerOperation;

        //Update text of current question
        _txtQuestions.text = (currentQuestion + 1) + "/" + amountOfQuestions;

        //Update experience text
        _txtExperience.text = "EXP: " + currentExperience;

        //Hide text question result
        _txtQuestionResult.text = string.Empty;

        isPlaying = true;
    }
    #endregion

    #region Public methods
    public void InitSession(ScriptableDifficulty difficulty)
    {
        correctQuestions = 0;

        currentDifficulty = difficulty;

        amountOfQuestions = currentDifficulty.AmountOfQuestionsPerSession;

        currentQuestion = 0;

        currentExperience = 0;

        GenerateQuestion();
    }
    #endregion
}
