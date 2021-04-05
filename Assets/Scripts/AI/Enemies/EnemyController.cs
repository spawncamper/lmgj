using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    // Location: on each Enemy prefab

    public float ReachDistance;
    public GameObject treasure;
    public float LookingAroundDelay;
    public float corpseRemainingDelay;

    private Transform[] waypoints;
    private string state = "idle";
    private Vector3 destination;
    private Vector3[] PointsMemory;
    private int memoryIndex;

    private GameObject player;
    EnemyClass enemyClass;

    UnityEngine.AI.NavMeshAgent agent;

    Animator anim;

    public delegate void PlayerDeath();
    public static event PlayerDeath PlayerDeathEvent;

    public delegate void EnemyDeath();
    public static event EnemyDeath EnemyDeathEvent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyClass = GetComponent<EnemyClass>();

        destination = transform.position;

        GameObject[] wptList = GameObject.FindGameObjectsWithTag("waypoint");

        if (wptList.Length > 0)
        {
            waypoints = new Transform[wptList.Length];
            for (int i = 0; i < wptList.Length; i++)
            {
                waypoints.SetValue(wptList[i].transform, i);
                //waypoints[i] = new Transform();
            }
        }
        else
        {
            Debug.LogError("[EnemyController] " + gameObject.name + "wprList.Length <= 0");
        }

        if (waypoints.Length == 0)
        {
            Debug.LogError("[EnemyController] " + gameObject.name + "No waypoints set - idle");
            state = "idle";
        }
        else
        {
            state = "thinking";
            print("[EnemyController] " + gameObject.name + "State Thinking");
        }
        
        string pointsText = "";
        
        for (int i = 0; i < waypoints.Length; i++)
        {
            pointsText += " i:" + i + "  vector:" + waypoints[i].position.ToString("F3") + ";    ";
        }
        Debug.Log(pointsText);

        //������� ������
        if (waypoints.Length > 0)
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


    void Update()
    {
        if (state == "dead")
        {
            print("[EnemyController] state == dead " + gameObject.name);

            return;
        }


        if (!GameManager.isPlayerDead)
        {
            UpdateEnemyControllerFSM();
        }
    }

    void UpdateEnemyControllerFSM()
    {
        if (state == "idle")
        {
            agent.isStopped = true;
            anim.SetBool("idle", true);
        }

        if (state == "dead")
        {
            print("[EnemyController] state == dead , object name " + gameObject.name);

            agent.isStopped = true;
            anim.SetBool("dead", true);

            if (EnemyDeathEvent != null)
                EnemyDeathEvent();

            print("[EnemyController] destroy self " + gameObject.name);

 //           Destroy(gameObject, corpseRemainingDelay);
        }

        if (state == "kill")
        {
            if (agent.remainingDistance < 0.5f)
            {
                print("[EnemyController] state == kill, player death");

                state = "idle";
                agent.isStopped = true;

                if (PlayerDeathEvent != null)
                    PlayerDeathEvent();

//               StartCoroutine(DestroySelf());
            }
            if (agent.remainingDistance > 20f)
            {
                agent.SetDestination(transform.position); //������������� ������
                state = "thinking";
                gameObject.GetComponentInChildren<SightController>().SetMode("straight");
            }
        }

        if (state == "greed")
        {
            Debug.DrawLine(agent.transform.position, treasure.transform.position, Color.yellow);
            if (agent.remainingDistance < 0.2f) //����� �������
            {
                Destroy(treasure, 0);
                state = "LookingForMore";
                agent.isStopped = true;
            }
        }

        if (state == "LookingForMore")
        {
            print("[EnemyController] " + gameObject.name + "STATE LookingFormore");
            gameObject.GetComponent<SightController>().SetMode("wide"); //�������� ������ � ������� �����
           // gameObject.GetComponentInChildren<SightController>().SetMode("wide"); //�������� ������ � ������� �����
            StartCoroutine(LookingAround()); //������������ � ����

        }

        if (state == "thinking")
        {
            if (memoryIndex == 0)
            {
                //Debug.Log("Starting memory");
                //�������� ������ ����� � �������� ����, ���������� �� � ������.
                int wptIndex = Random.Range(0, waypoints.Length - 1);
                //Debug.Log("Starting point index = " + wptIndex + "   vector3 = " + waypoints[wptIndex].position.ToString("F3"));
                PointsMemory[0] = waypoints[wptIndex].position;
                destination = PointsMemory[0];
                memoryIndex++;
            }
            else if (memoryIndex > 0 && memoryIndex < waypoints.Length)
            {
                //Debug.Log("Filling memory");
                int wptIndex = Random.Range(0, waypoints.Length - 1);
                int stopper = 0;
                for (int i = 0; i < PointsMemory.Length; i++)
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
                        //Debug.LogError("Error thinking - idle");
                        state = "idle";
                        break;
                    }

                }
            }
            else
            {
                //������� ������
                memoryIndex = 0;
                //Debug.Log("Clearing memory");
                for (int i = 0; i < PointsMemory.Length; i++)
                {
                    PointsMemory.SetValue(new Vector3(0, 0, 0), i);
                }

            }

            agent.destination = destination;
            state = "roaming";
        }

        if (state == "roaming")
        {
            if (agent.remainingDistance < ReachDistance)
            {
                state = "thinking";

            }
        }
    }


    public void CoinSpotted(Collider coin)
    {
        Debug.Log("CoinSpotted!");
        if (state == "roaming" || state == "thinking" || state == "LookingForMore" || state == "idle" || state == "greed")
        {
            state = "greed";
            if (!treasure)
            {
                agent.SetDestination(coin.gameObject.transform.position);
                treasure = coin.gameObject;
            }
            else
            {
                float curDist = Mathf.Abs(Vector3.Distance(agent.transform.position, agent.destination/*treasure.transform.position*/));
                float newDist = Mathf.Abs(Vector3.Distance(agent.transform.position, coin.gameObject.transform.position));
                Debug.DrawLine(agent.transform.position, agent.destination, Color.green);
                Debug.DrawLine(agent.transform.position, agent.destination, Color.red);
                Debug.Log(curDist.ToString("F3") + "   " + newDist.ToString("F3"));
                if (curDist > newDist)
                {
                    agent.SetDestination(coin.gameObject.transform.position);
                    Debug.Log("Change target!" + coin.gameObject.transform.position.ToString("F3"));
                    treasure = coin.gameObject;
                }
            }

            gameObject.GetComponentInChildren<SightController>().SetMode("disabled"); //��������� ������, ���� ���� � �������
        }

    }


    public void dead()
    {
        state = "dead";
    }


    public void PlayerSpotted(Collider Player)
    {
        Debug.Log("PlayerSpotted!");

        state = "kill";

        if (!player)
        {
            agent.SetDestination(Player.gameObject.transform.position);
            player = Player.gameObject;
            agent.speed = agent.speed * 2;
        }

        gameObject.GetComponentInChildren<SightController>().SetMode("disabled"); //��������� ������, ���� ������� �� ��
    }

    IEnumerator LookingAround()
    {
        //��� ����� ���� �� �������� ������� ���������� ��� �����-������ ��� ������� �� 1-2 �������.
        //state = "idle";
        //Debug.Log("LookingAround coroutine!");
        yield return new WaitForSeconds(LookingAroundDelay);
        //Debug.Log(state);

        if (state == "greed")
        {
            //Debug.Log("Coroutine state changed to greed!");
            yield return null; //���� ��������� ���������� �� greed - ������� ������� - �������.
        }

        if (state != "greed")
        {
            //gameObject.GetComponent<SightController>().SetMode("straight");
            gameObject.GetComponentInChildren<SightController>().SetMode("straight");
            state = "thinking";
        }
        agent.isStopped = false;
    }

}
