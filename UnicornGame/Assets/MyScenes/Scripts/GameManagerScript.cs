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
    private PlayerMotion motion;

    //UI
    public Animator gameCanvas, menuAnimator, starAnimator;
    public Text scoreText, starText, modifierText;
    private float score, starScore, modifierScore;
    private int lastScore;

    //Game over menu
    public Animator gameOverAnim;
    public Text gameOverScoreText, gameOverStarText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motion = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotion>();

        modifierText.text = "x" + modifierScore.ToString("0.0");
        starText.text = starScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");
    }

    private void Update()
    {
        if (MobileTouchInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motion.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotion>().IsMoving = true; //a bit expensive but since it's only activated once it's fine
            gameCanvas.SetTrigger("Show");
            menuAnimator.SetTrigger("Hide");

        }

        if (isGameStarted && !IsDead)
        {
            //increase score
            lastScore = (int)score;
            score += (Time.deltaTime * modifierScore);

            if (lastScore != (int)score)
            {
                //Debug.Log(lastScore);
                scoreText.text = score.ToString("0");
            }

        }
    }

    public void GetStar()
    {
        starAnimator.SetTrigger("Collect");
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

    public void OnPLayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnGameOver()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = true;
        gameOverScoreText.text = score.ToString("0");
        gameOverStarText.text = starScore.ToString("0");
        gameOverAnim.SetTrigger("Dead");
    }
}
