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

    public int score, piecesLeft;

    // Start is called before the first frame update
    void Start()
    {
        piecesLeft = 100;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        piecesLeftText.text = "Pieces Left: " + piecesLeft.ToString();
    }
    public void AddScore(int points)
    {
        score += points;
    }
}
