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

    private string SQUARED = "²";
    private string SQUAREROOT = "√";

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
        if (tripleR == 0)
        {
            result0Text.text = rightResult.ToString();
            result1Text.text = result1.ToString();
            result2Text.text = result2.ToString();

            correctDoor = 0;
        }
        else if (tripleR == 1)
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

        if (correctDoor == -1)
        {
            Debug.LogError("correctDoor wasnt set.. check line 60 -> Calcucaltion Manager!");
            return;
        }

        currentCorrectDoor = correctDoor;
    }

    private int GET_RESULT(int a, int b, string operation)
    {
        int result;

             if (operation == ADDITION)         { result = a + b;                   }
        else if (operation == SUBSTRACTION)     { result = a - b;                   }
        else if (operation == MULTIPLICATION)   { result = a * b;                   }
        else if (operation == DIVISION)         { result = a / b;                   }
        else if (operation == SQUARED)          { result = (int)Mathf.Pow(a, 2);    }
        else if (operation == SQUAREROOT)       { result = (int)Mathf.Sqrt(a);      }
        else                                    { result = 0;                       }

        return result;
    }

    private void NotifyForException(int level)
    {
        if (level < 0 || level > 10) { Debug.LogWarning("<color=red>level num to create equation was out of range</color>");

            return;
        }
    }

    public void CreateEquation(int p_difficulty, int p_level)
    {
        switch (p_difficulty)
        {
            case (int)DIFFICULTIES.EASY:
                NotifyForException(p_level);
                MATH_EASY(p_level);
                break;
            case (int)DIFFICULTIES.MEDIUM:
                NotifyForException(p_level);
                MATH_MEDIUM(p_level);
                break;
            case (int)DIFFICULTIES.HARD:
                NotifyForException(p_level);
                MATH_HARD(p_level);
                break;
            case (int)DIFFICULTIES.GENIOUS:

                break;

        }
    }

    private void MATH_EASY(int lvl)
    {
        switch (lvl)
        {
            ////////////////////////////////////////////////ADDITION//////////////////////////////////////////
            case 1:
                // A + B = C
                // MATH->EASY->LEVEL1 [1,11]
                SET_AND_CALCULATE(1,11,ADDITION,1,11,1,3);
                break;
            case 2:
                // A + B = C
                // MATH->EASY->LEVEL2 [1,21]
                SET_AND_CALCULATE(1, 21, ADDITION, 1, 21, 1, 2);
                break;
            //////////////////////////////////////////ADDITION\SUBSTRACTION////////////////////////////////////
            case 3:
                // A +- B = +-C
                // MATH->EASY->LEVEL3 [1,21]
                SET_AND_CALCULATE(1, 21, RANDOM_OPERATION(ADDITION, SUBSTRACTION), 1, 21, 1, 2);
                break;
            case 4:
                // A +- B = +-C
                // MATH->EASY->LEVEL4 [-51,50]
                SET_AND_CALCULATE(-51, 51, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -51, 51, 1, 2);
                break;
            case 5:
                // A +- B = +-C
                // MATH->EASY->LEVEL5 [-101,101]
                SET_AND_CALCULATE(-101, 100, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -101, 101, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION////////////////////////////////////
            case 6:
                // A * B = C
                // MATH->EASY->LEVEL6 [0,5]
                SET_AND_CALCULATE(0, 5, MULTIPLICATION, 0, 5, 1, 2);
                break;
            case 7:
                // A * B = C
                // MATH->EASY->LEVEL7 [1,10]
                SET_AND_CALCULATE(1, 10, MULTIPLICATION, 1, 10, 1, 3);
                break;

            case 8:
                // A * B = +-C
                // MATH->EASY->LEVEL8 [-10,10]
                SET_AND_CALCULATE(-10, 10, MULTIPLICATION, -10, 10, 1, 3);
                break;
            //////////////////////////////////////////DIVISION/////////////////////////////////////////////////
            case 9:
                // A / B = C
                // MATH->EASY->LEVEL9 [0,10]
                SET_AND_CALCULATE(0, 10, DIVISION, 0, 10, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION\DIVISION////////////////////////////////////
            case 10:
                // A */ B = C
                // MATH->EASY->LEVEL10 [10,20]
                SET_AND_CALCULATE(10, 20, RANDOM_OPERATION(MULTIPLICATION, DIVISION), 10, 20, 1, 2);
                break;
        }
    }

    private void MATH_MEDIUM(int lvl)
    {
        switch (lvl)
        {
            ////////////////////////////////////////////////ADDITION//////////////////////////////////////////
            case 1:
                // A + B = C
                // MATH->MEDIUM->LEVEL1 [5,21]
                SET_AND_CALCULATE(5, 21, ADDITION, 5, 21, 1, 3);
                break;
            //////////////////////////////////////////ADDITION\SUBSTRACTION////////////////////////////////////
            case 2:
                // A +- B = +-C
                // MATH->MEDIUM->LEVEL2 [-20,30]
                SET_AND_CALCULATE(-20, 30, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -20, 30, 1, 2);
                break;
            case 3:
                // A +- B = +-C
                // MATH->MEDIUM->LEVEL3 [-40,50]
                SET_AND_CALCULATE(-40, 50, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -40, 50, 1, 2);
                break;
            case 4:
                // A +- B = +-C
                // MATH->MEDIUM->LEVEL4 [-101,101]
                SET_AND_CALCULATE(-101, 101, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -101, 101, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION////////////////////////////////////
            case 5:
                // A * B = C
                // MATH->MEDIUM->LEVEL5 [3,11]
                SET_AND_CALCULATE(3, 11, RANDOM_OPERATION(ADDITION, SUBSTRACTION), 3, 11, 1, 2);
                break;
            case 6:
                // A * B = C
                // MATH->MEDIUM->LEVEL6 [4,12]
                SET_AND_CALCULATE(4, 12, MULTIPLICATION, 4, 12, 1, 2);
                break;
            //////////////////////////////////////////DIVISION/////////////////////////////////////////////////
            case 7:
                // A / B = C
                // MATH->MEDIUM->LEVEL7 [0,20]
                SET_AND_CALCULATE(0, 20, DIVISION, 0, 20, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION\DIVISION////////////////////////////////////
            case 8:
                // A */ B = C
                // MATH->MEDIUM->LEVEL8 [10,15]
                SET_AND_CALCULATE(10, 15, RANDOM_OPERATION(MULTIPLICATION, DIVISION), 10, 15, 1, 2);
                break;
            case 9:
                // A² = C
                // MATH->MEDIUM->LEVEL9 [1,6]
                SET_AND_CALCULATE(1, 6, SQUARED);
                break;
            case 10:
                // A² = C
                // MATH->MEDIUM->LEVEL10 [1,8]
                SET_AND_CALCULATE(1,8, SQUARED);
                break;
        }
    }

    private void MATH_HARD(int lvl)
    {
        switch (lvl)
        {
            ////////////////////////////////////////ADDITION\SUBSTRACTION//////////////////////////////////////

            case 1:
                // A +- B = +-C
                // MATH->HARD->LEVEL1 [-50,50]
                SET_AND_CALCULATE(-50, 50, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -50, 50, 1, 2);
                break;
            case 2:
                // A +- B = +-C
                // MATH->HARD->LEVEL2 [-100,100]
                SET_AND_CALCULATE(-100, 100, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -100, 100, 1, 2);
                break;

            ////////////////////////////////////////MULTIPLICATION//////////////////////////////////////

            case 3:
                // A * B = C
                // MATH->HARD->LEVEL3 [10,30]
                SET_AND_CALCULATE(10, 30, MULTIPLICATION, 10, 30, 1, 2);
                break;
            case 4:
                // A * B = C                
                // MATH->HARD->LEVEL4 [-20,50]
                SET_AND_CALCULATE(-20, 50, MULTIPLICATION, -20, 50, 1, 2);
                break;
            
            //////////////////////////////////////////DIVISION//////////////////////////////////////////

            case 5:
                // A / B = C
                // MATH->HARD->LEVEL5 [5,50]
                SET_AND_CALCULATE(5, 50, DIVISION, 5, 50, 1, 2);
                break;

            //////////////////////////////////////////SQUARED²//////////////////////////////////////////

            case 6:
                // A² = C
                // MATH->HARD->LEVEL6 [6,15]
                SET_AND_CALCULATE(6, 15, SQUARED);
                break;
            case 7:
                // A² = C
                // MATH->HARD->LEVEL7 [4,25]
                SET_AND_CALCULATE(4, 25, SQUARED);
                break;

            //////////////////////////////////////////√SQUARE-ROOT//////////////////////////////////////////

            case 8:
                // √A = C
                // MATH->HARD->LEVEL8 [2,50]
                SET_AND_CALCULATE(2, 50, SQUAREROOT);
                break;
            case 9:
                // √A = C
                // MATH->HARD->LEVEL9 [50,150]
                SET_AND_CALCULATE(50, 150, SQUAREROOT);
                break;
            //////////////////////////////////////////√SQUARE-ROOT\SQUARED²//////////////////////////////////////////

            case 10:
                // A² = C
                // MATH->HARD->LEVEL10 [1,8]
                SET_AND_CALCULATE(50, 350, RANDOM_OPERATION(SQUARED, SQUAREROOT));
                break;
        }
    }

    private void MATH_GENIOUS(int lvl)
    {
        switch (lvl)
        {
            ////////////////////////////////////////BINOMINALS//////////////////////////////////////

            case 1:
                // X + A = C
                // MATH->HARD->LEVEL1 [-50,50]
                SET_AND_CALCULATE(-50, 50, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -50, 50, 1, 2);
                break;
            case 2:
                // A +- B = +-C
                // MATH->HARD->LEVEL2 [-100,100]
                SET_AND_CALCULATE(-100, 100, RANDOM_OPERATION(ADDITION, SUBSTRACTION), -100, 100, 1, 2);
                break;

            case 3:
                // A * B = C
                // MATH->HARD->LEVEL3 [10,30]
                SET_AND_CALCULATE(10, 30, MULTIPLICATION, 10, 30, 1, 2);
                break;
            case 4:
                // A * B = C                
                // MATH->HARD->LEVEL4 [-20,50]
                SET_AND_CALCULATE(-20, 50, MULTIPLICATION, -20, 50, 1, 2);
                break;

            case 5:
                // A / B = C
                // MATH->HARD->LEVEL5 [5,50]
                SET_AND_CALCULATE(5, 50, DIVISION, 5, 50, 1, 2);
                break;

            case 6:
                // A² = C
                // MATH->HARD->LEVEL6 [6,15]
                SET_AND_CALCULATE(6, 15, SQUARED);
                break;
            case 7:
                // A² = C
                // MATH->HARD->LEVEL7 [4,25]
                SET_AND_CALCULATE(4, 25, SQUARED);
                break;

            case 8:
                // √A = C
                // MATH->HARD->LEVEL8 [2,50]
                SET_AND_CALCULATE(2, 50, SQUAREROOT);
                break;
            case 9:
                // √A = C
                // MATH->HARD->LEVEL9 [50,150]
                SET_AND_CALCULATE(50, 150, SQUAREROOT);
                break;

            case 10:
                // A² = C
                // MATH->HARD->LEVEL10 [1,8]
                SET_AND_CALCULATE(50, 350, RANDOM_OPERATION(SQUARED, SQUAREROOT));
                break;
        }
    }

    private void SET_AND_CALCULATE(int aMin, int aMax, string operation, int bMin=0, int bMax = 0, int deMin = 0, int deMax = 0)
    {
        int temp = 0;

        int A = Random.Range(aMin, aMax);
        int B = Random.Range(bMin, bMax);

        if(operation == DIVISION)
        {
            //DIVISION EXCEPTION -> TO GET PRETTY NUMBERS
            if (A < B)
            {
                temp = A;
                A = B;
                B = temp;
            }

            if (A % B != 0)
            {
                SET_AND_CALCULATE(aMin, aMax, operation);
            }
        }else if (operation == SQUAREROOT)
        {
            float sqrRootOfA = Mathf.Sqrt(A);
            int gg = (int)Mathf.Round(sqrRootOfA);
            bool bb = (sqrRootOfA % gg) == 0 ? true : false; 
            if (!bb) { Debug.Log("Bad Number for rooting! <color=red>:</color><color=yellow>)</color");
                SET_AND_CALCULATE(aMin, aMax, operation);
            }
        }
        
        int C = GET_RESULT(A, B, operation);

        string equation = "";

        int D;
        int E; 

        if (operation == SQUARED || operation == SQUAREROOT)
        {
            equation = A.ToString() + operation;
            D = C - 2;
            E = C + 2;
        }
        else
        {
            equation = A.ToString() + " " + operation + " " + B.ToString();
            D = C - (int)Random.Range(deMin, deMax);
            E = C + (int)Random.Range(deMin, deMax);

        }
        currentEquation = equation;
        
        SET_TEXT_ELEMENTS(equation, C, D, E);
    }

    private string RANDOM_OPERATION(string operation1, string operation2)
    {
        string operation = "";

        int kkout = Random.Range(0, 11);
        operation = kkout <= 5 ? operation1 : operation2;
        return operation;
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

