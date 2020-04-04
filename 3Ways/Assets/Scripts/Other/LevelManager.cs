﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int currentLevel = 1;
 
    public static int currentSectionCount;
    public static bool spawnNewSection = false;


    public GameObject SECTION1;
    public GameObject SECTION2;
    
    public static bool addNewSection;

    private int iSectionToAdd;
   
    private int iEASY_SECTIONS = 5;
    private int iMEDIUM_SECTIONS = 8;
    private int iHARD_SECTIONS = 10;
    private int iGENIOUS_SECTIONS = 15;

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
        if (spawnNewSection)
        {
            CageScript.enemiesSpawned = false;
            bool bIs2ndSection = (currentSectionCount % 2 == 0);
            bool bCanCreate = currentSectionCount >= 2;

            if (bCanCreate)
            {
                if (bIs2ndSection)
                {
                    StartCoroutine(DestroyFirstSection());
                    StartCoroutine(CreateNewSection(true));
                }
                else
                {
                    StartCoroutine(DestroySecondSection());
                    StartCoroutine(CreateNewSection(false));
                }
            }

            spawnNewSection = false;
        }
    }
    // returns total number of sections for each difficulty
    private int GetNumberOfSections(int difficulty)
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

    IEnumerator CreateNewSection(bool is2nd)
    {
        if(currentSectionCount == GetNumberOfSections((int)CalculationManager.DIFFICULTIES.EASY))
        {
            // spawn wining presentation ;)
            yield break;
        }

        int offsetZ = 56;

        if (is2nd)
        { // SPAWN FIRST SECTION, ON SECOND (current) POSITION WITH OFFSET
            Transform section2pos = GameObject.FindGameObjectWithTag("Section1P1").GetComponent<Transform>();

            Vector3 spawnPos = new Vector3(section2pos.position.x, section2pos.position.y, section2pos.position.z + offsetZ);
            Instantiate(SECTION1, spawnPos, section2pos.rotation);

            yield return new WaitForSeconds(1f);
            playerControler.GetValues(1);

        }
        else
        { // SPAWN SECOND SECTION, ON FIRST (current) POSITION WITH OFFSET
            Transform section1pos = GameObject.FindGameObjectWithTag("SectionP1").GetComponent<Transform>();
            Vector3 spawnPos = new Vector3(section1pos.position.x, section1pos.position.y, section1pos.position.z + offsetZ);

            Instantiate(SECTION2, spawnPos, section1pos.rotation);

            yield return new WaitForSeconds(1f);
            playerControler.GetValues(2);

        }
        //int section0 = is2ndSection ? 2 : 1;

        //playerControler.GetValues(section0);
        //calculations.GetValues(section0);
        //cage.GetValues(section0);


        addNewSection = true;
    }

    IEnumerator DestroyFirstSection()
    {
        GameObject firstSection = GameObject.FindGameObjectWithTag("SectionP1");
        Destroy(firstSection);
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator DestroySecondSection()
    {
        GameObject secondSection = GameObject.FindGameObjectWithTag("Section1P1");
        Destroy(secondSection);
        yield return new WaitForSeconds(1.5f);
    }
}