using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public static GameManagerScript Instance { set; get; }

    private bool isGameStarted = false;
    private PlayerMotion motor;

    //UI
    public Text scoreText, coinText, modifierText;
    private float score, coinScore, modifierScore; 

    private void Awake()
    {
        Instance = this;
        UpdateScores();
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotion>();
    }

    private void Update()
    {
       if (MobileTouchInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }
    }

    public void UpdateScores()
    {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();
        modifierText.text = modifierScore.ToString();

    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        UpdateScores();
    }
}
