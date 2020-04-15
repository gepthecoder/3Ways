using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    private  Slider          sLevelBar;

    public Transform   player1;

    private float   fSectionDistanceZ   = 56f;
    private int     iNumOfSection       = 10;

    private Vector3 StartPos;
    private Vector3 EndPos;

    private float totalDistance;
    private float playerDistance;
    private float playerProgress;

    private float levelBarWidth = 560f;
    
    void OnEnable()
    {
        GetTotalDistance();
    }

    void Awake()
    {
        sLevelBar = GetComponent<Slider>();
    }

    void Start()
    {
        GetStartEndPoint();
    }

    void Update()
    {
        playerDistance = player1.position.z - StartPos.z;
        playerProgress = playerDistance / totalDistance * 100;

        sLevelBar.value = playerProgress / 100 * levelBarWidth;
    }

    private void GetTotalDistance()
    {
        totalDistance = fSectionDistanceZ * iNumOfSection; // 560f
    }

    private void GetStartEndPoint()
    {
        StartPos = new Vector3(player1.position.x, player1.position.y, player1.position.z - 3.6f);
        EndPos = new Vector3(player1.position.x, player1.position.y, player1.position.z + totalDistance);
    }


}
