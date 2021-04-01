using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    public float ReachDistance;
    private string state = "idle";
    private Vector3 destination;
    private Vector3[] PointsMemory;
    private int memoryIndex;

 //   enum Days { idle, thinking, Clearing memory, Tue, Wed, Thu, Fri };

    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        destination = transform.position;

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints set - idle");
            state = "idle";
        }
        else
        {
            state = "thinking";
        }
        string pointsText = "";
        for (int i = 0; i < waypoints.Length; i++)
        {
            pointsText += " i:" + i + "  vector:" + waypoints[i].position.ToString("F3") +";    ";
        }
        Debug.Log(pointsText);

        //очищаем память
        if(waypoints.Length > 0)
        {
            ClearMemory();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(state == "thinking")
        {
            if(memoryIndex == 0)
            {
                StartMemory();
            }
            else if(memoryIndex > 0 && memoryIndex < waypoints.Length)
            {
                FillMemory();                
            }
            else
            {
                ClearMemory();
            }

         agent.destination = destination;
            state = "roaming";
        }

        if (state == "roaming")
        {
            if(agent.remainingDistance < ReachDistance)
            {
                state = "thinking";
            }
        }
    }

    void StartMemory()
    {
        Debug.Log("Starting memory");
        //выбираем первую точку в качестве цели, записываем ее в память.
        int wptIndex = Random.Range(0, waypoints.Length - 1);
        Debug.Log("Starting point index = " + wptIndex + "   vector3 = " + waypoints[wptIndex].position.ToString("F3"));
        PointsMemory[0] = waypoints[wptIndex].position;
        destination = PointsMemory[0];
        memoryIndex++;
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
