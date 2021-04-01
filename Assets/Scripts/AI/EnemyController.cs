using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float computationDelay = 0.5f;
    public float ReachDistance;
    private string state = "idle";
    bool isIdle = false;
    private Vector3 destination;
    private Vector3[] PointsMemory;
    private int memoryIndex;

 //   enum Days { idle, thinking, roaming };
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyControllerLoop());
    }

    IEnumerator EnemyControllerLoop()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
 //       destination = transform.position;  //  зачем?

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints set - idle");

            state = "idle";
            yield return StartCoroutine(IdleCoroutine());
        }

        if (waypoints.Length > 0)
        {
            ClearMemory();
        }

        /*    string pointsText = "";
             for (int i = 0; i < waypoints.Length; i++)
             {
                 pointsText += " i:" + i + "  vector:" + waypoints[i].position.ToString("F3") +";    ";
             }
             Debug.Log(pointsText); */



        state = "thinking";
        yield return StartCoroutine(ThinkingState());

        state = "roaming";
        yield return StartCoroutine(RoamingState());

        StartCoroutine(EnemyControllerLoop());
    }

    IEnumerator ThinkingState()
    {
        if (state == "thinking")
        {
            if (memoryIndex == 0)
            {
                print("start memory");
                StartCoroutine(StartMemory());
            }
            else if (memoryIndex > 0 && memoryIndex < waypoints.Length)
            {
                FillMemory();
            }
            else
            {
                ClearMemory();
            }

            agent.destination = destination;

            state = "roaming";
            yield return StartCoroutine(RoamingState());
            print("roaming");
        }

        yield return new WaitForSeconds(computationDelay);
    }

    IEnumerator RoamingState()
    {
        if (state == "roaming")
        {
            if (agent.remainingDistance < ReachDistance)
            {
                state = "thinking";
                print("roaming");
                yield return StartCoroutine(ThinkingState());
            }
        }

        yield return new WaitForSeconds(computationDelay);
    }

    IEnumerator IdleCoroutine()
    {
        print("idle");

        while (state == "idle")
        {
            yield return new WaitForSeconds(computationDelay);
        }
    }

    IEnumerator StartMemory()
    {
        Debug.Log("Starting memory");
        //выбираем первую точку в качестве цели, записываем ее в память.
        int wptIndex = Random.Range(0, waypoints.Length - 1);
        Debug.Log("Starting point index = " + wptIndex + "   vector3 = " + waypoints[wptIndex].position.ToString("F3"));
        PointsMemory[0] = waypoints[wptIndex].position;
        destination = PointsMemory[0];
        memoryIndex++;

        yield return null;
    }

    void FillMemory()
    {
        Debug.Log("Filling memory");

        int wptIndex = Random.Range(0, waypoints.Length - 1);
        int stopper = 0;

        for (int i = 0; i <= PointsMemory.Length; i++)
        {
            if (PointsMemory[i] != waypoints[wptIndex].position && PointsMemory[i] != new Vector3(0, 0, 0))
            {
                PointsMemory[memoryIndex] = waypoints[wptIndex].position;
                destination = waypoints[wptIndex].position;
                memoryIndex++;
                break;
            }

            //если по какой-то причине случился облом во всех итерациях генерим новую случайную точку
            if (i == waypoints.Length)
            {
                wptIndex = Random.Range(0, waypoints.Length - 1);
                i = 0;
            }

            stopper++; //если что-то пошло не так
            if (stopper > 1000)
            {
                Debug.LogError("Error thinking - idle");
                state = "idle";
                break;
            }

        }
    }

    void ClearMemory()
    {
        PointsMemory = new Vector3[waypoints.Length];

        memoryIndex = 0;
        Debug.Log("Clearing memory");
        for (int i = 0; i < PointsMemory.Length; i++)
        {
            PointsMemory.SetValue(new Vector3(0, 0, 0), i);
        }
    }
}