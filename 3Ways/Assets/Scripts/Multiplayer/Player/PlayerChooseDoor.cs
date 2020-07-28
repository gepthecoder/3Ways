using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerChooseDoor : MonoBehaviourPunCallbacks
{
    public int nTry = 0;
    public int selectedDoor;

    public bool doorPressed;
    public bool doorChoosen;

    protected float chooseDoorTimer;

    private AvatarControlHandler controller;

    private PhotonView PV;

    public enum Doors
    {
        DOOR0 = 0, DOOR1, DOOR2,
    }

    void Start()
    {
        controller = GetComponent<AvatarControlHandler>();
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PV.IsMine) { return; }

        if (!controller.canPlay) { return; }

        chooseDoorTimer += Time.deltaTime;

        if (controller.canChooseDoor && chooseDoorTimer > 7f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "SF_Door")
                    {
                        nTry++;

                        //if (nTry > 1)
                        //{
                        //    //Destroy reward -> REWARD ONLY SHOWN IF PLAYER SELECTED CORRECT DOOR IN FIRST TRY
                        //    //TO:DO -> GameObject reward = GameObject.FindGameObjectWithTag("reward");
                        //    if (reward != null)
                        //        Destroy(reward);
                        //}

                        if (hit.transform.name == "Door0")
                        {
                            // player chose DOOR 0
                            selectedDoor = (int)Doors.DOOR0;
                            controller.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if (hit.transform.name == "Door1")
                        {
                            // player chose DOOR 1
                            selectedDoor = (int)Doors.DOOR1;
                            controller.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if (hit.transform.name == "Door2")
                        {
                            // player chose DOOR 2
                            selectedDoor = (int)Doors.DOOR2;
                            controller.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if (hit.transform.name == "door")
                        {
                            Debug.Log("CHILD HIT!");
                            // we hit the child
                            // get parent
                            if (hit.transform.parent.transform.name.Contains("0"))
                            {
                                //door 0
                                selectedDoor = (int)Doors.DOOR0;
                                controller.canChooseDoor = false;

                                doorChoosen = true;
                                doorPressed = true;
                                chooseDoorTimer = 0;
                            }
                            else if (hit.transform.parent.transform.name.Contains("1"))
                            {
                                //door 1
                                // player chose DOOR 1
                                selectedDoor = (int)Doors.DOOR1;
                                controller.canChooseDoor = false;

                                doorChoosen = true;
                                doorPressed = true;
                                chooseDoorTimer = 0;
                            }
                            else if (hit.transform.parent.transform.name.Contains("2"))
                            {
                                //door 2
                                // player chose DOOR 2
                                selectedDoor = (int)Doors.DOOR2;
                                controller.canChooseDoor = false;

                                doorChoosen = true;
                                doorPressed = true;
                                chooseDoorTimer = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
