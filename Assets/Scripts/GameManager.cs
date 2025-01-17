using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public CannonPlayer player;
    public Text scoreText;
    private int scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ScoreUp(int score)
    {
        scoreCount += score;
        /*
        scoreText.text = scoreCount.ToString();*/
        scoreText.text = string.Format("{0:#,###}", scoreCount);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        gameOverText.SetActive(true);
        player.PlayerGameOver();
    }
}
