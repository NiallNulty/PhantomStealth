using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private float speedX;
    private float speedY;
    private Rigidbody2D rb;

    public bool isFinished = false;

    public string playerPathString = string.Empty;

    [SerializeField]
    public SerializableList<Vector3> playerPath = new SerializableList<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SetPlayerPathPoints());
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

    private IEnumerator SetPlayerPathPoints()
    {
        yield return new WaitForSeconds(.1f);

        playerPath.playerPath.Add(transform.position);

        if (!isFinished)
        {
            StartCoroutine(SetPlayerPathPoints());
        }
    }

    public void SetPlayerPathString()
    {
        playerPathString = JsonUtility.ToJson(playerPath).ToString();
    }
}
