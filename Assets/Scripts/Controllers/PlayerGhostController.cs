using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

public class PlayerGhostController : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> waypoints = new List<Vector3>();

    [SerializeField]
    private float moveSpeed = 5f;

    private int waypointIndex = 0;

    private bool canMove = false;

    private void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            waypoints = Globals.GhostPath;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if (canMove)
        {
            Move();
        }
        else
        {
            StartCoroutine(SetCanMove());
        }
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
    }

    private IEnumerator SetCanMove()
    {
        yield return new WaitForSeconds(1);

        canMove = true;
    }

    private void Move()
    {
        //If player ghost hasn't reached the last waypoint it can move
        //If player ghost reached last waypoint then it stops
        if (waypointIndex <= waypoints.Count - 1)
        {

            //Move from current waypoint to the next one using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex],
               moveSpeed * Time.deltaTime);

            // If player ghost reaches the position of waypoint its walking towards, starts moving to next waypoint
            if (transform.position == waypoints[waypointIndex])
            {
                waypointIndex += 1;
            }
        }
    }
}

