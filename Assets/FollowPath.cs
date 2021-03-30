using UnityEngine;

public class FollowPath : MonoBehaviour 
{
    [SerializeField] Transform goal;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float accuracy = 1.0f;
    [SerializeField] float rotSpeed = 2.0f;
    [SerializeField] GameObject wpManager;
    GameObject[] waypoints;
    GameObject currentNode;
    int currentWP = 0;
    Graph graph;

    void Start() 
    {
        waypoints = wpManager.GetComponent<WPManager>().waypoints;
        graph = wpManager.GetComponent<WPManager>().graph;
        currentNode = waypoints[7];
    }

    // Update is called once per frame
    void LateUpdate() {

        // If we've nowhere to go then just return
        if (graph.getPathLength() == 0 || currentWP == graph.getPathLength())
            return;

        //the node we are closest to at this moment
        currentNode = graph.getPathPoint(currentWP);

        //if we are close enough to the current waypoint move to next
        if (Vector3.Distance(
            graph.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        //if we are not at the end of the path
        if (currentWP < graph.getPathLength()) {
            goal = graph.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                            this.transform.position.y,
                                            goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // Rotate towards the heading
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

            // Move the tank
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }
}