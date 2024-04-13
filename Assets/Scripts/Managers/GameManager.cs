using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Gameplay,
        GameOver,
        LevelComplete
    }

    public GameState gameState = GameState.Gameplay;

    public bool collactableFound = false;

    [SerializeField]
    private GameObject collectableNotification;

    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private GameObject LevelCompleteScreen;

    [SerializeField]
    private TextMeshProUGUI txtBonus;

    [SerializeField]
    private TextMeshProUGUI txtTime;

    [SerializeField]
    private TextMeshProUGUI txtTotalPoints;

    [SerializeField]
    private TimerController timerController;

    public void ShowCollectableNotification()
    {
		try
		{
            StartCoroutine(ShowCollectableNotification(3));
        }
		catch (System.Exception)
		{

			throw;
		}
    }

    public void ShowGameOverScreen()
    {
        try
        {
            GameOverScreen.SetActive(true);
            gameState = GameState.GameOver;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void ShowLevelCompleteScreen()
    {
        try
        {
            LevelCompleteScreen.SetActive(true);
            gameState = GameState.LevelComplete;

            int currentTimeInSeconds = (int)Math.Round(timerController.currentTime, 0);
            string timeRemaining = TimeSpan.FromSeconds(currentTimeInSeconds).ToString("m\\:ss");
            
            //2 points for each second remaining
            int points = currentTimeInSeconds * 2;
            
            txtTime.text = "Time Remaining: " + timeRemaining + " (Points: " + points.ToString() + ")";
            
            if (collactableFound)
            {
                txtBonus.text = "Bonus Collectable Points: 15";
                points += 15;
            }

            txtTotalPoints.text = "Total Points: " + points;

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void RestartLevel()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void LoadMainMenuScene()
    {
        try
        {
            SceneManager.LoadScene("MainMenuScene");
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private IEnumerator ShowCollectableNotification(int seconds)
    {
        collectableNotification.SetActive(true);
        yield return new WaitForSeconds(seconds);
        collectableNotification.SetActive(false);
    }
}
