using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculationManager : MonoBehaviour
{
    public int currentCorrectDoor;

    public enum DIFFICULTIES { EASY = 0, MEDIUM, HARD, GENIOUS, }
    public int currentDifficulty;

    private TextMesh equationText;

    private TextMesh result0Text;
    private TextMesh result1Text;
    private TextMesh result2Text;

    private string ADDITION = "+";
    private string SUBSTRACTION = "-";
    private string MULTIPLICATION = "x";
    private string DIVISION = "/";

    string currentEquation;

    public void GetValues(int section)
    {
        switch (section)
        {
            case 1:
                GetValuesSection1();
                break;
            case 2:
                GetValuesSection2();
                break;
            default:
                break;
        }
    }

    private void GetValuesSection1()
    {
        equationText = GameObject.FindGameObjectWithTag("equation").GetComponent<TextMesh>();

        result2Text = GameObject.FindGameObjectWithTag("result2").GetComponent<TextMesh>();
        result0Text = GameObject.FindGameObjectWithTag("result0").GetComponent<TextMesh>();
        result1Text = GameObject.FindGameObjectWithTag("result1").GetComponent<TextMesh>();
    }

    private void GetValuesSection2()
    {
        equationText = GameObject.FindGameObjectWithTag("equation1").GetComponent<TextMesh>();

        result2Text = GameObject.FindGameObjectWithTag("result2_1").GetComponent<TextMesh>();
        result0Text = GameObject.FindGameObjectWithTag("result0_1").GetComponent<TextMesh>();
        result1Text = GameObject.FindGameObjectWithTag("result1_1").GetComponent<TextMesh>();
    }

    void Awake()
    {
        GetValues(1);
    }
    void Start()
    {
        SET_DIFFICULTY((int)DIFFICULTIES.EASY);
        CreateEquation(currentDifficulty, LevelManager.currentLevel);
    }

    void Update()
    {
        if (LevelManager.addNewSection)
        {
            CreateEquation((int)DIFFICULTIES.EASY, LevelManager.currentLevel);
            LevelManager.addNewSection = false;
        }
    }
   
    public void SET_DIFFICULTY(int difficulty)
    {
        currentDifficulty = difficulty;
    }

    public int GET_DIFFICULTY()
    {
        return currentDifficulty;
    }

    private void CLEAR_TEXT_ELEMENTS()
    {
        equationText.text = "";

        result0Text.text = "";
        result1Text.text = "";
        result2Text.text = "";
    }

    private void SET_TEXT_ELEMENTS(string equation, int rightResult, int result1, int result2)
    {
        CLEAR_TEXT_ELEMENTS();

        equationText.text = equation;

        int correctDoor = -1;

        int tripleR = Random.Range(0, 3);
        
        Debug.Log(tripleR + "  TRIPLE R");
        if(tripleR == 0)
        {
            result0Text.text = rightResult.ToString();
            result1Text.text = result1.ToString();
            result2Text.text = result2.ToString();

            correctDoor = 0;
        }
        else if(tripleR == 1)
        {
            result0Text.text = result1.ToString();
            result1Text.text = result2.ToString();
            result2Text.text = rightResult.ToString();

            correctDoor = 2;
        }
        else if (tripleR == 2)
        {
            result0Text.text = result2.ToString();
            result1Text.text = rightResult.ToString();
            result2Text.text = result1.ToString();

            correctDoor = 1;
        }

        if(correctDoor == -1)
        {
            Debug.LogError("correctDoor wasnt set.. check line 60 -> Calcucaltion Manager!");
            return;
        }

        currentCorrectDoor = correctDoor;
    }

    private int GET_RESULT(int a, int b, string operation)
    {
        int result;

        if(operation == ADDITION)
        {
            result = a + b;
        }
        else if (operation == SUBSTRACTION)
        {
            result = a - b;
        }
        else if (operation == MULTIPLICATION)
        {
            result = a * b;
        }
        else if (operation == DIVISION)
        {
            result = a / b;
        }
        else { result = 0; }

        return result;
    }

    public void CreateEquation(int p_difficulty, int level)
    {
        switch (p_difficulty)
        {
            case (int)DIFFICULTIES.EASY:
                switch (level)
                {
                    case 1:
                        // A + B = C
                        // MATH->EASY->LEVEL1
                        string operation1 = ADDITION;
                        int A = Random.Range(1, 11);
                        int B = Random.Range(1, 11);
                        int C = GET_RESULT(A, B, operation1);

                        string equation1 = A.ToString() + " " + operation1 + " " + B.ToString();
                        currentEquation = equation1;

                        int D = C - (int)Random.Range(1, 3);
                        int E = C + (int)Random.Range(1, 3);

                        SET_TEXT_ELEMENTS(equation1, C, D, E);
                        break;

                    case 2:
                        // A + B = C
                        // MATH->EASY->LEVEL2
                        string operation2 = ADDITION;
                        int Aa = Random.Range(5, 21);
                        int Bb = Random.Range(5, 21);
                        int Cc= GET_RESULT(Aa, Bb, operation2);

                        string equation2 = Aa.ToString() + " " + operation2 + " " + Bb.ToString();
                        currentEquation = equation2;

                        int Dd = Cc - (int)Random.Range(1, 2);
                        int Ee= Cc + (int)Random.Range(1, 2);

                        SET_TEXT_ELEMENTS(equation2, Cc, Dd, Ee);
                        break;
                    case 3:
                        // A +- B = C
                        // MATH->EASY->LEVEL3
                        int r = Random.Range(0, 100);
                        string operation3 = r <= 50 ? ADDITION : SUBSTRACTION;
                        int Aaa = Random.Range(5, 21);
                        int Bbb = Random.Range(5, 21);
                        int Ccc = GET_RESULT(Aaa, Bbb, operation3);

                        string equation3 = Aaa.ToString() + " " + operation3 + " " + Bbb.ToString();
                        currentEquation = equation3;

                        int Ddd = Ccc - (int)Random.Range(1, 2);
                        int Eee = Ccc + (int)Random.Range(1, 2);

                        SET_TEXT_ELEMENTS(equation3, Ccc, Ddd, Eee);
                        break;
                }
                break;

            case (int)DIFFICULTIES.MEDIUM:
                break;
            case (int)DIFFICULTIES.HARD:
                break;
            case (int)DIFFICULTIES.GENIOUS:
                break;

        }
        //if (p_difficulty == (int)DIFFICULTIES.EASY)
        //{
        //    if (level == 1)
        //    {
        //        // A + B = C
        //        // MATH->EASY->LEVEL1
        //        string operation = ADDITION;
        //        int A = Random.Range(1, 11);
        //        int B = Random.Range(1, 11);
        //        int C = GET_RESULT(A, B, operation);

        //        string equation = A.ToString() + " " + operation + " " + B.ToString();
        //        currentEquation = equation;
                
        //        int D = C - (int)Random.Range(1, 2);
        //        int E = C + (int)Random.Range(1, 2);

        //        SET_TEXT_ELEMENTS(equation, C, D, E);
        //    }

        //    if (level == 2)
        //    {
        //        // A + B = C
        //        // MATH->EASY->LEVEL1
        //        string operation = ADDITION;
        //        int A = Random.Range(1, 11);
        //        int B = Random.Range(1, 11);
        //        int C = GET_RESULT(A, B, operation);

        //        string equation = A.ToString() + " " + operation + " " + B.ToString();
        //        currentEquation = equation;

        //        int D = C - (int)Random.Range(1, 2);
        //        int E = C + (int)Random.Range(1, 2);

        //        SET_TEXT_ELEMENTS(equation, C, D, E);
        //    }
        //}
    }
}


// A (operation) B = C
// "A" + "+(operation)+" + "B" -> equationText
// "C" -> Random.Range(result0Text,result2Text) -> set door!!

// DIFFICULTY

// EASY -> 5 SECTIONS
//LEVEL 1   OPERATION +   (0,10)
//LEVEL 2   OPERATION +   (0,20)
//LEVEL 3   OPERATION +-  (0,20)
//LEVEL 4   OPERATION +-  (-50,50)
//LEVEL 5   OPERATION +-  (-100,100)

//LEVEL 6   OPERATION *   (0,5)
//LEVEL 7   OPERATION *   (0,10)
//LEVEL 8   OPERATION *   (-10,10)
//LEVEL 9   OPERATION */  (0,10)
//LEVEL 10  OPERATION */  (10,20) .......

// MEDIUM -> 8 SECTIONS

// HARD -> 10 SECTIONS

// GENIOUS -> 15 SECTIONS

