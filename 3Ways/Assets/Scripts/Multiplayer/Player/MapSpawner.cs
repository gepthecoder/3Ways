using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MapSpawner : MonoBehaviourPunCallbacks
{
    public int currentLevel = 1;

    public int currentSectionCount;
    public bool spawnNewSection = false;
    public bool spawnWinSection = false;

    public GameObject P1SECTION1;
    public GameObject P1SECTION2;

    public GameObject P2SECTION1;
    public GameObject P2SECTION2;

    public GameObject WINNING_SECTION;

    public static bool addNewSection;
    private int iSectionToAdd;

    public int iEASY_SECTIONS = 10;
    public int iMEDIUM_SECTIONS = 10;
    public int iHARD_SECTIONS = 10;
    public int iGENIOUS_SECTIONS = 10;

    private AvatarControlHandler playerControler;
    private PlayerCalculationManager calculations;

    private PhotonView PV;


    //private CageScript cage;

    private bool isPlayer1;


    public int winningSectionOffsetZ = -3;
    public float winningSectionOffsetX = 6.38f;
    public float winningSectionOffsetY = .05f;

    void Awake()
    {
        playerControler = GetComponent<AvatarControlHandler>();
        calculations = GetComponent<PlayerCalculationManager>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        isPlayer1 = playerControler.isMasterClient;
        //cage = GetComponent<CageScript>();
    }

    void Update()
    {
        if (!PV.IsMine) { return; }

        if (spawnWinSection)
        {
            if(GameObject.FindGameObjectWithTag("WIN_SECTION") != null)
            {
                // spawn 1 win section only
                playerControler.isWinningSection = true;

                return;
            }

            bool bIs2ndSection = (currentSectionCount % 2 == 0);
            // spawn wining presentation ;)
            //int   winningSectionOffsetZ = -9;
            //float winningSectionOffsetX = 6.38f;
            //float winningSectionOffsetY = .05f;

            Debug.Log("<color=green>Winning section spawned!!</color>");
            if (isPlayer1)
            {
                if (bIs2ndSection)
                {
                    //offset from second section
                    Transform section2pos = GameObject.FindGameObjectWithTag("Section1P1").GetComponent<Transform>();
                    Vector3 spawnPos = new Vector3(section2pos.position.x + winningSectionOffsetX, section2pos.position.y + winningSectionOffsetY, section2pos.position.z + winningSectionOffsetZ);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerFinnishLineSection"), spawnPos, WINNING_SECTION.transform.rotation, 0);
                }
                else
                {
                    //offset from first section
                    Transform section1pos = GameObject.FindGameObjectWithTag("SectionP1").GetComponent<Transform>();
                    Vector3 spawnPos = new Vector3(section1pos.position.x + winningSectionOffsetX, section1pos.position.y + winningSectionOffsetY, section1pos.position.z + winningSectionOffsetZ);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerFinnishLineSection"), spawnPos, WINNING_SECTION.transform.rotation, 0);
                }

                spawnWinSection = false;
                playerControler.isWinningSection = true;
            }
            else
            {
                if (bIs2ndSection)
                {
                    //offset from second section
                    Transform section2pos = GameObject.FindGameObjectWithTag("Section1P2").GetComponent<Transform>();
                    Vector3 spawnPos = new Vector3(section2pos.position.x + winningSectionOffsetX, section2pos.position.y + winningSectionOffsetY, section2pos.position.z + winningSectionOffsetZ);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerFinnishLineSection"), spawnPos, WINNING_SECTION.transform.rotation, 0);
                }
                else
                {
                    //offset from first section
                    Transform section1pos = GameObject.FindGameObjectWithTag("SectionP2").GetComponent<Transform>();
                    Vector3 spawnPos = new Vector3(section1pos.position.x + winningSectionOffsetX, section1pos.position.y + winningSectionOffsetY, section1pos.position.z + winningSectionOffsetZ);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerFinnishLineSection"), spawnPos, WINNING_SECTION.transform.rotation, 0);
                }

                spawnWinSection = false;
                playerControler.isWinningSection = true;
            }
           
        }

        if (spawnNewSection)
        {
            bool bIs2ndSection = (currentSectionCount % 2 == 0);
            Debug.Log("create new section..");
            if (isPlayer1)
            {
                if (bIs2ndSection)
                {
                    StartCoroutine(DestroyFirstSection(true));
                    CreateNewSection(true);
                    Debug.Log("<color=cyan>2nd section spawned</color>");
                    StartCoroutine(TransitionToNextLevel());

                }
                else
                {
                    StartCoroutine(DestroySecondSection(true));
                    CreateNewSection(false);
                    Debug.Log("<color=yellow>1st section spawned</color>");
                    StartCoroutine(TransitionToNextLevel());
                }

                spawnNewSection = false;
            }
            else
            {
                if (bIs2ndSection)
                {
                    StartCoroutine(DestroyFirstSection(false));
                    CreateNewSection(true);
                    Debug.Log("<color=cyan>2nd section spawned</color>");
                    StartCoroutine(TransitionToNextLevel());

                }
                else
                {
                    StartCoroutine(DestroySecondSection(false));
                    CreateNewSection(false);
                    Debug.Log("<color=yellow>1st section spawned</color>");
                    StartCoroutine(TransitionToNextLevel());
                }

                spawnNewSection = false;
            }

        }
    }

    IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("<color=red>Call create equation</color>");
        playerControler.HandleValues();
        calculations.CreateEquation(calculations.currentDifficulty, currentLevel);
        //CageScript.enemiesSpawned = false;

    }
    // returns total number of sections for each difficulty
    public int GetNumberOfSections(int difficulty)
    {
        int numOfSections = 0;

        switch (difficulty)
        {
            case (int)PlayerCalculationManager.DIFFICULTIES.EASY:
                numOfSections = iEASY_SECTIONS;
                break;
            case (int)PlayerCalculationManager.DIFFICULTIES.MEDIUM:
                numOfSections = iMEDIUM_SECTIONS;
                break;
            case (int)PlayerCalculationManager.DIFFICULTIES.HARD:
                numOfSections = iHARD_SECTIONS;
                break;
            case (int)PlayerCalculationManager.DIFFICULTIES.GENIOUS:
                numOfSections = iGENIOUS_SECTIONS;
                break;
            default:
                numOfSections = iEASY_SECTIONS;
                break;
        }

        return numOfSections;
    }

    void CreateNewSection(bool is2nd)
    {
        int offsetZ = 56;

        if (isPlayer1)
        {
            if (is2nd)
            { // SPAWN FIRST SECTION, ON SECOND (current) POSITION WITH OFFSET
                Transform section2pos = GameObject.FindGameObjectWithTag("Section1P1").GetComponent<Transform>();
                Debug.Log("SPAWN 1ST SECTION!");
                Vector3 spawnPos = new Vector3(section2pos.position.x, section2pos.position.y, section2pos.position.z + offsetZ);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P1SECTION1"), spawnPos, section2pos.rotation, 0);

            }
            else
            { // SPAWN SECOND SECTION, ON FIRST (current) POSITION WITH OFFSET
                Transform section1pos = GameObject.FindGameObjectWithTag("SectionP1").GetComponent<Transform>();
                Vector3 spawnPos = new Vector3(section1pos.position.x, section1pos.position.y, section1pos.position.z + offsetZ);
                Debug.Log("SPAWN 2ND SECTION!");
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P1SECTION2"), spawnPos, section1pos.rotation, 0);
            }
        }
        else
        {
            if (is2nd)
            { // SPAWN FIRST SECTION, ON SECOND (current) POSITION WITH OFFSET
                Transform section2pos = GameObject.FindGameObjectWithTag("Section1P2").GetComponent<Transform>();
                Debug.Log("SPAWN 1ST SECTION!");
                Vector3 spawnPos = new Vector3(section2pos.position.x, section2pos.position.y, section2pos.position.z + offsetZ);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P2SECTION1"), spawnPos, section2pos.rotation, 0);
            }
            else
            { // SPAWN SECOND SECTION, ON FIRST (current) POSITION WITH OFFSET
                Transform section1pos = GameObject.FindGameObjectWithTag("SectionP2").GetComponent<Transform>();
                Vector3 spawnPos = new Vector3(section1pos.position.x, section1pos.position.y, section1pos.position.z + offsetZ);
                Debug.Log("SPAWN 2ND SECTION!");
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P2SECTION2"), spawnPos, section1pos.rotation, 0);
            }
        }

       
    }

    IEnumerator DestroyFirstSection(bool bPlayer1)
    {
        if (bPlayer1)
        {
            GameObject firstSection = GameObject.FindGameObjectWithTag("SectionP1");
            if (firstSection == null)
            {
                Debug.LogWarning("FirstSection doesnt exist!");
                yield break;
            }
            Destroy(firstSection);
            Debug.Log("FirstSectionDestroyed");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            GameObject firstSection = GameObject.FindGameObjectWithTag("SectionP2");
            if (firstSection == null)
            {
                Debug.LogWarning("FirstSection doesnt exist!");
                yield break;
            }
            Destroy(firstSection);
            Debug.Log("FirstSectionDestroyed");
            yield return new WaitForSeconds(1f);
        }
       
    }

    IEnumerator DestroySecondSection(bool bPlayer1)
    {
        if (bPlayer1)
        {
            GameObject secondSection = GameObject.FindGameObjectWithTag("Section1P1");
            if (secondSection == null)
            {
                Debug.LogWarning("SecondSection doesnt exist!");
                yield break;
            }
            Destroy(secondSection);
            Debug.Log("SecondSectionDestroyed");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            GameObject secondSection = GameObject.FindGameObjectWithTag("Section1P2");
            if (secondSection == null)
            {
                Debug.LogWarning("SecondSection doesnt exist!");
                yield break;
            }
            Destroy(secondSection);
            Debug.Log("SecondSectionDestroyed");
            yield return new WaitForSeconds(1f);
        }
      
    }

}
