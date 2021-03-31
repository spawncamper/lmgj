using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour 
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float accuracy = 1.0f;
    [SerializeField] float rotSpeed = 2.0f;
    [SerializeField] GameObject waypointManager;
    [SerializeField] int waypointRouteLength;
    [SerializeField] bool shouldMove = true;
    [SerializeField] int targetWaypoint;
    [SerializeField] float chaseDistance = 10;
    [SerializeField] float suspicionTime = 2f;
    [SerializeField] float attackSpeed = 5f;
    [SerializeField] float patrolSpeed = 3f;

    GameObject player;
    NavMeshAgent agent;
    GameObject[] levelWaypoints;
    List<Transform> currentWaypointPath;
    GameObject currentNode;

    float distanceToPlayer;
    int currentWP = 0;
    int currentWaypointIndex;

    void Start() 
    {
        levelWaypoints = waypointManager.GetComponent<WPManager>().waypoints;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (this.shouldMove) 
        { 
            FindDistanceToPlayerAndAttack(); 
        }
    }

    void FindDistanceToPlayerAndAttack()
    {
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseDistance)
            {
                ChasePlayer();
            }

            else if (distanceToPlayer > chaseDistance)
            {
                PatrolState();
            }
        }
    }

    void ChasePlayer()
    {
        agent.speed = attackSpeed;

        if (agent.enabled)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
    }

    void PatrolState()
    {
        agent.speed = patrolSpeed;
        Vector3 nextPosition = GetCurrentWaypointPosition(currentWaypointIndex);
        StartMovement(nextPosition);

        if (AtWaypoint())
        {
            SetNextWaypoint();
        }
    }

    void SetNextWaypoint()
    {
        int nextWaypointIndex = CycleWaypoint(currentWaypointIndex);
        currentWaypointIndex = nextWaypointIndex;
    }

    public int CycleWaypoint(int waypointIndex)   //  this needs to go into the PatrolPath.cs
    {
        if (waypointIndex <= (GetChildCount() - 2))
        {
            waypointIndex++;
        }
        else if (waypointIndex == (GetChildCount() - 1))
        {
            waypointIndex = 0;
        }
        return waypointIndex;
    }

    Vector3 GetCurrentWaypointPosition(int wayPointIndex)
    {
        return transform.GetChild(wayPointIndex).transform;
    }

    void StartMovement(Vector3 destination)
    {
        if (agent.enabled)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
        }
    }

    private bool AtWaypoint()
    {
        if (!agent.pathPending && agent != null)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }
}