using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private const int STAR_SCORE_AMOUNT = 5;

    public static GameManagerScript Instance { set; get; }

    public bool IsDead { set; get; }
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

        modifierText.text = "x" + modifierScore.ToString("0.0");
        starText.text = starScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");
    }

    private void Update()
    {
        if (MobileTouchInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }

        if (isGameStarted && !IsDead)
        {
            //increase score
            lastScore = (int)score;
            score += (Time.deltaTime * modifierScore);

            if (lastScore != (int)score)
            {
                Debug.Log(lastScore);
                scoreText.text = score.ToString("0");
            }

        }
    }

    public void GetStar()
    {
        starScore++;
        starText.text = starScore.ToString("0");
        score += STAR_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");

    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;

        modifierText.text = "x" + modifierScore.ToString("0.0");
    }
}
