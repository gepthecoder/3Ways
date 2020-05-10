using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int currentLevel = 1;
 
    public static int currentSectionCount;
    public static bool spawnNewSection = false;
    public static bool spawnWinSection = false;

    public GameObject SECTION1;
    public GameObject SECTION2;
    public GameObject WINNING_SECTION;

    public static bool addNewSection;
    private int iSectionToAdd;

    public static int iEASY_SECTIONS = 3;
    public static int iMEDIUM_SECTIONS = 8;
    public static int iHARD_SECTIONS = 10;
    public static int iGENIOUS_SECTIONS = 15;

    private PlayerControl playerControler;
    private CalculationManager calculations;
    private CageScript cage;

    void Start()
    {        
        playerControler = GetComponent<PlayerControl>();
        calculations = GetComponent<CalculationManager>();
        cage = GetComponent<CageScript>();
    }

    void Update()
    {
        if (spawnWinSection)
        {
            bool bIs2ndSection = (currentSectionCount % 2 == 0);
            // spawn wining presentation ;)
            int winningSectionOffsetZ = -9;
            float winningSectionOffsetX = 6.38f;
            float winningSectionOffsetY = .05f;

            Debug.Log("<color=green>Winning section spawned!!</color>");
            if (bIs2ndSection)
            {
                //offset from second section
                Transform section2pos = GameObject.FindGameObjectWithTag("Section1P1").GetComponent<Transform>();
                Vector3 spawnPos = new Vector3(section2pos.position.x + winningSectionOffsetX, section2pos.position.y + winningSectionOffsetY, section2pos.position.z + winningSectionOffsetZ);
                Instantiate(WINNING_SECTION, spawnPos, WINNING_SECTION.transform.rotation);
            }
            else
            {
                //offset from first section
                Transform section1pos = GameObject.FindGameObjectWithTag("SectionP1").GetComponent<Transform>();
                Vector3 spawnPos = new Vector3(section1pos.position.x + winningSectionOffsetX, section1pos.position.y + winningSectionOffsetY, section1pos.position.z + winningSectionOffsetZ);
                Instantiate(WINNING_SECTION, spawnPos, WINNING_SECTION.transform.rotation);
            }

            spawnWinSection = false;
            PlayerControl.isWinningSection = true;
        }

        if (spawnNewSection)
        {
            bool bIs2ndSection = (currentSectionCount % 2 == 0);
            Debug.Log("create new section..");
            if (bIs2ndSection)
            {
                StartCoroutine(DestroyFirstSection());
                CreateNewSection(true);
                Debug.Log("<color=cyan>2nd section spawned</color>");
                StartCoroutine(TransitionToNextLevel());
                
            }
            else
            {
                StartCoroutine(DestroySecondSection());
                CreateNewSection(false);
                Debug.Log("<color=yellow>1st section spawned</color>");
                StartCoroutine(TransitionToNextLevel());
            }

            spawnNewSection = false;
        }
    }

    IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("<color=red>Call create equation</color>");
        playerControler.HandleValues();
        calculations.CreateEquation(CalculationManager.currentDifficulty, LevelManager.currentLevel);
        CageScript.enemiesSpawned = false;

    }
    // returns total number of sections for each difficulty
    public int GetNumberOfSections(int difficulty)
    {
        int numOfSections = 0;

        switch (difficulty)
        {
            case (int)CalculationManager.DIFFICULTIES.EASY:
                numOfSections = iEASY_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.MEDIUM:
                numOfSections = iMEDIUM_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.HARD:
                numOfSections = iHARD_SECTIONS;
                break;
            case (int)CalculationManager.DIFFICULTIES.GENIOUS:
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

        if (is2nd)
        { // SPAWN FIRST SECTION, ON SECOND (current) POSITION WITH OFFSET
            Transform section2pos = GameObject.FindGameObjectWithTag("Section1P1").GetComponent<Transform>();
            Debug.Log("SPAWN 1ST SECTION!");
            Vector3 spawnPos = new Vector3(section2pos.position.x, section2pos.position.y, section2pos.position.z + offsetZ);
            Instantiate(SECTION1, spawnPos, section2pos.rotation);
        }
        else
        { // SPAWN SECOND SECTION, ON FIRST (current) POSITION WITH OFFSET
            Transform section1pos = GameObject.FindGameObjectWithTag("SectionP1").GetComponent<Transform>();
            Vector3 spawnPos = new Vector3(section1pos.position.x, section1pos.position.y, section1pos.position.z + offsetZ);
            Debug.Log("SPAWN 2ND SECTION!");
            Instantiate(SECTION2, spawnPos, section1pos.rotation);
        }
    }

    IEnumerator DestroyFirstSection()
    {
        GameObject firstSection = GameObject.FindGameObjectWithTag("SectionP1");
        if(firstSection == null) { Debug.LogWarning("FirstSection doesnt exist!");
            yield break; }
        Destroy(firstSection);
        Debug.Log("FirstSectionDestroyed");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator DestroySecondSection()
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

    
}
