using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator anime;
    protected bool bAttack;

    private Transform player;

    void Start()
    {
        bAttack = false;
        anime = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void OnTriggerEnter()
    {
        if(anime != null)
        {
            bAttack = true;
            StartCoroutine(Attack(true));
        }
    }

    public void OnTriggerExit()
    {
        if (anime != null)
        {
            bAttack = false;
            StartCoroutine(Attack(false));
        }
    }
    private IEnumerator Attack(bool atck)
    {
        yield return new WaitForSeconds(.75f);
        anime.SetBool("attack", atck);
    }
    
    void Update()
    {
        transform.LookAt(player.position);
    }
}
