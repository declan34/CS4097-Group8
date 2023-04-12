using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    public Text playerScoreText;

	[SerializeField]
	public Text computerScoreText;

	[SerializeField]
    public Text piecesLeftText;

    [SerializeField]
    public Text TimerText;

    public int playerScore, computerScore, piecesLeft;
    public float TimeLeft;
    public bool TimerOn = false;

    public static ScoreSystem Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        TimerOn = true;
        piecesLeft = 100;
        playerScore = 0;
        computerScore = 0;
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
        playerScoreText.text = "Player Score: " + playerScore.ToString();
		computerScoreText.text = "Computer Score: " + computerScore.ToString();
		piecesLeftText.text = "Pieces Left: " + piecesLeft.ToString();
        
    }
    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        TimerText.text = "Time Left: " + string.Format("{0:00} : {1:00}", minutes, seconds);
    }


    public void AddPlayerScore(int points)
    {
        playerScore += points;
    }
	public void AddComputerScore(int points)
	{
		computerScore += points;
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
