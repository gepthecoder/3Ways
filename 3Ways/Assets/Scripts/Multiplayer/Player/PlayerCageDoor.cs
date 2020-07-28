using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCageDoor : MonoBehaviourPunCallbacks
{
    public bool playerHasBeenAttacked;

    private Transform cage0pos;
    private Transform cage1pos;
    private Transform cage2pos;

    public GameObject enemy;
    public GameObject reward;

    private PlayerCalculationManager calculations;
    private MyMovementController controller;

    public bool enemiesSpawned;
    
    void Awake()
    {
    }

    void Start()
    {
        calculations = GetComponent<PlayerCalculationManager>();
        controller = GetComponent<MyMovementController>();
        enemiesSpawned = false;
        GetValues(1);

    }

    void Update()
    {
        if (!enemiesSpawned)
        {
            Debug.Log("SPAWN REWARD & MONSTERS");

            if (calculations.currentCorrectDoor == (int)PlayerChooseDoor.Doors.DOOR0)
            {
                Instantiate(reward, cage0pos);
                Instantiate(enemy, cage1pos);
                Instantiate(enemy, cage2pos);
                enemiesSpawned = true;
                return;
            }
            else if (calculations.currentCorrectDoor == (int)PlayerChooseDoor.Doors.DOOR1)
            {
                Instantiate(reward, cage1pos);
                Instantiate(enemy, cage0pos);
                Instantiate(enemy, cage2pos);
                enemiesSpawned = true;
                return;
            }
            else if (calculations.currentCorrectDoor == (int)PlayerChooseDoor.Doors.DOOR2)
            {
                Instantiate(reward, cage2pos);
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
                GetValuesSection1(controller.isPlayer1);
                break;
            case 2:
                GetValuesSection2(controller.isPlayer1);
                break;
            default:
                break;
        }
    }

    private void GetValuesSection1(bool bPlayer1)
    {
        Transform pos0_1 = GameObject.FindGameObjectWithTag("wrongDoor0").GetComponent<Transform>();
        Transform pos0_2 = GameObject.FindGameObjectWithTag("2wrongDoor0").GetComponent<Transform>();
        cage0pos = bPlayer1 ? pos0_1 : pos0_2;

        Transform pos1_1 = GameObject.FindGameObjectWithTag("wrongDoor1").GetComponent<Transform>();
        Transform pos1_2 = GameObject.FindGameObjectWithTag("2wrongDoor1").GetComponent<Transform>();
        cage1pos = bPlayer1 ? pos1_1 : pos1_2;

        Transform pos2_1 = GameObject.FindGameObjectWithTag("wrongDoor2").GetComponent<Transform>();
        Transform pos2_2 = GameObject.FindGameObjectWithTag("2wrongDoor2").GetComponent<Transform>();
        cage2pos = bPlayer1 ? pos2_1 : pos2_2;
    }

    private void GetValuesSection2(bool bPlayer1)
    {

        Transform pos0_1 = GameObject.FindGameObjectWithTag("wrongDoor0_1").GetComponent<Transform>();
        Transform pos0_2 = GameObject.FindGameObjectWithTag("2wrongDoor0_1").GetComponent<Transform>();
        cage0pos = bPlayer1 ? pos0_1 : pos0_2;

        Transform pos1_1 = GameObject.FindGameObjectWithTag("wrongDoor1_1").GetComponent<Transform>();
        Transform pos1_2 = GameObject.FindGameObjectWithTag("2wrongDoor1_1").GetComponent<Transform>();
        cage1pos = bPlayer1 ? pos1_1 : pos1_2;

        Transform pos2_1 = GameObject.FindGameObjectWithTag("wrongDoor2_1").GetComponent<Transform>();
        Transform pos2_2 = GameObject.FindGameObjectWithTag("2wrongDoor2_1").GetComponent<Transform>();
        cage2pos = bPlayer1 ? pos2_1 : pos2_2;
    }
}
