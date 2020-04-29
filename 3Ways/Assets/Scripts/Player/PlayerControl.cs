using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static bool doorAnimeOpened;
    public static bool passLevel;
    public static bool canChooseDoor;
    public static bool transition;
    public static bool changeData;
    public static bool isWinningSection;
    public static bool bStartCountDown;

    protected bool endOfGame = false;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float transitionSpeed = 5.0f;
    [SerializeField]
    private float fallBackSpeed = 2.0f;
    [SerializeField]
    private float passSpeed = .8f;

    private Animator anime;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    protected float timer_runAnime;

    // DOOR POSITIONS
    //public Transform[] doorPositions;
    private Transform door0Pos;
    private Transform door1Pos;
    private Transform door2Pos;
    
    public Transform centerPlayerPos;
    public Transform centerDoorLookPos;

    // PASS POSITION
    private Transform passCagePos;
    private Transform passWinPos;
    // WIN LOOK AT TARGET
    private Transform passWinLookAt;
    // WIN DANCE POSITION
    private Transform dancePos;

    //public Transform[] passCagePositions;
    private Transform cageDoor0Pos;
    private Transform cageDoor1Pos;
    private Transform cageDoor2Pos;

    private Transform lookAtTheEnd;

    private CalculationManager calcucaltions;
    private CageScript cage;

    public int slideOnce = 0;

    private MeshRenderer door0_frame;
    private MeshRenderer door1_frame;
    private MeshRenderer door2_frame;

    public Material redFrameMat;
    public Material greenFrameMat;
    public Material defaultMat;

    void Awake()
    {
        GetValues(1);
    }
    
    void Start()
    {
        anime           = GetComponentInChildren<Animator>();
        controller      = GetComponent<CharacterController>();
        calcucaltions   = GetComponent<CalculationManager>();
        cage            = GetComponent<CageScript>();

        doorAnimeOpened = false;
        passLevel       = false;
        canChooseDoor   = true;
        changeData      = false;

        StartCoroutine(WAIT_ROOM());
    }

    void FixedUpdate()
    {
        // SPAWN PLAYER TO WAITING ROOM
        if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.SPAWNING)
        {
            //Debug.Log("<color=green>SPAWNING</color>");
            PLAY_ANIMATION_THINK(true); // TO:DO -> wait animation
            GameTimer.timeHasStarted = false;
        }
        // RUNNING
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.RUNNING)
        {
            GameTimer.timeHasStarted = true;

            timer_runAnime += Time.deltaTime;

            if (timer_runAnime >= .19f)
            {
                PLAY_ANIMATION_RUN(true);
                timer_runAnime = 0;
            }
            else { PLAY_ANIMATION_RUN(false); }

            PLAY_ANIMATION_THINK(false);
            if (endOfGame) { GetWinPosition(); }
            float       step    = endOfGame ? 6f * Time.deltaTime   : moveSpeed * Time.deltaTime;
            Transform   GoToPos = endOfGame ? passWinPos            : centerPlayerPos;
            transform.position = Vector3.MoveTowards(transform.position, GoToPos.position, step);
            //controller.Move(Vector3.forward * moveSpeed * Time.deltaTime);
            Debug.Log("<color=green>RUNNING</color>");
        }
        // UNLOCKING
        else if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.UNLOCKING)
        {
            //Debug.Log("<color=green>UNLOCKING</color>");

            PLAY_ANIMATION_THINK(false);

            PLAY_ANIMATION_DOOR_TRANSITION();

            //Debug.Log(ChooseDoor.selectedDoor);
            GoToDoor(ChooseDoor.selectedDoor);
        }
        // THINKING
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.THINKING)
        {
            //Debug.Log("<color=green>THINK</color>");
            PLAY_ANIMATION_FALL_BACK(false);
            PLAY_ANIMATION_RUN(false);
            PLAY_ANIMATION_THINK(true);

            //canChooseDoor = true;
        }
        // ENTERING
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.ENTERING)
        {
            //Debug.Log("<color=green>ENTERING</color>");
            StartCoroutine(FaceOFF());
        }
        // PASS LEVEL
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.PASS)
        {
            Debug.Log("<color=blue>PASS</color>");
            StartCoroutine(Continue());
        }
        // REPEAT
        else if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.REPEAT)
        {
            //Debug.Log("<color=red>REPEAT</color>");
            //FACE MONSTER
            StartCoroutine(WaitSec(1f));
            PLAY_ANIMATION_FALL_BACK(true);
            float step = fallBackSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, centerPlayerPos.position, step);
            StartCoroutine(CloseDoor());
        }
        // TRANSITION
        else if (StateMachine.iCurrentState == (int)StateMachine.PlayerStates.TRANSITION)
        {
            if (!door.slide)
            {
                MoveOn();
            }
            else
            { // SLIDE
                PLAY_ANIMATION_RUN(false);
                Slide();
            }
            float step = passSpeed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, passCagePos.position, step);      
        }
        // WIN
        else if(StateMachine.iCurrentState == (int)StateMachine.PlayerStates.WIN)
        {
            Debug.Log("WININI PANINI");
            GoToEndPosition();
            //GetWinPosition();
            //float step = fallBackSpeed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, passWinPos.position, step);
        }
    }

    private IEnumerator WaitSec(float t)
    {
        yield return new WaitForSeconds(t);
    }

    private void PLAY_ANIMATION_RUN(bool running)
    {
        if(anime != null)
        {
            anime.SetBool("run", running);
        }   
    }

    private void PLAY_ANIMATION_THINK(bool thinking)
    {
        if (anime != null)
        {
            //anime.SetBool("run", false);
            anime.SetBool("think", thinking);
        }
    }

    private void PLAY_ANIMATION_DOOR_TRANSITION()
    {
        if(anime != null)
        {
            HandleDoorTransitions();
        }
    }

    private void PLAY_ANIMATION_FALL_BACK(bool fall)
    {
        if(anime != null)
        {
            anime.SetBool("fall", fall);
        }
    }

    private void PLAY_ANIMATION_PASS(bool pass)
    {
        if (anime != null)
        {
            anime.SetBool("pass", pass);
        }
    }

    private void PLAY_ANIMATION_SLIDE(bool slide)
    {
        if(anime != null)
        {
            anime.SetBool("slide", slide);
        }
    }

    private void HandleDoorTransitions()
    {
        if (ChooseDoor.selectedDoor == (int)ChooseDoor.Doors.DOOR0 && ChooseDoor.doorChoosen)
        {

            anime.SetTrigger("unlockingL");
            ChooseDoor.doorChoosen = false;
        }
        else if (ChooseDoor.selectedDoor == (int)ChooseDoor.Doors.DOOR1 && ChooseDoor.doorChoosen)
        {
            anime.SetTrigger("unlocking");
            ChooseDoor.doorChoosen = false;
        }
        else if (ChooseDoor.selectedDoor == (int)ChooseDoor.Doors.DOOR2 && ChooseDoor.doorChoosen)
        {
            anime.SetTrigger("unlockingR");
            ChooseDoor.doorChoosen = false;
        }
    }

    private IEnumerator WAIT_ROOM()
    {
        bStartCountDown = true;
        yield return new WaitForSeconds(5f);
        // start running after player 2 connects -> TODO
        StateMachine.bIsWaiting = false;

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

    private IEnumerator FaceOFF()
    {
        PLAY_ANIMATION_THINK(true);
        Animator doorAnime = GetSelectedDoorAnime(ChooseDoor.selectedDoor);
        if(doorAnime != null)
        {
            doorAnime.SetBool("closeDoor0" + ChooseDoor.selectedDoor, false);
            doorAnime.SetBool("openDoor0"   + ChooseDoor.selectedDoor, true);
        }
        
        yield return new WaitForSeconds(1f);

        doorAnimeOpened = true;
        PLAY_ANIMATION_THINK(false);

        //color door frame
        MeshRenderer DOOR_FRAME = doorToColor(ChooseDoor.selectedDoor);
        Material redOrGreen = (ChooseDoor.selectedDoor == calcucaltions.currentCorrectDoor) ? greenFrameMat : redFrameMat;
        DOOR_FRAME.material = redOrGreen;
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

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2f);
        //close door
        Animator doorAnime = GetSelectedDoorAnime(ChooseDoor.selectedDoor);
        if (doorAnime != null) {
            doorAnime.SetBool("openDoor0" + ChooseDoor.selectedDoor, false);
            doorAnime.SetBool("closeDoor0" + ChooseDoor.selectedDoor, true);
        }

        MeshRenderer DOOR_FRAME = doorToColor(ChooseDoor.selectedDoor);
        DOOR_FRAME.material = defaultMat;
        
        yield return new WaitForSeconds(2.5f);
        canChooseDoor = true;
    }

    IEnumerator Continue()
    {
        PLAY_ANIMATION_THINK(false);
        PLAY_ANIMATION_PASS(true);
        yield return new WaitForSeconds(1.5f);
        Debug.Log("<color=brown>Pass Cage Pos</color>");

        passCagePos = GetSelectedCageDoorTransform(ChooseDoor.selectedDoor);
        transform.LookAt(passCagePos);
        Debug.Log("<color=brown>LOOK ATT</color>");
        PLAY_ANIMATION_PASS(false);
        //MoveOn();
        canChooseDoor = true;
        transition = true;
        Debug.Log("<color=brown>Transitioooon</color>");
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
        Debug.Log("SLIDE 1");
        PLAY_ANIMATION_SLIDE(true);
        yield return new WaitForSeconds(1.15f);
        PLAY_ANIMATION_SLIDE(false);
        Debug.Log("SLIDE 0");

        //transform.LookAt(centerDoorLookPos);
        //////////////////////////////////////////////////////////////////////
        //DestroyPreviousSection(LevelManager.currentLevel);
        //if (changeData)
        //{
        //    Debug.Log("<color=red>curent level = </color>: " + LevelManager.currentLevel);
        //    HandleValues();
        //    changeData = false;
        //}
        if (isWinningSection)
        {
            //GetWinPosition();
            //transform.LookAt(passWinPos);
            //endOfGame = true; 
            Debug.Log("GO TO WIN");

            StateMachine.iCurrentState = (int)StateMachine.PlayerStates.WIN;
        }
        else
        {
            transform.LookAt(centerDoorLookPos);
            door.slide = false;
            StateMachine.iCurrentState = (int)StateMachine.PlayerStates.RUNNING;
        }


    }

    private void GoToEndPosition()
    {

        GetWinPosition();
        GetWinLookAtTarget();
        GetDancePosition();
        float step = 5 * Time.deltaTime;

        //if (PlayerWinCollider.PlayerWon)
        //{
            //JUMP
            transform.LookAt(passWinLookAt);

        //WE HAVE A WINNER
        anime.SetTrigger("win");

        transform.position = Vector3.MoveTowards(transform.position, dancePos.position, step);
            
        //}
        //else
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, passWinPos.position, step);
        //}
      

    }

    public void HandleValues()
    {
        if (LevelManager.currentSectionCount % 2 == 0 && LevelManager.currentSectionCount != 0)
        {
            //GET FIRST SECTION DATA
            Debug.Log("<color=red>FIRST SECTION DATA</color>");
            GetValues(1);
            calcucaltions.GetValues(1);
            cage.GetValues(1);
            //yield return new WaitForSeconds(.75f);
            //calcucaltions.CreateEquation((int)CalculationManager.DIFFICULTIES.EASY, LevelManager.currentLevel);
        }
        else
        {
            //GET SECOND SECTION DATA
            Debug.Log("<color=red>SECOND SECTION DATA</color>");
            GetValues(2);
            calcucaltions.GetValues(2);
            cage.GetValues(2);
            //yield return new WaitForSeconds(.75f);
            //calcucaltions.CreateEquation((int)CalculationManager.DIFFICULTIES.EASY, LevelManager.currentLevel);
        }
    }

    private void DestroyPreviousSection(int currentLvL)
    {
        if(currentLvL >= 2)
        {
            GameObject section1 = GameObject.FindGameObjectWithTag("SectionP1");
            GameObject section2 = GameObject.FindGameObjectWithTag("Section1P1");
            
            GameObject PreviousSection = (currentLvL != 0 && currentLvL % 2 == 0) ? section1 : section2;

            Destroy(PreviousSection);
        }
        else { return; }
        
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
        GetDoorPositions1();
        GetCageDoorPositions1();
        GetOtherPositions1();
        GetDoorFramesMesh1();
    }

    private void GetValuesSection2()
    {
        GetDoorPositions2();
        GetCageDoorPositions2();
        GetOtherPositions2();
        GetDoorFramesMesh2();
    }

    ///////////////////////////////////////////////////////////////////////////////
    // WIN SECTION
    private void GetWinPosition()
    {
        passWinPos = GameObject.FindGameObjectWithTag("winningPos").GetComponent<Transform>();
    }

    private void GetWinLookAtTarget()
    {
        passWinLookAt = GameObject.FindGameObjectWithTag("lookAtTheEnd").GetComponent<Transform>();
    }

    private void GetDancePosition()
    {
        dancePos = GameObject.FindGameObjectWithTag("dancePos").GetComponent<Transform>();
    }

    // SECTION 1 
    private void GetDoorPositions1()
    {
        door0Pos = GameObject.FindGameObjectWithTag("door0Pos").GetComponent<Transform>();
        door1Pos = GameObject.FindGameObjectWithTag("door1Pos").GetComponent<Transform>();
        door2Pos = GameObject.FindGameObjectWithTag("door2Pos").GetComponent<Transform>();
    }

    private void GetCageDoorPositions1()
    {
        cageDoor0Pos = GameObject.FindGameObjectWithTag("cageDoor0").GetComponent<Transform>();
        cageDoor1Pos = GameObject.FindGameObjectWithTag("cageDoor1").GetComponent<Transform>();
        cageDoor2Pos = GameObject.FindGameObjectWithTag("cageDoor2").GetComponent<Transform>();
    }

    private void GetOtherPositions1()
    {
        Debug.Log("other1");
        centerPlayerPos = GameObject.FindGameObjectWithTag("playerThinkPos").GetComponent<Transform>();
        centerDoorLookPos = GameObject.FindGameObjectWithTag("thinkPosLooking").GetComponent<Transform>();
    }

    private void GetDoorFramesMesh1()
    {
        door0_frame = GameObject.FindGameObjectWithTag("0frame").GetComponent<MeshRenderer>();
        door1_frame = GameObject.FindGameObjectWithTag("1frame").GetComponent<MeshRenderer>();
        door2_frame = GameObject.FindGameObjectWithTag("2frame").GetComponent<MeshRenderer>();

    }
    ///////////////////////////////////////////////////////////////////////////////////////
    // SECTION 2
    private void GetDoorPositions2()
    {
        door0Pos = GameObject.FindGameObjectWithTag("door0Pos1").GetComponent<Transform>();
        door1Pos = GameObject.FindGameObjectWithTag("door1Pos1").GetComponent<Transform>();
        door2Pos = GameObject.FindGameObjectWithTag("door2Pos1").GetComponent<Transform>();
    }

    private void GetCageDoorPositions2()
    {
        cageDoor0Pos = GameObject.FindGameObjectWithTag("cageDoor0_1").GetComponent<Transform>();
        cageDoor1Pos = GameObject.FindGameObjectWithTag("cageDoor1_1").GetComponent<Transform>();
        cageDoor2Pos = GameObject.FindGameObjectWithTag("cageDoor2_1").GetComponent<Transform>();
    }

    private void GetOtherPositions2()
    {
        Debug.Log("other2");
        centerPlayerPos = GameObject.FindGameObjectWithTag("playerThinkPos1").GetComponent<Transform>();
        centerDoorLookPos = GameObject.FindGameObjectWithTag("thinkPosLooking1").GetComponent<Transform>();
    }

    private void GetDoorFramesMesh2()
    {
        door0_frame = GameObject.FindGameObjectWithTag("0frame1").GetComponent<MeshRenderer>();
        door1_frame = GameObject.FindGameObjectWithTag("1frame1").GetComponent<MeshRenderer>();
        door2_frame = GameObject.FindGameObjectWithTag("2frame1").GetComponent<MeshRenderer>();

    }
    ///////////////////////////////////////////////////////////////////////////////////////

    private Transform GetSelectedCageDoorTransform(int selectedDoor)
    {
        Transform cageDoorPos;

        switch (selectedDoor)
        {
            case (int)ChooseDoor.Doors.DOOR0:
                cageDoorPos = cageDoor0Pos;
                break;
            case (int)ChooseDoor.Doors.DOOR1:
                cageDoorPos = cageDoor1Pos;
                break;
            case (int)ChooseDoor.Doors.DOOR2:
                cageDoorPos = cageDoor2Pos;
                break;

            default:
                cageDoorPos = cageDoor0Pos;
                Debug.LogError("Failed to request door.. door 0 choosen");
                break;
        }
        Debug.Log(cageDoorPos.name + " GET SELECTED TRANSFORM");
        return cageDoorPos;
    }

    private Animator GetSelectedDoorAnime(int selectedDoor)
    {
        Animator anime = null;

        switch (selectedDoor)
        {
            case (int)ChooseDoor.Doors.DOOR0:
                anime = door0Pos.GetComponentInParent<Animator>();
                break;
            case (int)ChooseDoor.Doors.DOOR1:
                anime = door1Pos.GetComponentInParent<Animator>();
                break;
            case (int)ChooseDoor.Doors.DOOR2:
                anime = door2Pos.GetComponentInParent<Animator>();
                break;
            default:
                break;
        }

        return anime;
    }
}
