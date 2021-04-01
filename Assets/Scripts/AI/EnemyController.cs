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
    IEnumerator Start()
    {        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        destination = transform.position;  //  �����?

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints set - idle");
            
            state = "idle";
            yield return StartCoroutine(IdleCoroutine());
        }
        else
        {
            state = "thinking";
            yield return StartCoroutine(ThinkingState());
        }

        string pointsText = "";
        for (int i = 0; i < waypoints.Length; i++)
        {
            pointsText += " i:" + i + "  vector:" + waypoints[i].position.ToString("F3") +";    ";
        }
        Debug.Log(pointsText);

        if(waypoints.Length > 0)
        {
            ClearMemory();
        }

        state = "thinking";
        yield return StartCoroutine(ThinkingState());

        state = "roaming";
        yield return StartCoroutine(RoamingState());
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
        //�������� ������ ����� � �������� ����, ���������� �� � ������.
        int wptIndex = Random.Range(0, waypoints.Length - 1);
        Debug.Log("Starting point index = " + wptIndex + "   vector3 = " + waypoints[wptIndex].position.ToString("F3"));
        PointsMemory[0] = waypoints[wptIndex].position;
        destination = PointsMemory[0];
        memoryIndex++;

        yield return new WaitForSeconds(computationDelay);
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

            //���� �� �����-�� ������� �������� ����� �� ���� ��������� ������� ����� ��������� �����
            if (i == waypoints.Length)
            {
                wptIndex = Random.Range(0, waypoints.Length - 1);
                i = 0;
            }

            stopper++; //���� ���-�� ����� �� ���
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