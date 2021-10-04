using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int boxScoreToWin;
    public int enemyScoreToWin;
    public int boxScore;
    public int enemyScore;
    public GameObject arrow;
    public GameObject walls;
    bool hasWon;
    public AudioSource shootSound;
    public AudioSource enemySound;
    public AudioSource playerSound;
    public AudioSource boxFallSound;
    public AudioSource swapSound;
    bool hasdied;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boxScore >= boxScoreToWin && enemyScore >= enemyScoreToWin)
        {
            hasWon = true;
        }
        if (hasWon)
        {
            arrow.SetActive(true);
            walls.SetActive(false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("shoot", 0f, .1f);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("shoot");
        }
        if (GetComponent<SwapTimeManager>().dying&&!hasdied)
        {
            hasdied = true;
            playerSound.Play();   
        }
    }
    void shoot()
    {
        if(!GetComponent<SwapTimeManager>().paused && !GetComponent<SwapTimeManager>().MoveGlob.GetComponent<playerMovement>().canMove)
        shootSound.Play();
    }
    public void addScore()
    {
        enemyScore++;
        enemySound.Play();
    }
    public void boxScoreAdd()
    {
        boxScore++;
        boxFallSound.Play();

    }
    public void swapSoundPlay()
    {
        swapSound.Play();
    }
}
