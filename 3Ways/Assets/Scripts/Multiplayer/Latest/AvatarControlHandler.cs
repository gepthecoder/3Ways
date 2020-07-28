using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonAnimatorView))]
public class AvatarControlHandler : MonoBehaviourPunCallbacks
{
    public int iCurrentState;

    public enum PlayerStates
    {
        SPAWNING = 0,
        RUNNING,
        THINKING,
        UNLOCKING,
        ENTERING,
        PASS,
        REPEAT,
        TRANSITION,
        WIN,
    }

    public int iTempState;

    public enum TempState
    {
        OTHER = 0,
        NEXT_STOP_WIN_ROOM,
    }

    private CharacterController         controller;
    private PlayerSetup                 playerSetup;

    private PhotonView                  PV;
    private PlayerChooseDoor            PCD;
    private PlayerCalculationManager    PCM;
    private MapSpawner                  MS;
    private PlayerDanceMoves            PDM;

    private CameraFollow                CF;

    protected bool endOfGame = false;
    private Animator playerAnime;

    public bool isMasterClient;
    public bool isMINE;

    public bool canChooseDoor;
    public bool doorAnimeOpened;
    public bool transition;
    public bool isWinningSection;

    public int currentSection = 0;

    [Header("Player Movement")]
    [Space(10)]
    [SerializeField]
    private float moveSpeed = 10f;
    [Space(5)]
    [SerializeField]
    private float transitionSpeed = 6.0f;
    [Space(5)]
    [SerializeField]
    private float fallBackSpeed = 2.0f;
    [Space(5)]
    [SerializeField]
    private float passSpeed = 5f;

    private float timer_runAnime;


    [Header("Transform Targets")]
    [Space(10)]
    public Transform centerPlayerPos;
    [Space(5)]
    public Transform centerDoorLookPos;
    [Space(10)]
    [SerializeField]
    private Transform door0Pos;
    [Space(5)]
    [SerializeField]
    private Transform door1Pos;
    [Space(5)]
    [SerializeField]
    private Transform door2Pos;
    [Space(10)]
    public Transform passCagePos;
    [Space(5)]
    public Transform passWinPos;
    [Space(5)]
    public Transform passWinLookAt;
    [Space(5)]
    public Transform cageDoor0Pos;
    [Space(5)]
    public Transform cageDoor1Pos;
    [Space(5)]
    public Transform cageDoor2Pos;
    [Space(5)]
    public Transform dancePos;

    [Space(10)]
    public bool stopPlayer;

    public bool canPlay;

void Awake()
    {
        isMasterClient = PhotonNetwork.IsMasterClient;
    }

    void Start()
    {
        PV      = GetComponent<PhotonView>();
        isMINE  = PV.IsMine;
        if (!PV.IsMine) { return; }

        controller  = GetComponent<CharacterController>();
        playerSetup = GetComponent<PlayerSetup>();
        playerAnime = GetComponent<Animator>();

        PCD = GetComponent<PlayerChooseDoor>();
        PCM = GetComponent<PlayerCalculationManager>();
        MS  = GetComponent<MapSpawner>();
        PDM = GetComponent<PlayerDanceMoves>();
        CF = GetComponentInChildren<CameraFollow>();

        GET_CURRENT_POSITIONS(1, isMasterClient);

        canChooseDoor   = true;
        doorAnimeOpened = false;
        canPlay = true;
        jumpOnce = true;

        PLAYER_HAS_WON_THE_GAME = false;
    }

    void Update()
    {
        if (!PV.IsMine) { return; }

        HandleFinnish();

        if (GameSetup.GS.bSTART_GAME && canPlay)
        {

            #region PLAYER 1
            if (isMasterClient)
            { // PLAYER 1

                //STATE-MACHINE
                if (transition)
                {
                    //passCagePos = GetSelectedCageDoorTransform(PCD.selectedDoor);
                    //if(passCagePos == null) {
                    //    HandleValues();
                    //    passCagePos = GetSelectedCageDoorTransform(PCD.selectedDoor); }
                    //transform.LookAt(passCagePos);
                    HandleValues();
                    iCurrentState = (int)PlayerStates.TRANSITION;
                    transition = false;
                }
                else
                {
                    if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.RUNNING)
                    {
                        iCurrentState = (int)PlayerStates.THINKING;
                    }

                    else
                    {
                        if (!StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.SPAWNING)
                        {
                            iCurrentState = (int)PlayerStates.RUNNING;
                        }

                    }

                    if (PCD.doorPressed)
                    {
                        iCurrentState = (int)PlayerStates.UNLOCKING;
                        PCD.doorPressed = false;
                    }

                    if (OpenDoor.inFrontOfDoor)
                    {
                        iCurrentState = (int)PlayerStates.ENTERING;

                        OpenDoor.inFrontOfDoor = false;
                        PCD.doorPressed = false;
                        StopCollider.choosingPosition = false;
                    }

                    if (PCM.currentCorrectDoor == PCD.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && doorAnimeOpened)
                    {
                        iCurrentState = (int)PlayerStates.PASS;

                        MS.currentSectionCount++;
                        MS.currentLevel++;

                        PCD.nTry = 0;
                        if (MS.currentSectionCount == MS.GetNumberOfSections((int)PlayerCalculationManager.DIFFICULTIES.EASY))
                        {
                            iTempState = (int)TempState.NEXT_STOP_WIN_ROOM;
                            MS.spawnWinSection = true;
                            return;
                        }
                        else if (MS.currentSectionCount >= 2)
                        {
                            MS.spawnNewSection = true;
                            iTempState = (int)TempState.OTHER;
                            doorAnimeOpened = false;
                        }
                        else if (MS.currentSectionCount == 1)
                        {
                            StartCoroutine(PCM.TransitionToNextLevel());
                            iTempState = (int)TempState.OTHER;
                            doorAnimeOpened = false;
                        }
                    }
                    else if (PCM.currentCorrectDoor != PCD.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && doorAnimeOpened)
                    {
                        iCurrentState = (int)PlayerStates.REPEAT;
                        doorAnimeOpened = false;
                    }

                    if (StopCollider.choosingPosition && iCurrentState == (int)PlayerStates.REPEAT && transform.position == centerPlayerPos.position)
                    {
                        StartCoroutine(LookStraight());
                        iCurrentState = (int)PlayerStates.THINKING;
                    }

                }
                //STATE-HANDLERS
                if (iCurrentState == (int)PlayerStates.RUNNING)
                {
                    PLAYER_MOVEMENT(centerPlayerPos);
                }
                else if (iCurrentState == (int)PlayerStates.UNLOCKING)
                {
                    photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, false);
                    PLAY_ANIMATION_DOOR_TRANSITION();
                    GoToDoor(PCD.selectedDoor);
                }
                else if (iCurrentState == (int)PlayerStates.THINKING)
                {
                    photonView.RPC("PLAY_ANIMATION_FALL_BACK", RpcTarget.All, false);
                    photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
                    photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, true);
                }
                else if (iCurrentState == (int)PlayerStates.ENTERING)
                {
                    photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
                    //StartCoroutine(FaceOFF());
                    photonView.RPC("FACE_YOUR_DESTINY", RpcTarget.All, null);
                }
                else if (iCurrentState == (int)PlayerStates.PASS)
                {
                    //Debug.Log("<color=green>PASS</color>");
                    photonView.RPC("RPC_Continue", RpcTarget.All);
                }
                else if (iCurrentState == (int)PlayerStates.REPEAT)
                {
                    Debug.Log("<color=red>REPEAT</color>");
                    StartCoroutine(WaitSec(1f));
                    photonView.RPC("PLAY_ANIMATION_FALL_BACK", RpcTarget.All, true);
                    float step = fallBackSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, centerPlayerPos.position, step);
                    StartCoroutine(CloseDoor());
                    Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
                    StartCoroutine(AgainCanChooseDoor());
                    //photonView.RPC("RPC_CloseDoor", RpcTarget.All);
                }
                else if (iCurrentState == (int)PlayerStates.TRANSITION)
                {
                    if (isWinningSection)
                    {
                        GoToEndPosition(isMasterClient);
                    }
                    else
                    {
                        transform.LookAt(centerDoorLookPos);
                        iCurrentState = (int)PlayerStates.RUNNING;
                    }
                    #region source code
                    //if (isMasterClient)
                    //{
                    //    if (!door.slide)
                    //    {
                    //        MoveOn();
                    //    }
                    //    else
                    //    { // SLIDE
                    //        PLAY_ANIMATION_RUN(false);
                    //        Slide();
                    //    }

                    //    float step = passSpeed * Time.deltaTime;
                    //    transform.position = Vector3.MoveTowards(transform.position, passCagePos.position, step);
                    //}
                    //else
                    //{
                    //    if (!door2.slide)
                    //    {
                    //        MoveOn();
                    //    }
                    //    else
                    //    { // SLIDE
                    //        PLAY_ANIMATION_RUN(false);
                    //        Slide();
                    //    }

                    //    float step = passSpeed * Time.deltaTime;
                    //    transform.position = Vector3.MoveTowards(transform.position, passCagePos.position, step);
                    //}
                    #endregion

                }
                else if (iCurrentState == (int)PlayerStates.WIN)
                {
                    GoToDancePosition();

                }

            }
            #endregion

            #region PLAYER 2
            else
            { // PLAYER 2
              //STATE-MACHINE
                if (transition)
                {
                    //passCagePos = GetSelectedCageDoorTransform(PCD.selectedDoor);
                    //if(passCagePos == null) {
                    //    HandleValues();
                    //    passCagePos = GetSelectedCageDoorTransform(PCD.selectedDoor); }
                    //transform.LookAt(passCagePos);
                    HandleValues();
                    iCurrentState = (int)PlayerStates.TRANSITION;
                    transition = false;
                }
                else
                {
                    if (Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.RUNNING)
                    {
                        iCurrentState = (int)PlayerStates.THINKING;
                    }
                    else if (!Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.SPAWNING)
                    {
                        iCurrentState = (int)PlayerStates.RUNNING;
                    }

                    if (PCD.doorPressed)
                    {
                        iCurrentState = (int)PlayerStates.UNLOCKING;
                        PCD.doorPressed = false;
                    }

                    if (PlayerOpenDoor.inFrontOfDoor)
                    {
                        iCurrentState = (int)PlayerStates.ENTERING;

                        PlayerOpenDoor.inFrontOfDoor = false;
                        PCD.doorPressed = false;
                        Stop2Collider.choosingPosition = false;
                    }

                    if (PCM.currentCorrectDoor == PCD.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && doorAnimeOpened)
                    {
                        iCurrentState = (int)PlayerStates.PASS;

                        MS.currentSectionCount++;
                        MS.currentLevel++;

                        PCD.nTry = 0;
                        if (MS.currentSectionCount == MS.GetNumberOfSections((int)PlayerCalculationManager.DIFFICULTIES.EASY))
                        {
                            iTempState = (int)TempState.NEXT_STOP_WIN_ROOM;
                            MS.spawnWinSection = true;
                            return;
                        }
                        else if (MS.currentSectionCount >= 2)
                        {
                            MS.spawnNewSection = true;
                            iTempState = (int)TempState.OTHER;
                            doorAnimeOpened = false;
                        }
                        else if (MS.currentSectionCount == 1)
                        {
                            StartCoroutine(PCM.TransitionToNextLevel());
                            iTempState = (int)TempState.OTHER;
                            doorAnimeOpened = false;
                        }
                    }
                    else if (PCM.currentCorrectDoor != PCD.selectedDoor && iCurrentState == (int)PlayerStates.ENTERING && doorAnimeOpened)
                    {
                        iCurrentState = (int)PlayerStates.REPEAT;
                        doorAnimeOpened = false;
                    }

                    if (Stop2Collider.choosingPosition && iCurrentState == (int)PlayerStates.REPEAT && transform.position == centerPlayerPos.position)
                    {
                        StartCoroutine(LookStraight());
                        iCurrentState = (int)PlayerStates.THINKING;
                    }

                }

                //STATE-HANDLERS
                if (iCurrentState == (int)PlayerStates.RUNNING)
                {
                    PLAYER_MOVEMENT(centerPlayerPos);
                }
                else if (iCurrentState == (int)PlayerStates.UNLOCKING)
                {
                    photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, false);
                    PLAY_ANIMATION_DOOR_TRANSITION();
                    GoToDoor(PCD.selectedDoor);
                }
                else if (iCurrentState == (int)PlayerStates.THINKING)
                {
                    photonView.RPC("PLAY_ANIMATION_FALL_BACK", RpcTarget.All, false);
                    photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
                    photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, true);
                }
                else if (iCurrentState == (int)PlayerStates.ENTERING)
                {
                    photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
                    //StartCoroutine(FaceOFF());
                    photonView.RPC("FACE_YOUR_DESTINY", RpcTarget.All, null);
                }
                else if (iCurrentState == (int)PlayerStates.PASS)
                {
                    Debug.Log("<color=green>PASS</color>");
                    photonView.RPC("RPC_Continue", RpcTarget.All);
                }
                else if (iCurrentState == (int)PlayerStates.REPEAT)
                {
                    Debug.Log("<color=red>REPEAT</color>");
                    StartCoroutine(WaitSec(1f));
                    photonView.RPC("PLAY_ANIMATION_FALL_BACK", RpcTarget.All, true);
                    float step = fallBackSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, centerPlayerPos.position, step);
                    StartCoroutine(CloseDoor());
                    Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
                    StartCoroutine(AgainCanChooseDoor());
                }
                else if (iCurrentState == (int)PlayerStates.TRANSITION)
                {
                    if (isWinningSection)
                    {
                        GoToEndPosition(isMasterClient);
                    }
                    else
                    {
                        transform.LookAt(centerDoorLookPos);
                        iCurrentState = (int)PlayerStates.RUNNING;
                    }

                }
                else if (iCurrentState == (int)PlayerStates.WIN)
                {
                    GoToDancePosition();
                }

            }
            #endregion
        }
        else
        {
            // waiting room

            if (canPlay)
            {
                photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, true);

                iCurrentState = (int)PlayerStates.SPAWNING;
                //sound hmm TO:DO
            }
        }
    }

    void HandleFinnish()
    {
        if (stopPlayer)
        {
            GameSetup.bPLAYER_WON = true;
            photonView.RPC("PLAYER_DEFEATED", RpcTarget.All, null);
            canPlay = false; //TO:DO-> integrate also for choosing door
            stopPlayer = false;
            Debug.Log("STOP PLAYER ONE AND FOR ALLL!");
        }
    }

    [PunRPC]
    void Movement()
    {
        transform.position = Vector3.forward * Time.deltaTime * moveSpeed;
    }

    [PunRPC]
    void PLAYER_DEFEATED()
    {
        playerAnime.SetTrigger("defeat");
    }

    void PLAYER_MOVEMENT(Transform PosToRun)
    {
        timer_runAnime += Time.deltaTime;

        if (timer_runAnime >= .20f)
        {
            photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, true);
            timer_runAnime = 0;
        }
        else
        {
            photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
        }

        photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, false);

        if (endOfGame) { GetWinPosition(isMasterClient); }
        float step = endOfGame ? 6f * Time.deltaTime : moveSpeed * Time.deltaTime;
        Transform GoToPos = endOfGame ? passWinPos : PosToRun;

        transform.position = Vector3.MoveTowards(transform.position, GoToPos.position, step);
    }

 
    //ANIMATIONS
    [PunRPC]
    private void THINK()
    {
        playerAnime.SetBool("think", true);
    }

    [PunRPC]
    private void PLAY_ANIMATION_THINK(bool thinking)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("think", thinking);
        }
    }

    [PunRPC]
    private void PLAY_ANIMATION_RUN(bool running)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("run", running);
        }
    }

    [PunRPC]
    private void PLAY_ANIMATION_FALL_BACK(bool fall)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("fall", fall);
        }
    }

    private void PLAY_ANIMATION_DOOR_TRANSITION()
    {
        if (playerAnime != null)
        {
            HandleDoorTransitions();
        }
    }

    [PunRPC]
    private void PLAY_ANIMATION_PASS(bool pass)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("pass", pass);
        }
    }

    [PunRPC]
    private void PLAY_ANIMATION_SLIDE(bool slide)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("slide", slide);
        }
    }

    // FUNCTIONS

    private IEnumerator WaitSec(float t)
    {
        yield return new WaitForSeconds(t);
    }


    private void GetOtherPositions1(bool player1)
    {
        Transform centerPlayer0_1Pos = GameObject.FindGameObjectWithTag("playerThinkPos").GetComponent<Transform>();
        Transform centerPlayer0_2Pos = GameObject.FindGameObjectWithTag("playerThinkPos2").GetComponent<Transform>();

        centerPlayerPos = player1 ? centerPlayer0_1Pos : centerPlayer0_2Pos;

        Transform centerDoorLook1_1Pos = GameObject.FindGameObjectWithTag("thinkPosLooking").GetComponent<Transform>();
        Transform centerDoorLook1_2Pos = GameObject.FindGameObjectWithTag("thinkPosLooking2").GetComponent<Transform>();

        centerDoorLookPos = player1 ? centerDoorLook1_1Pos : centerDoorLook1_2Pos;

    }

    private void GetOtherPositions2(bool player1)
    {
        Transform centerPlayer0_1Pos = GameObject.FindGameObjectWithTag("playerThinkPos1").GetComponent<Transform>();
        Transform centerPlayer0_2Pos = GameObject.FindGameObjectWithTag("2playerThinkPos1").GetComponent<Transform>();

        centerPlayerPos = player1 ? centerPlayer0_1Pos : centerPlayer0_2Pos;

        Transform centerDoorLook1_1Pos = GameObject.FindGameObjectWithTag("thinkPosLooking1").GetComponent<Transform>();
        Transform centerDoorLook1_2Pos = GameObject.FindGameObjectWithTag("2thinkPosLooking1").GetComponent<Transform>();

        centerDoorLookPos = player1 ? centerDoorLook1_1Pos : centerDoorLook1_2Pos;
    }

    private void GetDoorPositions1(bool player1)
    {
        Transform door0_1Pos = GameObject.FindGameObjectWithTag("door0Pos").GetComponent<Transform>();
        Transform door1_1Pos = GameObject.FindGameObjectWithTag("door1Pos").GetComponent<Transform>();
        Transform door2_1Pos = GameObject.FindGameObjectWithTag("door2Pos").GetComponent<Transform>();

        Transform door0_2Pos = GameObject.FindGameObjectWithTag("2door0Pos").GetComponent<Transform>();
        Transform door1_2Pos = GameObject.FindGameObjectWithTag("2door1Pos").GetComponent<Transform>();
        Transform door2_2Pos = GameObject.FindGameObjectWithTag("2door2Pos").GetComponent<Transform>();

        door0Pos = player1 ? door0_1Pos : door0_2Pos;
        door1Pos = player1 ? door1_1Pos : door1_2Pos;
        door2Pos = player1 ? door2_1Pos : door2_2Pos;
    }

    private void GetDoorPositions2(bool player1)
    {

        Transform door0_1Pos = GameObject.FindGameObjectWithTag("door0Pos1").GetComponent<Transform>();
        Transform door1_1Pos = GameObject.FindGameObjectWithTag("door1Pos1").GetComponent<Transform>();
        Transform door2_1Pos = GameObject.FindGameObjectWithTag("door2Pos1").GetComponent<Transform>();

        Transform door0_2Pos = GameObject.FindGameObjectWithTag("2door0Pos1").GetComponent<Transform>();
        Transform door1_2Pos = GameObject.FindGameObjectWithTag("2door1Pos1").GetComponent<Transform>();
        Transform door2_2Pos = GameObject.FindGameObjectWithTag("2door2Pos1").GetComponent<Transform>();

        door0Pos = player1 ? door0_1Pos : door0_2Pos;
        door1Pos = player1 ? door1_1Pos : door1_2Pos;
        door2Pos = player1 ? door2_1Pos : door2_2Pos;
    }

    private void GetCageDoorPositions1(bool player1)
    {
        Transform cage0_1Pos = GameObject.FindGameObjectWithTag("cageDoor0").GetComponent<Transform>();
        Transform cage1_1Pos = GameObject.FindGameObjectWithTag("cageDoor1").GetComponent<Transform>();
        Transform cage2_1Pos = GameObject.FindGameObjectWithTag("cageDoor2").GetComponent<Transform>();

        Transform cage0_2Pos = GameObject.FindGameObjectWithTag("2cageDoor0").GetComponent<Transform>();
        Transform cage1_2Pos = GameObject.FindGameObjectWithTag("2cageDoor1").GetComponent<Transform>();
        Transform cage2_2Pos = GameObject.FindGameObjectWithTag("2cageDoor2").GetComponent<Transform>();

        cageDoor0Pos = player1 ? cage0_1Pos : cage0_2Pos;
        cageDoor1Pos = player1 ? cage1_1Pos : cage1_2Pos;
        cageDoor2Pos = player1 ? cage2_1Pos : cage2_2Pos;
    }

    private void GetCageDoorPositions2(bool player1)
    {
        Transform cage0_1Pos = GameObject.FindGameObjectWithTag("cageDoor0_1").GetComponent<Transform>();
        Transform cage1_1Pos = GameObject.FindGameObjectWithTag("cageDoor1_1").GetComponent<Transform>();
        Transform cage2_1Pos = GameObject.FindGameObjectWithTag("cageDoor2_1").GetComponent<Transform>();

        Transform cage0_2Pos = GameObject.FindGameObjectWithTag("2cageDoor_0").GetComponent<Transform>();
        Transform cage1_2Pos = GameObject.FindGameObjectWithTag("2cageDoor_1").GetComponent<Transform>();
        Transform cage2_2Pos = GameObject.FindGameObjectWithTag("2cageDoor_2").GetComponent<Transform>();

        cageDoor0Pos = player1 ? cage0_1Pos : cage0_2Pos;
        cageDoor1Pos = player1 ? cage1_1Pos : cage1_2Pos;
        cageDoor2Pos = player1 ? cage2_1Pos : cage2_2Pos;
    }


    public void GET_CURRENT_POSITIONS(int section, bool isMaster)
    {
        switch (section)
        {
            case 1:
                GetValuesSection1(isMaster);
                break;
            case 2:
                GetValuesSection2(isMaster);
                break;
            default:
                break;
        }
    }

    public void GetValuesSection1(bool isMaster)
    {
        GetOtherPositions1(isMaster);
        GetDoorPositions1(isMaster);
        GetCageDoorPositions1(isMaster);
    }

    public void GetValuesSection2(bool isMaster)
    {
        GetOtherPositions2(isMaster);
        GetDoorPositions2(isMaster);
        GetCageDoorPositions2(isMaster);
    }


    ///////////////////////////////////////////////////////////////////////////////////////

    //////////////////////DOOR HANDLING////////////////////////////////////////////////

    //[PunRPC]
    private void HandleDoorTransitions()
    {
        if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR0 && PCD.doorChoosen)
        {
            photonView.RPC("UNLOCK_LEFT", RpcTarget.All, null);
            PCD.doorChoosen = false;
        }
        else if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR1 && PCD.doorChoosen)
        {
            photonView.RPC("UNLOCK_MIDDLE", RpcTarget.All, null);
            PCD.doorChoosen = false;
        }
        else if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR2 && PCD.doorChoosen)
        {
            photonView.RPC("UNLOCK_RIGHT", RpcTarget.All, null);
            PCD.doorChoosen = false;
        }
    }

    [PunRPC]
    public void UNLOCK_LEFT()
    {
        playerAnime.SetTrigger("unlockingL");
    }

    [PunRPC]
    public void UNLOCK_MIDDLE()
    {
        playerAnime.SetTrigger("unlocking");
    }

    [PunRPC]
    public void UNLOCK_RIGHT()
    {
        playerAnime.SetTrigger("unlockingR");
    }

    private void GoToDoor(int door)
    {
        switch (door)
        {
            case 0:
                LeftDoor();
                break;
            case 1:
                MiddleDoor();
                break;
            case 2:
                RightDoor();
                break;
        }
    }

    private void LeftDoor()
    {
        Transform doorPosition = null;

        string doorName = door0Pos.name;
        doorPosition = doorName.Contains("0") ? door0Pos : null;
        if (doorPosition == null) { Debug.LogError("door position not valid!"); }
        // LOOK AND MOVE TO LEFT DOOR
        transform.LookAt(doorPosition);
        float step = transitionSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, doorPosition.position, step);
    }

    private void RightDoor()
    {
        Transform doorPosition = null;

        string doorName = door2Pos.name;
        doorPosition = doorName.Contains("2") ? door2Pos : null;
        if (doorPosition == null) { Debug.LogError("door position not valid!"); }
        // LOOK AND MOVE TO RIGHT DOOR
        transform.LookAt(doorPosition);
        float step = transitionSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, doorPosition.position, step);
    }

    private void MiddleDoor()
    {
        Transform doorPosition = null;

        string doorName = door1Pos.name;
        doorPosition = doorName.Contains("1") ? door1Pos : null;
        if (doorPosition == null) { Debug.LogError("door position not valid!"); }
        // LOOK AND MOVE TO RIGHT DOOR
        transform.LookAt(doorPosition);
        float step = transitionSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, doorPosition.position, step);
    }

    ///////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////FACE OFF////////////////////////////////////////////////////////

    [PunRPC]
    public void FACE_YOUR_DESTINY()
    {
        StartCoroutine(FaceOFF());
    }

    private IEnumerator FaceOFF()
    {
        //photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, true);
        PLAY_ANIMATION_THINK(true);
        yield return new WaitForSeconds(1f);
        doorAnimeOpened = true;
        PLAY_ANIMATION_THINK(false);

        //Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
        //Debug.Log("<color=blue>Door anime:</color> " + doorAnime.gameObject.name);

        //photonView.RPC("OPEN_DOOR", RpcTarget.All, doorAnime);
        //OPEN_DOOR(doorAnime);

        //photonView.RPC("PLAY_ANIMATION_THINK", RpcTarget.All, false);
    }

    [PunRPC]
    public void OPEN_DOOR(Animator doorAnime)
    {
        if (doorAnime != null)
        {
            doorAnime.SetBool("closeDoor0" + PCD.selectedDoor, false);
            doorAnime.SetBool("openDoor0" + PCD.selectedDoor, true);
            //open door sound
        }
    }

    [PunRPC]
    public void CLOSE_DOOR(Animator doorAnime)
    {
        if (doorAnime != null)
        {
            doorAnime.SetBool("closeDoor0" + PCD.selectedDoor, false);
            doorAnime.SetBool("openDoor0" + PCD.selectedDoor, true);
            //open door sound
        }
    }

    private Animator GetSelectedDoorAnime(int selectedDoor)
    {
        Animator anime = null;

        switch (selectedDoor)
        {
            case (int)PlayerChooseDoor.Doors.DOOR0:
                anime = door0Pos.GetComponentInParent<Animator>();
                photonView.RPC("CLOSING_DOOR_0", RpcTarget.All, null);
                break;
            case (int)PlayerChooseDoor.Doors.DOOR1:
                anime = door1Pos.GetComponentInParent<Animator>();
                photonView.RPC("CLOSING_DOOR_1", RpcTarget.All, null);
                break;
            case (int)PlayerChooseDoor.Doors.DOOR2:
                anime = door2Pos.GetComponentInParent<Animator>();
                photonView.RPC("CLOSING_DOOR_2", RpcTarget.All, null);
                break;
            default:
                break;
        }

        return anime;
    }



    [PunRPC]
    public void CLOSING_DOOR_0()
    {
        if (door0Pos.GetComponentInParent<Animator>() != null)
        {
            door0Pos.GetComponentInParent<Animator>().SetBool("openDoor0" + PCD.selectedDoor, false);
            door0Pos.GetComponentInParent<Animator>().SetBool("closeDoor0" + PCD.selectedDoor, true);
            //SFXscript.sfxScript.PLAY_DOOR_CLOSE_SOUND();
        }
    }

    [PunRPC]
    public void CLOSING_DOOR_1()
    {
        if (door1Pos.GetComponentInParent<Animator>() != null)
        {
            door1Pos.GetComponentInParent<Animator>().SetBool("openDoor0" + PCD.selectedDoor, false);
            door1Pos.GetComponentInParent<Animator>().SetBool("closeDoor0" + PCD.selectedDoor, true);
            //SFXscript.sfxScript.PLAY_DOOR_CLOSE_SOUND();
        }
    }

    [PunRPC]
    public void CLOSING_DOOR_2()
    {
        if (door2Pos.GetComponentInParent<Animator>() != null)
        {
            door2Pos.GetComponentInParent<Animator>().SetBool("openDoor0" + PCD.selectedDoor, false);
            door2Pos.GetComponentInParent<Animator>().SetBool("closeDoor0" + PCD.selectedDoor, true);
            //SFXscript.sfxScript.PLAY_DOOR_CLOSE_SOUND();
        }
    }
    //////////////////////////////////////////////////////////////////////////////////

    public void HandleValues()
    {
        if (MS.currentSectionCount % 2 == 0 && MS.currentSectionCount != 0)
        {
            //GET FIRST SECTION DATA
            GET_CURRENT_POSITIONS(1, isMasterClient);
            PCM.GetValues(1);
            //cage.GetValues(1);
        }
        else
        {
            //GET SECOND SECTION DATA
            GET_CURRENT_POSITIONS(2, isMasterClient);
            PCM.GetValues(2);
            //cage.GetValues(2);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////

    //public IEnumerator TransitionToNextLevel()
    //{
    //    yield return new WaitForSeconds(1f);
    //    HandleValues();
    //    //PCM.RPC_CreateEquation();
    //    //CageScript.enemiesSpawned = false;
    //}

    IEnumerator LookStraight()
    {
        yield return new WaitForSeconds(2.6f);
        transform.LookAt(centerDoorLookPos);
    }

    [PunRPC]
    public void RPC_Continue()
    {
        StartCoroutine(Continue());
    }

    IEnumerator Continue()
    {
        PLAY_ANIMATION_THINK(false);
        PLAY_ANIMATION_PASS(true);
        yield return new WaitForSeconds(1.5f);
        PLAY_ANIMATION_PASS(false);
        canChooseDoor = true;
        transition = true;
    }

    [PunRPC]
    public void RPC_CloseDoor()
    {
        StartCoroutine(CloseDoor());
        Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
        if (doorAnime != null)
        {
            doorAnime.SetBool("openDoor0" + PCD.selectedDoor, false);
            doorAnime.SetBool("closeDoor0" + PCD.selectedDoor, true);
            //SFXscript.sfxScript.PLAY_DOOR_CLOSE_SOUND();
        }
        StartCoroutine(AgainCanChooseDoor());

        photonView.RPC("CLOSE_DOOR", RpcTarget.All, doorAnime);

    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2f);
        //close door
      
        //MeshRenderer DOOR_FRAME = doorToColor(PCD.selectedDoor);
        //DOOR_FRAME.material = defaultMat;
    }

    IEnumerator AgainCanChooseDoor()
    {
        yield return new WaitForSeconds(2.5f);
        canChooseDoor = true;
    }

    private Transform GetSelectedCageDoorTransform(int selectedDoor)
    {
        Transform cageDoorPos;

        switch (selectedDoor)
        {
            case (int)PlayerChooseDoor.Doors.DOOR0:
                cageDoorPos = cageDoor0Pos;
                break;
            case (int)PlayerChooseDoor.Doors.DOOR1:
                cageDoorPos = cageDoor1Pos;
                break;
            case (int)PlayerChooseDoor.Doors.DOOR2:
                cageDoorPos = cageDoor2Pos;
                break;

            default:
                cageDoorPos = cageDoor0Pos;
                Debug.LogError("Failed to request door.. door 0 choosen");
                break;
        }
        return cageDoorPos;
    }

    private void MoveOn()
    {
        timer_runAnime += Time.deltaTime;

        if (timer_runAnime >= .20f)
        {
            PLAY_ANIMATION_RUN(true);

            timer_runAnime = 0;
        }
        else { PLAY_ANIMATION_RUN(false); }
    }

    private void Slide()
    {
        StartCoroutine(SlideMotion());
    }

    private IEnumerator SlideMotion()
    {
        PLAY_ANIMATION_SLIDE(true);
        //SFXscript.sfxScript.PLAY_SLIDE_SOUND();
        yield return new WaitForSeconds(1.15f);
        PLAY_ANIMATION_SLIDE(false);

        if (isWinningSection)
        {
            iCurrentState = (int)PlayerStates.WIN;
        }
        else
        {
            transform.LookAt(centerDoorLookPos);
            if (isMasterClient)
            {
                door.slide = false;
            }
            else
            {
                door2.slide = false;
            }
            iCurrentState = (int)PlayerStates.RUNNING;
        }
    }

    private bool jumpOnce;

    private void GoToDancePosition()
    {
        GetWinPosition(isMasterClient);
        GetWinLookAtTarget(isMasterClient);
        GetDancePosition(isMasterClient);

        float step = 7.2f * Time.deltaTime;

        photonView.RPC("PLAY_ANIMATION_RUN", RpcTarget.All, false);
        //WE HAVE A WINNER
        if (jumpOnce) {
            photonView.RPC("RPC_WIN_ANIME", RpcTarget.All, null);
            jumpOnce = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, dancePos.position, step);
        transform.LookAt(passWinLookAt);

        if (transform.position == dancePos.position)
        {
            PDM.PLAY_DAMCE_MOVE(PDM.iCurrentDanceMove);
        }
    }

    //public bool P1WonStopMovingP2;
    //public bool P2WonStopMovingP1;

    public bool PLAYER_HAS_WON_THE_GAME;

    [PunRPC]
    public void ShowWinGUI()
    {
        GameSetup.bPLAYER_WON = true;
    }

    private void GoToEndPosition(bool isMaster)
    {
        GetWinPosition(isMasterClient);
        GetWinLookAtTarget(isMasterClient);
        GetDancePosition(isMasterClient);

        if (PLAYER_HAS_WON_THE_GAME)
        {
            photonView.RPC("ShowWinGUI", RpcTarget.All);
            CF.PLAYER_WON = true;
            iCurrentState = (int)PlayerStates.WIN;
            return;
        }

        transform.LookAt(passWinLookAt);
        PLAYER_MOVEMENT(passWinPos);

        #region SOURCE CODE
        //if (isMaster)
        //{
        //    if (PlayerWinCollider.P1_WON)
        //    {

        //        iCurrentState = (int)PlayerStates.WIN;
        //        P1WonStopMovingP2 = true;
        //        CF.PLAYER_WON = true;
        //        return;

        //    }
        //}
        //else
        //{
        //    if (PlayerWinCollider.P2_WON)
        //    {
        //        GameSetup.bPLAYER_WON = true;


        //        iCurrentState = (int)PlayerStates.WIN;
        //        CF.PLAYER_WON = true;
        //        P2WonStopMovingP1 = true;

        //        return;

        //    }
        //}
        #endregion
    }

    [PunRPC]
    public void RPC_WIN_ANIME()
    {
        playerAnime.SetTrigger("win");
    }

    private void GetWinPosition(bool player1)
    {
        Transform pass1 = GameObject.FindGameObjectWithTag("winningPos").GetComponent<Transform>();
        Transform pass2 = GameObject.FindGameObjectWithTag("winningPos").GetComponent<Transform>();

        passWinPos = player1 ? pass1 : pass2;
    }

    private void GetWinLookAtTarget(bool player1)
    {
        Transform passWinLookAt1 = GameObject.FindGameObjectWithTag("lookAtTheEnd").GetComponent<Transform>();
        Transform passWinLookAt2 = GameObject.FindGameObjectWithTag("lookAtTheEnd").GetComponent<Transform>();

        passWinLookAt = player1 ? passWinLookAt1 : passWinLookAt2;
    }

    private void GetDancePosition(bool player1)
    {
        Transform winDancePos1 = GameObject.FindGameObjectWithTag("dancePos").GetComponent<Transform>();
        Transform winDancePos2 = GameObject.FindGameObjectWithTag("2dancePos").GetComponent<Transform>();
        dancePos = player1 ? winDancePos1 : winDancePos2;
    }
}
