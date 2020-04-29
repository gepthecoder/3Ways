using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDoor : MonoBehaviour
{
    public static int nTry = 0;
    public static int selectedDoor;
    public static bool doorPressed;
    public static bool doorChoosen;

    protected float chooseDoorTimer;

    public enum Doors
    {
        DOOR0 = 0, DOOR1, DOOR2,
    }

    void Update()
    {
        chooseDoorTimer += Time.deltaTime;


        if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.THINKING && PlayerControl.canChooseDoor && chooseDoorTimer > 7f)
        {
            //Debug.Log("<color=red>CAAAN CHOOSE DOOR</color>");
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Shooooooot <color=red>raycast</color>");

                    if (hit.transform.tag == "SF_Door")
                    {
                        Debug.Log("touched gameobject name: " + hit.transform.name);
                        nTry++;

                        if (nTry > 1)
                        {
                            //Destroy reward -> REWARD ONLY SHOWN IF PLAYER SELECTED CORRECT DOOR IN FIRST TRY
                            GameObject reward = GameObject.FindGameObjectWithTag("reward");
                            if(reward != null)
                                Destroy(reward);
                        }

                        if (hit.transform.name == "Door0")
                        {
                            // player chose DOOR 0
                            Debug.Log("door 0 choosen!");
                            selectedDoor = (int)Doors.DOOR0;
                            PlayerControl.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if (hit.transform.name == "Door1")
                        {
                            // player chose DOOR 1
                            Debug.Log("door 1 choosen!");
                            selectedDoor = (int)Doors.DOOR1;
                            PlayerControl.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if (hit.transform.name == "Door2")
                        {
                            // player chose DOOR 2
                            Debug.Log("door 2 choosen!");
                            selectedDoor = (int)Doors.DOOR2;
                            PlayerControl.canChooseDoor = false;

                            doorChoosen = true;
                            doorPressed = true;
                            chooseDoorTimer = 0;
                        }
                        else if(hit.transform.name == "door")
                        {
                            Debug.Log("CHILD HIT!");
                            // we hit the child
                            // get parent
                            if (hit.transform.parent.transform.name.Contains("0"))
                            {
                                //door 0
                                Debug.Log("door 0 choosen! - door");
                                selectedDoor = (int)Doors.DOOR0;
                                PlayerControl.canChooseDoor = false;

                                doorChoosen = true;
                                doorPressed = true;
                                chooseDoorTimer = 0;
                            }
                            else if (hit.transform.parent.transform.name.Contains("1"))
                            {
                                //door 1
                                // player chose DOOR 1
                                Debug.Log("door 1 choosen! - door");
                                selectedDoor = (int)Doors.DOOR1;
                                PlayerControl.canChooseDoor = false;

                                doorChoosen = true;
                                doorPressed = true;
                                chooseDoorTimer = 0;
                            }
                            else if (hit.transform.parent.transform.name.Contains("2"))
                            {
                                //door 2
                                // player chose DOOR 2
                                Debug.Log("door 2 choosen! - door");
                                selectedDoor = (int)Doors.DOOR2;
                                PlayerControl.canChooseDoor = false;

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
