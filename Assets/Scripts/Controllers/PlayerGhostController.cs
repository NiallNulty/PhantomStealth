using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 5f;

    private int waypointIndex = 0;

    private void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //If player ghost hasn't reached the last waypoint it can move
        //If player ghost reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {

            //Move from current waypoint to the next one using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If player ghost reaches the position of waypoint its walking towards, starts moving to next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }
}

