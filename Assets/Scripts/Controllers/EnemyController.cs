using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;
    private bool canMove = false;
    private bool canFlip = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) 
        {
            Move();
        }
        else
        {
            StartCoroutine(WaitToMove(2));
        }
    }

    private void Move()
    {
        try
        {
            if (canFlip)
            {
                Flip();
                canFlip = false;
                return;
            }

            if (currentPoint == pointB.transform)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                StartCoroutine(WaitToMove(2));
                currentPoint = pointA.transform;
                canMove = false;
                rb.velocity = new Vector2(0, 0);
                canFlip = true;
                return;
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                StartCoroutine(WaitToMove(2));
                currentPoint = pointB.transform;
                canMove = false;
                rb.velocity = new Vector2(0, 0);
                canFlip = true;
                return;
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void Flip()
    {
        try
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

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

    private IEnumerator WaitToMove(int seconds)
    {
        // Pause for specified seconds
        yield return new WaitForSeconds(seconds);

        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointB.transform.position, pointB.transform.position);
    }
}
