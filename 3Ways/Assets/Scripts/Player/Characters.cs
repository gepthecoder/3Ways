using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    private int iCurrentCharacter;

    public GameObject character0;
    public GameObject character1;
    public GameObject character2;
    
    void Awake()
    {
        GetPlayerPrefs();
        SetCharacter(iCurrentCharacter);
    }

    private void GetPlayerPrefs()
    {
        iCurrentCharacter = PlayerPrefs.GetInt("iCurrentCharacter", 0);
    }

    private void SetCharacter(int selectedChar)
    {
        switch (selectedChar)
        {
            case 0:
                character0.SetActive(true);
                character1.SetActive(false);
                character2.SetActive(false);
                break;
            case 1:
                character0.SetActive(false);
                character1.SetActive(true);
                character2.SetActive(false);
                break;
            case 2:
                character0.SetActive(false);
                character1.SetActive(false);
                character2.SetActive(true);
                break;

            default:

                character0.SetActive(true);
                character1.SetActive(false);
                character2.SetActive(false);
                break;
        }
    }

}
