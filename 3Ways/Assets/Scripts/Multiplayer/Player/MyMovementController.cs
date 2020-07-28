using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMovementController : MonoBehaviour
{
    public bool canChooseDoor;
    public bool doorAnimeOpened;
    public bool isWinningSection;
    public bool transition;

    private PhotonView PV;


    [Header("Player Movement")]
    [Space(10)]
    [SerializeField]
    private float moveSpeed = 10f;
    [Space(5)]
    [SerializeField]
    private float transitionSpeed = 5.0f;
    [Space(5)]
    [SerializeField]
    private float fallBackSpeed = 2.0f;
    [Space(5)]
    [SerializeField]
    private float passSpeed = .8f;
    [Space(10)]
    [Header("Waiting Room")]
    [Space(10)]
    [SerializeField]
    private float timeToWait = 8f;
    [Space(5)]
    public bool bStartCountDown;
    
    private Animator playerAnime;
    private Camera playerCam;

    private PlayerStateMachine PSM;
    private PlayerChooseDoor PCD;
    private MapSpawner MS;
    private PlayerCalculationManager PCM;
    private PlayerDanceMoves PDM;

    protected float timer_runAnime;
    protected bool endOfGame = false;

    public bool isPlayer1;

    /// 
    ///             TRANSFORMS
    /// 

    private Transform door0Pos;
    private Transform door1Pos;
    private Transform door2Pos;

    public Transform centerPlayerPos;
    public Transform centerDoorLookPos;

    // PASS POSITION
    public Transform passCagePos;
    private Transform passWinPos;
    // WIN LOOK AT TARGET
    private Transform passWinLookAt;
    // WIN DANCE POSITION
    private Transform dancePos;

    public Transform cageDoor0Pos;
    public Transform cageDoor1Pos;
    public Transform cageDoor2Pos;

    private Transform lookAtTheEnd;

    public MeshRenderer door0_frame;
    public MeshRenderer door1_frame;
    public MeshRenderer door2_frame;

    [Space(10)]
    [Header("Frame Color Material")]
    [Space(5)]
    public Material redFrameMat;
    [Space(5)]
    public Material greenFrameMat;
    [Space(5)]
    public Material defaultMat;

    void Start()
    {

        PV = GetComponent<PhotonView>();
        playerAnime = GetComponentInChildren<Animator>();
        playerCam = GetComponentInChildren<Camera>();


        IS_PLAYER1();

        //PSM = GetComponent<PlayerStateMachine>();
        //PCD = GetComponent<PlayerChooseDoor>();
        //MS = GetComponent<MapSpawner>();
        //PCM = GetComponent<PlayerCalculationManager>();
        //PDM = GetComponent<PlayerDanceMoves>();

        canChooseDoor = false;
        doorAnimeOpened = false;

        //GetValues(1);

        //StartCoroutine(WAITING_ROOM(timeToWait));
    }

    void Update()
    {
        if (PV.IsMine)
        {
            transform.position = Vector3.forward * transitionSpeed * Time.deltaTime;

            timer_runAnime += Time.deltaTime;

            if (timer_runAnime >= .19f)
            {
                PLAY_ANIMATION_RUN(true);
                timer_runAnime = 0;
            }
            else { PLAY_ANIMATION_RUN(false); }
            // handle player movement

            //if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.SPAWNING)
            //{
            //    // thinking position (WAITING)
            //    PLAY_ANIMATION_THINK(true);
            //    //TO:DO -> GAME TIMER (timeHasStarted = false;)
            //}
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.RUNNING)
            //{
            //    timer_runAnime += Time.deltaTime;

            //    if (timer_runAnime >= .19f)
            //    {
            //        PLAY_ANIMATION_RUN(true);
            //        timer_runAnime = 0;
            //    }
            //    else { PLAY_ANIMATION_RUN(false); }

            //    PLAY_ANIMATION_THINK(false);
            //    if (endOfGame) { GetWinPosition(true); }

            //    float step = endOfGame ? 6f * Time.deltaTime : moveSpeed * Time.deltaTime;
            //    Transform goToPos = endOfGame ? passWinPos : centerPlayerPos;

            //    transform.position = Vector3.MoveTowards(transform.position, goToPos.position, step);

            //}
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.UNLOCKING)
            //{
            //    //Debug.Log("<color=green>UNLOCKING</color>");

            //    PLAY_ANIMATION_THINK(false);

            //    PLAY_ANIMATION_DOOR_TRANSITION();

            //    //Debug.Log(ChooseDoor.selectedDoor);
            //    GoToDoor(PCD.selectedDoor);
            //}
            //// THINKING
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.THINKING)
            //{
            //    //Debug.Log("<color=green>THINK</color>");
            //    PLAY_ANIMATION_FALL_BACK(false);
            //    PLAY_ANIMATION_RUN(false);
            //    PLAY_ANIMATION_THINK(true);
            //}

            //// ENTERING
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.ENTERING)
            //{
            //    //Debug.Log("<color=green>ENTERING</color>");
            //    StartCoroutine(FaceOFF());
            //}
            //// PASS LEVEL
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.PASS)
            //{
            //    //Debug.Log("<color=blue>PASS</color>");
            //    StartCoroutine(Continue());
            //}
            //// REPEAT
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.REPEAT)
            //{
            //    //Debug.Log("<color=red>REPEAT</color>");
            //    //FACE MONSTER
            //    StartCoroutine(WaitSec(1f));
            //    PLAY_ANIMATION_FALL_BACK(true);
            //    float step = fallBackSpeed * Time.deltaTime;
            //    transform.position = Vector3.MoveTowards(transform.position, centerPlayerPos.position, step);
            //    StartCoroutine(CloseDoor());
            //}
            //// TRANSITION
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.TRANSITION)
            //{
            //    if (isPlayer1)
            //    {
            //        if (!door.slide)
            //        {
            //            MoveOn();
            //        }
            //        else
            //        { // SLIDE
            //            PLAY_ANIMATION_RUN(false);
            //            Slide();
            //        }
            //        float step = passSpeed * Time.deltaTime;

            //        transform.position = Vector3.MoveTowards(transform.position, passCagePos.position, step);
            //    }
            //    else
            //    {

            //        if (!door2.slide)
            //        {
            //            MoveOn();
            //        }
            //        else
            //        { // SLIDE
            //            PLAY_ANIMATION_RUN(false);
            //            Slide();
            //        }
            //        float step = passSpeed * Time.deltaTime;

            //        transform.position = Vector3.MoveTowards(transform.position, passCagePos.position, step);
            //    }

            //}
            //// WIN
            //else if (PSM.iCurrentState == (int)PlayerStateMachine.PlayerStates.WIN)
            //{
            //    GoToEndPosition();
            //}

        }
    }

    private void GoToEndPosition()
    {

        GetWinPosition(isPlayer1);
        GetWinLookAtTarget(isPlayer1);
        GetDancePosition(isPlayer1);

        float step = 5 * Time.deltaTime;

        //JUMP
        transform.LookAt(passWinLookAt);

        //WE HAVE A WINNER
        playerAnime.SetTrigger("win");

        transform.position = Vector3.MoveTowards(transform.position, dancePos.position, step);

        if (transform.position == dancePos.position)
        {
            Debug.Log("Dance !!");
            PDM.PlayDanceAnime(PDM.iCurrentDanceMove, playerAnime);
        }
    }

    private void IS_PLAYER1()
    {
        isPlayer1 = PhotonNetwork.IsMasterClient;
    }

    private IEnumerator WAITING_ROOM(float iWaitT)
    {
        bStartCountDown = true;
        yield return new WaitForSeconds(iWaitT);
        Debug.Log("Lets Run!");
        //PSM.bIsWaiting = false;
    }

    private IEnumerator FaceOFF()
    {
        PLAY_ANIMATION_THINK(true);
        Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
        if (doorAnime != null)
        {
            doorAnime.SetBool("closeDoor0" + PCD.selectedDoor, false);
            doorAnime.SetBool("openDoor0" + PCD.selectedDoor, true);
        }

        yield return new WaitForSeconds(1f);

        doorAnimeOpened = true;
        PLAY_ANIMATION_THINK(false);

        //color door frame
        MeshRenderer DOOR_FRAME = doorToColor(PCD.selectedDoor);
        Material redOrGreen = (PCD.selectedDoor == PCM.currentCorrectDoor) ? greenFrameMat : redFrameMat;
        DOOR_FRAME.material = redOrGreen;
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

    private IEnumerator WaitSec(float t)
    {
        yield return new WaitForSeconds(t);
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2f);
        //close door
        Animator doorAnime = GetSelectedDoorAnime(PCD.selectedDoor);
        if (doorAnime != null)
        {
            doorAnime.SetBool("openDoor0" + PCD.selectedDoor, false);
            doorAnime.SetBool("closeDoor0" + PCD.selectedDoor, true);
        }

        MeshRenderer DOOR_FRAME = doorToColor(PCD.selectedDoor);
        DOOR_FRAME.material = defaultMat;

        yield return new WaitForSeconds(2.5f);
        canChooseDoor = true;
    }

    private void MoveOn()
    {
        timer_runAnime += Time.deltaTime;

        if (timer_runAnime >= .19f)
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
        yield return new WaitForSeconds(1.15f);
        PLAY_ANIMATION_SLIDE(false);

        if (isWinningSection)
        {
            PSM.iCurrentState = (int)PlayerStateMachine.PlayerStates.WIN;
        }
        else
        {
            transform.LookAt(centerDoorLookPos);
            if (isPlayer1) { door.slide = false; }
            else { door2.slide = false; }
            
            PSM.iCurrentState = (int)PlayerStateMachine.PlayerStates.RUNNING;
        }
    }

    ////////////ANIMATIONS_FUNCTIONS///////////////
    private void PLAY_ANIMATION_THINK(bool thinking)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("think", thinking);
        }
    }

    private void PLAY_ANIMATION_RUN(bool running)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("run", running);
        }
    }

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

    private void PLAY_ANIMATION_PASS(bool pass)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("pass", pass);
        }
    }

    private void PLAY_ANIMATION_SLIDE(bool slide)
    {
        if (playerAnime != null)
        {
            playerAnime.SetBool("slide", slide);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // WIN SECTION
    private void GetWinPosition(bool player1)
    {
        Transform pass1 = GameObject.FindGameObjectWithTag("winningPos").GetComponent<Transform>();
        Transform pass2 = GameObject.FindGameObjectWithTag("2winningPos").GetComponent<Transform>();
        
        passWinPos = player1 ? pass1 : pass2;
    }

    private void GetWinLookAtTarget(bool player1)
    {
        Transform passWinLookAt1 = GameObject.FindGameObjectWithTag("lookAtTheEnd").GetComponent<Transform>();
        Transform passWinLookAt2 = GameObject.FindGameObjectWithTag("2lookAtTheEnd").GetComponent<Transform>();

        passWinLookAt = player1 ? passWinLookAt1 : passWinLookAt2; 
    }

    private void GetDancePosition(bool player1)
    {
        Transform winDancePos1 = GameObject.FindGameObjectWithTag("dancePos").GetComponent<Transform>();
        Transform winDancePos2 = GameObject.FindGameObjectWithTag("2dancePos").GetComponent<Transform>();
        dancePos = player1 ? winDancePos1 : winDancePos2;
    }

    // SECTION 1 
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

    private void GetOtherPositions1(bool player1)
    {
        Transform centerPlayer0_1Pos = GameObject.FindGameObjectWithTag("playerThinkPos").GetComponent<Transform>();       
        Transform centerDoorLook1_1Pos = GameObject.FindGameObjectWithTag("thinkPosLooking").GetComponent<Transform>();

        Transform centerPlayer0_2Pos = GameObject.FindGameObjectWithTag("playerThinkPos2").GetComponent<Transform>();
        Transform centerDoorLook1_2Pos = GameObject.FindGameObjectWithTag("thinkPosLooking2").GetComponent<Transform>();

        centerPlayerPos = player1 ? centerPlayer0_1Pos : centerPlayer0_2Pos;
        centerDoorLookPos = player1 ? centerDoorLook1_1Pos : centerDoorLook1_2Pos;
    }

    private void GetDoorFramesMesh1(bool player1)
    {
        MeshRenderer frame0_1Mesh = GameObject.FindGameObjectWithTag("0frame").GetComponent<MeshRenderer>();
        MeshRenderer frame1_1Mesh = GameObject.FindGameObjectWithTag("1frame").GetComponent<MeshRenderer>();
        MeshRenderer frame2_1Mesh = GameObject.FindGameObjectWithTag("2frame").GetComponent<MeshRenderer>();

        MeshRenderer frame0_2Mesh = GameObject.FindGameObjectWithTag("2_0frame").GetComponent<MeshRenderer>();
        MeshRenderer frame1_2Mesh = GameObject.FindGameObjectWithTag("2_1frame").GetComponent<MeshRenderer>();
        MeshRenderer frame2_2Mesh = GameObject.FindGameObjectWithTag("2_2frame").GetComponent<MeshRenderer>();

        door0_frame = player1 ? frame0_1Mesh : frame0_2Mesh;
        door1_frame = player1 ? frame1_1Mesh : frame1_2Mesh;
        door2_frame = player1 ? frame2_1Mesh : frame2_2Mesh;

    }
    ///////////////////////////////////////////////////////////////////////////////////////
    // SECTION 2

        
        
        
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
    
    private void GetOtherPositions2(bool player1)
    {
        Transform centerPlayer0_1Pos = GameObject.FindGameObjectWithTag("playerThinkPos1").GetComponent<Transform>();
        Transform centerDoorLook1_1Pos = GameObject.FindGameObjectWithTag("thinkPosLooking1").GetComponent<Transform>();

        Transform centerPlayer0_2Pos = GameObject.FindGameObjectWithTag("2playerThinkPos1").GetComponent<Transform>();
        Transform centerDoorLook1_2Pos = GameObject.FindGameObjectWithTag("2thinkPosLooking1").GetComponent<Transform>();

        centerPlayerPos = player1 ? centerPlayer0_1Pos : centerPlayer0_2Pos;
        centerDoorLookPos = player1 ? centerDoorLook1_1Pos : centerDoorLook1_2Pos;
    }

    private void GetDoorFramesMesh2(bool player1)
    {

        MeshRenderer frame0_1Mesh = GameObject.FindGameObjectWithTag("0frame1").GetComponent<MeshRenderer>();
        MeshRenderer frame1_1Mesh = GameObject.FindGameObjectWithTag("1frame1").GetComponent<MeshRenderer>();
        MeshRenderer frame2_1Mesh = GameObject.FindGameObjectWithTag("2frame1").GetComponent<MeshRenderer>();

        MeshRenderer frame0_2Mesh = GameObject.FindGameObjectWithTag("2_0frame1").GetComponent<MeshRenderer>();
        MeshRenderer frame1_2Mesh = GameObject.FindGameObjectWithTag("2_1frame1").GetComponent<MeshRenderer>();
        MeshRenderer frame2_2Mesh = GameObject.FindGameObjectWithTag("2_2frame1").GetComponent<MeshRenderer>();

        door0_frame = player1 ? frame0_1Mesh : frame0_2Mesh;
        door1_frame = player1 ? frame1_1Mesh : frame1_2Mesh;
        door2_frame = player1 ? frame2_1Mesh : frame2_2Mesh;

    }
    
    ///////////////////////////////////////////////////////////////////////////////////////
    //public void HandleValues()
    //{
    //    if (MS.currentSectionCount % 2 == 0 && MS.currentSectionCount != 0)
    //    {
    //        //GET FIRST SECTION DATA
    //        GetValues(1);
    //        //calcucaltions.GetValues(1);
    //        //cage.GetValues(1);
    //    }
    //    else
    //    {
    //        //GET SECOND SECTION DATA
    //        GetValues(2);
    //        //calcucaltions.GetValues(2);
    //        //cage.GetValues(2);
    //    }
    //}

    public void GetValues(int section)
    {
        switch (section)
        {
            case 1:
                GetValuesSection1(isPlayer1);
                break;
            case 2:
                GetValuesSection2(isPlayer1);
                break;
            default:
                break;
        }
    }

    private void GetValuesSection1(bool bPlayer1)
    {
        GetDoorPositions1(bPlayer1);
        GetCageDoorPositions1(bPlayer1);
        GetOtherPositions1(bPlayer1);
        GetDoorFramesMesh1(bPlayer1);
    }

    private void GetValuesSection2(bool bPlayer1)
    {
        GetDoorPositions2(bPlayer1);
        GetCageDoorPositions2(bPlayer1);
        GetOtherPositions2(bPlayer1);
        GetDoorFramesMesh2(bPlayer1);
    }



    private void HandleDoorTransitions()
    {
        if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR0 && PCD.doorChoosen)
        {
            playerAnime.SetTrigger("unlockingL");
            PCD.doorChoosen = false;
        }
        else if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR1 && PCD.doorChoosen)
        {
            playerAnime.SetTrigger("unlocking");
            PCD.doorChoosen = false;
        }
        else if (PCD.selectedDoor == (int)PlayerChooseDoor.Doors.DOOR2 && PCD.doorChoosen)
        {
            playerAnime.SetTrigger("unlockingR");
            PCD.doorChoosen = false;
        }
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


    private Animator GetSelectedDoorAnime(int selectedDoor)
    {
        Animator anime = null;

        switch (selectedDoor)
        {
            case (int)PlayerChooseDoor.Doors.DOOR0:
                anime = door0Pos.GetComponentInParent<Animator>();
                break;
            case (int)PlayerChooseDoor.Doors.DOOR1:
                anime = door1Pos.GetComponentInParent<Animator>();
                break;
            case (int)PlayerChooseDoor.Doors.DOOR2:
                anime = door2Pos.GetComponentInParent<Animator>();
                break;
            default:
                break;
        }

        return anime;
    }

    private MeshRenderer doorToColor(int selectedDoor)
    {
        MeshRenderer frame = null;

        switch (selectedDoor)
        {
            case 0:
                frame = door0_frame;
                break;

            case 1:
                frame = door1_frame;
                break;

            case 2:
                frame = door2_frame;
                break;

            default:
                break;
        }


        return frame;
    }
}
