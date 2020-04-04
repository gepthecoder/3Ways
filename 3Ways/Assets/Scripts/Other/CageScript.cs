using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public static bool playerHasBeenAttacked;

    private Transform cage0pos;
    private Transform cage1pos;
    private Transform cage2pos;

    public GameObject enemy;

    private CalculationManager calculations;

    public static bool enemiesSpawned;


    void Awake()
    {
        GetValues(1);
    }

    void Start()
    {
        calculations = GetComponent<CalculationManager>();
        enemiesSpawned = false;
    }
     
    void Update()
    {
        if (!enemiesSpawned)
        {
            if(calculations.currentCorrectDoor == (int)ChooseDoor.Doors.DOOR0)
            {
                Instantiate(enemy, cage1pos);
                Instantiate(enemy, cage2pos);
                enemiesSpawned = true;
                return;
            }
            else if (calculations.currentCorrectDoor == (int)ChooseDoor.Doors.DOOR1)
            {
                Instantiate(enemy, cage0pos);
                Instantiate(enemy, cage2pos);
                enemiesSpawned = true;
                return;
            }
            else if (calculations.currentCorrectDoor == (int)ChooseDoor.Doors.DOOR2)
            {
                Instantiate(enemy, cage0pos);
                Instantiate(enemy, cage1pos);
                enemiesSpawned = true;
                return;
            }
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
        cage0pos = GameObject.FindGameObjectWithTag("wrongDoor0").GetComponent<Transform>();
        cage1pos = GameObject.FindGameObjectWithTag("wrongDoor1").GetComponent<Transform>();
        cage2pos = GameObject.FindGameObjectWithTag("wrongDoor2").GetComponent<Transform>();
    }

    private void GetValuesSection2()
    {
        cage0pos = GameObject.FindGameObjectWithTag("wrongDoor0_1").GetComponent<Transform>();
        cage1pos = GameObject.FindGameObjectWithTag("wrongDoor1_1").GetComponent<Transform>();
        cage2pos = GameObject.FindGameObjectWithTag("wrongDoor2_1").GetComponent<Transform>();

    }

}
