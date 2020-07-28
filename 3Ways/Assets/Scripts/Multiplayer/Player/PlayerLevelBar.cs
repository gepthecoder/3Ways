using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerLevelBar : MonoBehaviour
{
    public static PlayerLevelBar PLB;

    public Slider P1Slider;
    public Slider P2Slider;

    public Transform P1;
    public Transform P2;

    private float fSectionDistanceZ = 56f;
    private int iNumOfSection = 10;
    private float levelBarWidth = 560f;

    // SLIDER PROPS P1
    private Vector3 StartPos;
    private Vector3 EndPos;

    private float totalDistance;
    private float playerDistance;
    private float playerProgress;
    //

    // SLIDER PROPS P2
    private Vector3 StartPos1;
    private Vector3 EndPos1;

    private float totalDistance1;
    private float playerDistance1;
    private float playerProgress1;
    //

    private bool beginUpdate;

    void Awake()
    {
        PLB = this;
        GetTotalDistance();
    }

    void Start()
    {
        P1Slider.value = 0;
        P2Slider.value = 0;
        beginUpdate = false;
    }

    void Update()
    {
        if (beginUpdate)
            HandleProgressBars();
    }

    // HANDLER

    void HandleP1Progress()
    {
        playerDistance = P1.position.z - StartPos.z;
        playerProgress = playerDistance / totalDistance * 100;

        P1Slider.value = playerProgress / 100 * levelBarWidth;

    }

    void HandleP2Progress()
    {
        playerDistance1 = P2.position.z - StartPos1.z;
        playerProgress1 = playerDistance1 / totalDistance1 * 100;

        P2Slider.value = playerProgress1 / 100 * levelBarWidth;

    }

    void HandleProgressBars()
    {
        HandleP1Progress();
        HandleP2Progress();
    }

    private void GetTotalDistance()
    {
        totalDistance = fSectionDistanceZ * iNumOfSection; // 560f
        totalDistance1 = totalDistance;
    }


    private void SetPlayer1()
    {
        GameObject Player1 = GameObject.FindGameObjectWithTag("P1");

        if (Player1 == null)
        {
            Debug.Log("No player 1 jet");
            return;
        }
        Debug.Log("Player 1: " + Player1.GetComponent<PhotonView>().Owner.NickName);

        P1 = Player1.transform;
    }

    private void SetPlayer2()
    {
        GameObject Player2 = GameObject.FindGameObjectWithTag("P2");

        if (Player2 == null) {
            Debug.Log("No player 2 jet");
            return; }
        Debug.Log("Player 2: " + Player2.GetComponent<PhotonView>().Owner.NickName);

        P2 = Player2.transform;
    }

    public void Load()
    {
        SetPlayer1();
        SetPlayer2();

        GET_POSTIONS();
        beginUpdate = true;
    }

    private void GetStartEndPointP1()
    {
        if(P1 == null) { return; }

        Vector3 sPOS1 = new Vector3(P1.position.x, P1.position.y, P1.position.z - 3.6f);
        StartPos = sPOS1;


        Vector3 ePOS1 = new Vector3(P1.position.x, P1.position.y, P1.position.z + totalDistance);
        EndPos = ePOS1;

        Debug.Log(EndPos + "  END POS");
    }

    private void GetStartEndPointP2()
    {
        if(P2 == null) { return; }

        Vector3 sPOS2 = new Vector3(P2.position.x, P2.position.y, P2.position.z - 3.6f);
        StartPos1 = sPOS2;
        
        Vector3 ePOS2 = new Vector3(P2.position.x, P2.position.y, P2.position.z + totalDistance);
        EndPos1 = ePOS2;
        Debug.Log(EndPos1 + "  END POS");
    }

    public void GET_POSTIONS()
    {
        GetStartEndPointP1();
        GetStartEndPointP2();
    }

























    //public static PlayerLevelBar PLB;

    //private Slider playerSlider;

    //public GameObject player1;
    //public GameObject player2;

    //private float fSectionDistanceZ = 56f;
    //private int iNumOfSection = 10;

    //private Vector3 StartPos;
    //private Vector3 EndPos;

    //private float totalDistance;
    //private float playerDistance;
    //private float playerProgress;

    //private float levelBarWidth = 560f;

    //private bool isPlayer1;

    //void OnEnable()
    //{
    //    GetTotalDistance();
    //}

    //void Awake()
    //{
    //    PLB = this;
    //    playerSlider = GetComponent<Slider>();
    //    SetCurrentPlayer();

    //}

    //public void Load()
    //{
    //    Debug.Log("LOAAAD");
    //    SetPlayerTransforms();
    //    if(player1 != null) { Debug.LogWarning("NoPlayer!!!!!"); } else
    //    {
    //        GetStartEndPoint(isPlayer1);
    //    }
    //}

    //void Update()
    //{
    //    if (isPlayer1)
    //    {
    //        if(gameObject.tag == "PLB1")
    //        {
    //            playerDistance = player1.transform.position.z - StartPos.z;
    //            playerProgress = playerDistance / totalDistance * 100;

    //            playerSlider.value = playerProgress / 100 * levelBarWidth;

    //            Debug.Log(playerSlider.value + "  VALUE OF SLIDER");
    //        }
    //        else
    //        {
    //            playerDistance = player2.transform.position.z - StartPos.z;
    //            playerProgress = playerDistance / playerProgress * 100;

    //            playerSlider.value = playerProgress / 100 * levelBarWidth;
    //        }

    //    }
    //    else
    //    {
    //        if (gameObject.tag == "PLB1")
    //        {
    //            playerDistance = player1.transform.position.z - StartPos.z;
    //            playerProgress = playerDistance / totalDistance * 100;

    //            playerSlider.value = playerProgress / 100 * levelBarWidth;

    //            Debug.Log(playerSlider.value + "  VALUE OF SLIDER");
    //        }
    //        else
    //        {
    //            playerDistance = player2.transform.position.z - StartPos.z;
    //            playerProgress = playerDistance / playerProgress * 100;

    //            playerSlider.value = playerProgress / 100 * levelBarWidth;
    //        }
    //    }
    //    //if (gameObject.tag == "PLB1")
    //    //{

    //    //}
    //    //else if(gameObject.tag == "PLB2")
    //    //{

    //    //}

    //}

    /////////FUNCTIONS
    //private void GetTotalDistance()
    //{
    //    totalDistance = fSectionDistanceZ * iNumOfSection; // 560f
    //}

    //private void GetStartEndPoint(bool bPlayer1)
    //{
    //    Vector3 sPOS1 = new Vector3(player1.transform.position.x, player1.transform.position.y, player1.transform.position.z - 3.6f);
    //    Vector3 sPOS2 = new Vector3(player2.transform.position.x, player2.transform.position.y, player2.transform.position.z - 3.6f);
    //    StartPos = bPlayer1 ? sPOS1 : sPOS2;


    //    Vector3 ePOS1 = new Vector3(player1.transform.position.x, player1.transform.position.y, player1.transform.position.z + totalDistance);
    //    Vector3 ePOS2 = new Vector3(player2.transform.position.x, player2.transform.position.y, player2.transform.position.z + totalDistance);

    //    EndPos = new Vector3(player1.transform.position.x, player1.transform.position.y, player1.transform.position.z + totalDistance);
    //    Debug.Log(EndPos + "  END POS");

    //}

    //private void SetCurrentPlayer()
    //{
    //    isPlayer1 = PhotonNetwork.IsMasterClient;
    //}

    //private void SetPlayerTransforms()
    //{
    //    GameObject Player1 = GameObject.FindGameObjectWithTag("P1");
    //    GameObject Player2 = GameObject.FindGameObjectWithTag("P2");

    //    Debug.Log("Player 1: " + Player1.GetComponent<PhotonView>().Owner.NickName);
    //    Debug.Log("Player 2: " + Player2.GetComponent<PhotonView>().Owner.NickName);

    //    player1 = Player1;
    //    player2 = Player2;
    //}

}
