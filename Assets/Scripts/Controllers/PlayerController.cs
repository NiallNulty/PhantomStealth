using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private float speedX;
    private float speedY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        try
        {
            speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
            speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
            rb.velocity = new Vector2(speedX, speedY);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void RotatePlayer()
    {
        try
        {
            Vector2 moveDirection = rb.velocity;
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
