using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    public Text scoreText;
    
    [SerializeField]
    public Text piecesLeftText;

    [SerializeField]
    public Text TimerText;

    public int score, piecesLeft;
    public float TimeLeft;
    public bool TimerOn = false;


    // Start is called before the first frame update
    void Start()
    {
        TimerOn = true;
        piecesLeft = 100;
        score = 0;
        TimeLeft = 600;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is Up");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
        scoreText.text = "Score: " + score.ToString();
        piecesLeftText.text = "Pieces Left: " + piecesLeft.ToString();
        
    }
    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        TimerText.text = "Time Left: " + string.Format("{0:00} : {1:00}", minutes, seconds);
    }


    public void AddScore(int points)
    {
        score += points;
    }
    public void subtractTiles(int drawtiles)
    {
        piecesLeft = piecesLeft - drawtiles;
    }
    public void addTiles(int returntiles)
    {
        piecesLeft = piecesLeft + returntiles;
    }
}
