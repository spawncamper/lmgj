using UnityEngine;

public class FollowPath : MonoBehaviour 
{
    Transform goal;
    float speed = 5.0f;
    float accuracy = 1.0f;
    float rotSpeed = 2.0f;
    [SerializeField] GameObject wpManager;
    GameObject[] waypoints;
    GameObject currentNode;
    int currentWP = 0;
    Graph graph;
    [SerializeField] int targetWaypoint;

    void Start() 
    {
        waypoints = wpManager.GetComponent<WPManager>().waypoints;
        graph = wpManager.GetComponent<WPManager>().graph;
        currentNode = waypoints[7];
    }

    void LateUpdate() 
    {
        if (graph.getPathLength() == 0 || currentWP == graph.getPathLength())
            return;

        currentNode = graph.getPathPoint(currentWP);

        if (Vector3.Distance(
            graph.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        if (currentWP < graph.getPathLength()) {
            goal = graph.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                            this.transform.position.y,
                                            goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    public void GoToWayPoint()
    {
        print("button clicked");
        
        // Use the AStar method passing it currentNode and distination
        graph.AStar(currentNode, waypoints[targetWaypoint]);
        // Reset index
        currentWP = 0;
    }
}