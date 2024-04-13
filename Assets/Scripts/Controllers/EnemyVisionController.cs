using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.gameState == GameManager.GameState.GameOver)
        {
            return;
        }

        if (collision.gameObject == player)
        {
            gameManager.ShowGameOverScreen();
            player.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
