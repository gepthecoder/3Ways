using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculationManager : MonoBehaviour
{
    public int currentCorrectDoor;

    public enum DIFFICULTIES { EASY = 0, MEDIUM, HARD, GENIOUS, }
    public static int currentDifficulty;
    public int nTimes = 5;

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

    void Awake()
    {
        if (PlayerPrefs.HasKey("currentDifficulty"))
        {
            // we had a previous session
            currentDifficulty = PlayerPrefs.GetInt("currentDifficulty", 0);
        }
        else
        {
            PlayerPrefs.SetInt("currentDifficulty", currentDifficulty);
        }

        GetValues(1);
    }
    void Start()
    {
        //SET_DIFFICULTY((int)DIFFICULTIES.EASY);
        CreateEquation(currentDifficulty, LevelManager.currentLevel);

        FillDivisionEquationNumbers(nTimes, currentDifficulty);
    }

    //void Update()
    //{
    //    if (LevelManager.addNewSection)
    //    {
    //        Debug.Log("add new section-> lvl manager");
    //        CreateEquation((int)DIFFICULTIES.EASY, LevelManager.currentLevel);
    //        CageScript.enemiesSpawned = false;
    //        LevelManager.addNewSection = false;
    //    }
    //}

    private List<int> Anums = new List<int>();
    private List<int> Bnums = new List<int>();

    private void FillDivisionEquationNumbers(int nTimes, int difficulty)
    {
        for (int i = 0; i < nTimes; i++)
        {
            PrepareDivisionEquations(difficulty);
        }
    }

    private void PrepareDivisionEquations(int difficulty)
    {
        int A, B, temp;
        switch (difficulty)
        {
            case (int)DIFFICULTIES.EASY:
                A = Random.Range(10, 50);
                B = Random.Range(1, 11);
                if(B > A)
                {
                    temp = A;
                    A = B;
                    B = temp;
                }
                else if(A % B != 0)
                {
                    PrepareDivisionEquations(difficulty);
                }
                else { Anums.Add(A); Bnums.Add(B); }

                break;
            case (int)DIFFICULTIES.MEDIUM:
                A = Random.Range(10, 100);
                B = Random.Range(1, 15);
                if (B > A)
                {
                    temp = A;
                    A = B;
                    B = temp;
                }
                else if (A % B != 0)
                {
                    PrepareDivisionEquations(difficulty);
                }
                else { Anums.Add(A); Bnums.Add(B); }

                break;
            case (int)DIFFICULTIES.HARD:
                A = Random.Range(10, 200);
                B = Random.Range(1, 20);
                if (B > A)
                {
                    temp = A;
                    A = B;
                    B = temp;
                }
                else if (A % B != 0)
                {
                    PrepareDivisionEquations(difficulty);
                }
                else { Anums.Add(A); Bnums.Add(B); }

                break;
            case (int)DIFFICULTIES.GENIOUS:
                A = Random.Range(10, 500);
                B = Random.Range(1, 50);
                if (B > A)
                {
                    temp = A;
                    A = B;
                    B = temp;
                }
                else if (A % B != 0)
                {
                    PrepareDivisionEquations(difficulty);
                }
                else { Anums.Add(A); Bnums.Add(B); }

                break;

            default:
                break;
        }
    }

   
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
                NotifyForException(p_level);
                MATH_GENIOUS(p_level);
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
                SET_AND_CALCULATE(1, 7, MULTIPLICATION, 1, 7, 1, 2);
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
                SET_AND_CALCULATE(1, 30, DIVISION, 1, 30, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION\DIVISION////////////////////////////////////
            case 10:
                // A */ B = C
                // MATH->EASY->LEVEL10 [10,20]
                SET_AND_CALCULATE(2, 26, RANDOM_OPERATION(MULTIPLICATION, DIVISION), 2, 26, 1, 2);
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
                SET_AND_CALCULATE(3, 11, MULTIPLICATION, 3, 11, 1, 2);
                break;
            case 6:
                // A * B = C
                // MATH->MEDIUM->LEVEL6 [4,12]
                SET_AND_CALCULATE(4, 18, MULTIPLICATION, 4, 18, 1, 2);
                break;
            //////////////////////////////////////////DIVISION/////////////////////////////////////////////////
            case 7:
                // A / B = C
                // MATH->MEDIUM->LEVEL7 [0,20]
                SET_AND_CALCULATE(2, 30, DIVISION, 2, 30, 1, 2);
                break;
            //////////////////////////////////////////MULTIPLICATION\DIVISION////////////////////////////////////
            case 8:
                // A */ B = C
                // MATH->MEDIUM->LEVEL8 [10,15]
                SET_AND_CALCULATE(5, 30, RANDOM_OPERATION(MULTIPLICATION, DIVISION), 5, 30, 1, 2);
                break;
            case 9:
                // A² = C
                // MATH->MEDIUM->LEVEL9 [2,8]
                SET_AND_CALCULATE(2, 8, SQUARED);
                break;
            case 10:
                // A² = C
                // MATH->MEDIUM->LEVEL10 [4,12]
                SET_AND_CALCULATE(4,12, SQUARED);
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
        {// X +- A = B      -> operation == ADDITION        ? X=B-A     : X=B+A
         // AX = B          -> operation == MULTIPLICATION  ? X=B/A     : X=B*A
         // √X² +- A = B    -> operation == SQUARED         ? X=√(B+-A) : X=(B+-A)²

            ////////////////////////////////////////BINOMINALS//////////////////////////////////////

            ///////////////////////////////////ADDITION\SUBSTRACTION////////////////////////////////

            case 1:
                // MATH->GENIOUS->LEVEL1 [-50,50]
                SET_AND_MANAGE_BINOMINALS(-50, 50, -50, 50);
                break;
            case 2:
                // MATH->GENIOUS->LEVEL2 [-100,100]
                SET_AND_MANAGE_BINOMINALS(-100, 100, -100, 100);
                break;
            case 3:
                // MATH->GENIOUS->LEVEL3 [-250, 250]
                SET_AND_MANAGE_BINOMINALS(-250, 250, -250, 250);
                break;
            case 4:
                // MATH->GENIOUS->LEVEL4 [-500, 500]
                SET_AND_MANAGE_BINOMINALS(-500, 500, -500, 500);
                break;
            case 5:
                // MATH->GENIOUS->LEVEL5 [-1000, 1000]
                SET_AND_MANAGE_BINOMINALS(-1000, 1000, -1000, 1000);
                break;

            ///////////////////////////////////MULTIPLICATION\DIVISION////////////////////////////////

            case 6:
                // MATH->GENIOUS->LEVEL6 [-50, 50]
                SET_AND_MANAGE_BINOMINALS(-50, 50, -50, 50, false, true, false);
                break;
            case 7:
                // MATH->GENIOUS->LEVEL7 [-100, 100]
                SET_AND_MANAGE_BINOMINALS(-100, 100, -100, 100, false, true, false);
                break;
            case 8:
                // MATH->GENIOUS->LEVEL8 [-200, 200]
                SET_AND_MANAGE_BINOMINALS(-200, 200, -200, 200, false, true, false);
                break;

            ///////////////////////////////////√SQUARE-ROOT\SQUARED²////////////////////////////////

            case 9:
                // MATH->GENIOUS->LEVEL9 [-1000, 1000]
                SET_AND_MANAGE_BINOMINALS(-300, 300, -300, 300, false, false, true);
                break;
            case 10:
                // MATH->GENIOUS->LEVEL10 [-1000, 1000]
                SET_AND_MANAGE_BINOMINALS(-300, 300, -300, 300, false, false, true);
                break;

        }
    }

    private int GET_RANDOM_INT_BETWEEN(int Xx, int Yy)
    {
        int randomNum = Random.Range(Xx, Yy);
        return randomNum;
    }


    /*
     Divison Numbers for A
         
         */
    private void SET_AND_CALCULATE(int aMin, int aMax, string operation, int bMin=0, int bMax = 0, int deMin = 0, int deMax = 0)
    {
        int temp = 0;

        int A = Random.Range(aMin, aMax);
        int B = Random.Range(bMin, bMax);
        Debug.Log("A: " + A + " B: " + B);

        if (A == 0 || B == 0)
        {
            Debug.Log("Exception found.. try again (:");
            SET_AND_CALCULATE(aMin, aMax, operation, bMin, bMax);
        }

        if (operation == DIVISION)
        {
            //DIVISION EXCEPTION -> TO GET PRETTY NUMBERS

            //if (A < B)
            //{
            //    temp = A;
            //    A = B;
            //    B = temp;
            //}
            //Debug.Log("<color=red>A: </color>" + A + "<color=green>B: </color>" + B);
            //if (A % B != 0)
            //{
            //    Debug.Log("<color=red>Retry!!</color>");
            //    SET_AND_CALCULATE(aMin, aMax, operation, bMin, bMax);
            //}
            int t = Random.Range(0, nTimes);
            A = Anums[t];
            B = Bnums[t];

        }else if (operation == SQUAREROOT)
        {
            float sqrRootOfA = Mathf.Sqrt(A);
            int gg = (int)Mathf.Round(sqrRootOfA);
            bool bb = (sqrRootOfA % gg) == 0 ? true : false; 
            if (!bb) { Debug.Log("Bad Number for rooting! <color=red>:</color><color=yellow>)</color");
                SET_AND_CALCULATE(aMin, aMax, operation, bMin, bMax);
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

    private void SET_AND_MANAGE_BINOMINALS(int aMin, int aMax, int bMin, int bMax, bool hard=true, bool veryHard=false, bool ultraHard=false)
    {
        int A = Random.Range(aMin, aMax);
        int B = Random.Range(bMin, bMax);

        if(A == 0 || B == 0)
        {
            if (veryHard) { SET_AND_MANAGE_BINOMINALS(aMin, aMax, bMin, bMax, hard, veryHard, ultraHard); }
        }

        int iTemp = Random.Range(0, 10);
        bool bChooseLeftSide = iTemp <= 5 ? true : false;

        bool bHard_Normal   = hard      && !veryHard    && !ultraHard;
        bool bHard_Harsh    = veryHard  && !ultraHard   && !hard;
        bool bHard_Mega     = ultraHard && !veryHard    && !hard;

        string sOperationChoose = "";
        string binominal = "";

        int result = 0; ;

        int D;
        int E;

        if (bChooseLeftSide)
        {
            if (bHard_Normal)
            {
                sOperationChoose = RANDOM_OPERATION(ADDITION, SUBSTRACTION);

                result = GET_NORMAL_BINOMINAL_RESULT_LX(sOperationChoose, A, B);
                binominal = GET_NORMAL_BINOMINAL_EQUATION_LX(sOperationChoose, A, B);
            }
            else if (bHard_Harsh)
            {
                sOperationChoose = RANDOM_OPERATION(MULTIPLICATION, DIVISION);

                result = GET_HARSH_BINOMINAL_RESULT_LX(sOperationChoose, A, B);
                binominal = GET_HARSH_BINOMINAL_EQUATION_LX(sOperationChoose, A, B);
            }
            else if (bHard_Mega)
            {
                sOperationChoose = RANDOM_OPERATION(SQUARED, SQUAREROOT);
                string sOperationChoose2 = RANDOM_OPERATION(ADDITION, SUBSTRACTION);

                result = GET_MEGA_BINOMINAL_RESULT_LX(sOperationChoose, sOperationChoose2, A, B, aMin, aMax, bMin, bMax, hard, veryHard, ultraHard);
                binominal = GET_MEGA_BINOMINAL_EQUATION_LX(sOperationChoose, A, B, sOperationChoose2);
            }
            else
            {
                Debug.LogWarning("Hey dude! Seems like somethings wrong with <color=green>parameter</color> entry in CalculationManager.cs...");
            }

        }
        else
        {
            if (bHard_Normal)
            {
                sOperationChoose = RANDOM_OPERATION(ADDITION, SUBSTRACTION);

                result = GET_NORMAL_BINOMINAL_RESULT_RX(sOperationChoose, A, B);
                binominal = GET_NORMAL_BINOMINAL_EQUATION_RX(sOperationChoose, A, B);
            }
            else if (bHard_Harsh)
            {
                sOperationChoose = RANDOM_OPERATION(MULTIPLICATION, DIVISION);

                result = GET_HARSH_BINOMINAL_RESULT_RX(sOperationChoose, A, B);
                binominal = GET_HARSH_BINOMINAL_EQUATION_RX(sOperationChoose, A, B);
            }
            else if (bHard_Mega)
            {
                sOperationChoose = RANDOM_OPERATION(SQUARED, SQUAREROOT);
                string sOperationChoose2 = RANDOM_OPERATION(ADDITION, SUBSTRACTION);

                result = GET_MEGA_BINOMINAL_RESULT_RX(sOperationChoose, sOperationChoose2, A, B, aMin, aMax, bMin, bMax, hard, veryHard, ultraHard);
                binominal = GET_MEGA_BINOMINAL_EQUATION_RX(sOperationChoose, A, B, sOperationChoose2);
            }
            else
            {
                Debug.LogWarning("Hey dude! Seems like something wrong with <color=green>parameter</color> entry in CalculationManager.cs...");
            }
        }

        D = result - (int)Random.Range(1, 2);
        E = result + (int)Random.Range(1, 2);

        SET_TEXT_ELEMENTS(binominal, result, D, E);
    }


    private string RANDOM_OPERATION(string operation1, string operation2)
    {
        string operation = "";

        int kkout = Random.Range(0, 11);
        operation = kkout <= 5 ? operation1 : operation2;
        return operation;
    }

    #region X +- A = B -> FUNCTIONS
    private string GET_NORMAL_BINOMINAL_EQUATION_LX(string sOperationChoose, int A, int B)
    {
        return "x" + " " + sOperationChoose + " " + A + " " + "=" + " " + B;
    }

    private int GET_NORMAL_BINOMINAL_RESULT_LX(string sOperationChoose, int A, int B)
    {
        //if A on left side of equation -> X+-A = B (B+-A)
        if (sOperationChoose == ADDITION)
        {
            //left side operation => + 
            return B - A;
        }
        else
        {
            //right side operation => -
            return B + A;
        }
    }

    private int GET_HARSH_BINOMINAL_RESULT_LX(string sOperationChoose, int A, int B)
    {
        //if A on left side of equation 
        if (sOperationChoose == MULTIPLICATION)
        {
            //left side operation => / 
            return B / A;
        }
        else
        {
            //right side operation => *
            return B * A;
        }
    }

    private string GET_HARSH_BINOMINAL_EQUATION_LX(string sOperationChoose, int A, int B)
    {
        return "x" + " " + sOperationChoose + " " + A + " " + "=" + " " + B;
    }

    private int GET_MEGA_BINOMINAL_RESULT_LX(string sOperationChoose,string sOperationChoose2, int A, int B, int aMin, int aMax, int bMin, int bMax, bool hard, bool veryHard, bool ultraHard)
    {
        //if A on left side of equation 
        if (sOperationChoose == SQUARED)
        {
            //left side operation
            int temp = sOperationChoose2 == ADDITION ? B - A : B + A;

            float sqrRootOfTemp = Mathf.Sqrt(temp);
            int gg = (int)Mathf.Round(sqrRootOfTemp);
            bool bb = (sqrRootOfTemp % gg) == 0 ? true : false;
            if (!bb)
            {
                Debug.Log("<color=red>Number not valid for operation: √x</color>");
                SET_AND_MANAGE_BINOMINALS(aMin, aMax, bMin, bMax, hard, veryHard, ultraHard);
            }

            int result = (int)Mathf.Sqrt(temp);
            return result;
        }
        else
        {
            //right side operation => ^^
            int temp = (sOperationChoose2 == ADDITION) ? (B - A) : (B + A);
            int result = (int)Mathf.Pow(temp, 2);
            return result;
        }
    }

    private string GET_MEGA_BINOMINAL_EQUATION_LX(string sOperationChoose, int A, int B, string sOperationChoose2)
    {
        if (sOperationChoose == SQUARED)
        {
            return "x" + sOperationChoose + " " + sOperationChoose2 + " " + A + " " + "=" + " " + B;
        }
        else
        {
            return sOperationChoose + "x" + " " + sOperationChoose2 + " " + A + " " + "=" + " " + B;
        }
    }
    #endregion

    #region A = X +- B -> FUNCTIONS
    private string GET_NORMAL_BINOMINAL_EQUATION_RX(string sOperationChoose, int A, int B)
    {
        return A + " " + "=" + " " + "x" + " " + sOperationChoose + " " + B;
    }

    private int GET_NORMAL_BINOMINAL_RESULT_RX(string sOperationChoose, int A, int B)
    {
        //if A on left side of equation -> X+-A = B (B+-A)
        if (sOperationChoose == ADDITION)
        {
            //left side operation => + 
            return A - B;
        }
        else
        {
            //right side operation => -
            return A + B;
        }
    }

    private int GET_HARSH_BINOMINAL_RESULT_RX(string sOperationChoose, int A, int B)
    {
        //if A on left side of equation 
        if (sOperationChoose == MULTIPLICATION)
        {
            //left side operation => / 
            return A / B;
        }
        else
        {
            //right side operation => *
            return A * B;
        }
    }

    private string GET_HARSH_BINOMINAL_EQUATION_RX(string sOperationChoose, int A, int B)
    {
        string equation = A + " " + "=" + " " + "x" + " " + sOperationChoose + " " + B;
        return equation;
    }

    private int GET_MEGA_BINOMINAL_RESULT_RX(string sOperationChoose, string sOperationChoose2, int A, int B, int aMin, int aMax, int bMin, int bMax, bool hard, bool veryHard, bool ultraHard)
    {
        //if X on RIGHT side of equation 
        if (sOperationChoose == SQUARED)
        {
            //left side operation
            int temp = sOperationChoose2 == ADDITION ? A - B : A + B;

            float sqrRootOfTemp = Mathf.Sqrt(temp);
            int gg = (int)Mathf.Round(sqrRootOfTemp);
            bool bb = (sqrRootOfTemp % gg) == 0 ? true : false;
            if (!bb)
            {
                Debug.Log("<color=red>Number not valid for operation: √x</color>");
                SET_AND_MANAGE_BINOMINALS(aMin, aMax, bMin, bMax, hard, veryHard, ultraHard);
            }

            int result = (int)Mathf.Sqrt(temp);
            return result;
        }
        else
        {
            //right side operation => ^^
            int temp = (sOperationChoose2 == ADDITION) ? A - B : A + B;
            int result = (int)Mathf.Pow(temp, 2);
            return result;
        }
    }

    private string GET_MEGA_BINOMINAL_EQUATION_RX(string sOperationChoose, int A, int B, string sOperationChoose2)
    {
        if (sOperationChoose == SQUARED)
        {
            string equation = A + " " + "=" + " " + "x" + sOperationChoose + " " + sOperationChoose2 + " " + B;
            return equation;
        }
        else
        {
            string equation = A + " " + "=" + " " + sOperationChoose + "x" + " " + sOperationChoose2 + " " + B;
            return equation;
        }
    }

    #endregion
}


// DIFFICULTY

// EASY -> 5 SECTIONS

// MEDIUM -> 8 SECTIONS

// HARD -> 10 SECTIONS

// GENIOUS -> 15 SECTIONS

