using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private const int STAR_SCORE_AMOUNT = 5;

    public static GameManagerScript Instance { set; get; }

    private bool isGameStarted = false;
    private PlayerMotion motor;

    //UI
    public Text scoreText, starText, modifierText;
    private float score, starScore, modifierScore;
    private int lastScore;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotion>();
        UpdateScores();
    }

    private void Update()
    {
       if (MobileTouchInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }

       if (isGameStarted)
        {
            //increase score
            lastScore = (int)score; 
            score += (Time.deltaTime * modifierScore);

            if (lastScore == (int)score)
            {
                Debug.Log(lastScore);
                scoreText.text = score.ToString("0");
            }
            
        }
    }

    public void GetStar()
    {
        starScore += STAR_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");

    }


    public void UpdateScores()
    {
        scoreText.text = score.ToString();
        starText.text = starScore.ToString();
        modifierText.text = "x" + modifierScore.ToString("0.0");

    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        UpdateScores();
    }
}
