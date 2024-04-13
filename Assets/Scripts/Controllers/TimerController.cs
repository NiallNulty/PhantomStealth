using System;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public float currentTime = 0f;
    private float startingTime = 70f;

    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState != GameManager.GameState.LevelComplete)
        {
            currentTime -= 1 * Time.deltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(currentTime);

            timer.text = ts.ToString("m\\:ss\\.ff");

            if (currentTime <= 10)
            {
                this.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            if (currentTime <= 0)
            {
                currentTime = 0;
                gameManager.ShowGameOverScreen();
            }
        }
    }
}
