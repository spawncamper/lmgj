using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    // Placed on the Robber

    enum stateSelection
    {
        FleeState, SeekState, IdleState, PursueState, EvadeState, WanderState, HideState, StalkState, KeepAwayState
    }   

    [SerializeField] GameObject target;

    Vector3 selectedHidingSpot = Vector3.zero;
    Vector3 wanderTarget = Vector3.zero;

    Drive playerDrive;
    NavMeshAgent agent;
    PlayerClass player;
    AIController AIController;

    void OnEnable()
    {
        AIController.FleeStateEntryEvent += FleeEntry;
        AIController.SeekStateEntryEvent += SeekEntry;
        AIController.IdleStateEntryEvent += IdleEntry;
        AIController.PursueStateEntryEvent += PursueEntry;
        AIController.EvadeStateEntryEvent += EvadeEntry;
        AIController.WanderStateEntryEvent += WanderEntry;
        AIController.HideStateEntryEvent += HideEntry;
        AIController.StalkStateEntryEvent += StalkEntry;
        AIController.KeepAwayStateEntryEvent += KeepAwayEntry;
    }

    void OnDisable()
    {
        AIController.FleeStateEntryEvent -= FleeEntry;
        AIController.SeekStateEntryEvent -= SeekEntry;
        AIController.IdleStateEntryEvent -= IdleEntry;
        AIController.PursueStateEntryEvent -= PursueEntry;
        AIController.EvadeStateEntryEvent -= EvadeEntry;
        AIController.WanderStateEntryEvent -= WanderEntry;
        AIController.HideStateEntryEvent -= HideEntry;
        AIController.StalkStateEntryEvent -= StalkEntry;
        AIController.KeepAwayStateEntryEvent -= KeepAwayEntry;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerDrive = target.GetComponent<Drive>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerClass>();
        AIController = FindObjectOfType<AIController>();
    }

    void UpdateBotBrain(stateSelection newState)
    {        
        if (newState == stateSelection.FleeState)
        {
            Flee(target.transform.position);
        }
        else if (newState == stateSelection.SeekState)
        {
            Seek(target.transform.position);
        }
        else if (newState == stateSelection.IdleState)
        {
            Idle(Vector3.zero);
        }
        else if (newState == stateSelection.PursueState)
        {
            Pursue(Vector3.zero);
        }
        else if (newState == stateSelection.EvadeState)
        {
            Evade(Vector3.zero);
        }
        else if (newState == stateSelection.WanderState)
        {
            Wander(Vector3.zero);
        }
        else if (newState == stateSelection.HideState)
        {
            Hide(Vector3.zero);
        }
        else if (newState == stateSelection.StalkState)
        {
            Stalk(Vector3.zero);
        }
        else if (newState == stateSelection.KeepAwayState)
        {
            KeepAway(Vector3.zero);
        }
        else
        {
            Debug.LogError("[Bot] Update() on object " + gameObject.name + "default case");
        }
    }

    void IdleEntry()
    {
        print("[Bot] IdleEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.IdleState);
    }

    void Idle(Vector3 location)
    {
        agent.isStopped = true;
    }

    void SeekEntry()
    {
        print("[Bot] SeekEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.SeekState);
    }

    void Seek(Vector3 location)
    {
        agent.isStopped = false;
        agent.SetDestination(location);
        Debug.DrawLine(agent.transform.position, location, Color.yellow);
    }

    void FleeEntry()
    {
        print("[Bot] FleeEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.FleeState);
    }

    void Flee(Vector3 location)
    {
        agent.isStopped = false;
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void PursueEntry()
    {
        print("[Bot] PursueEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.PursueState);
    }

    void Pursue(Vector3 location)
    {
        agent.isStopped = false;

        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > AIController.ReturnRightAngle() && relativeHeading < AIController.ReturnWanderDistance()) 
            || playerDrive.currentSpeed < AIController.ReturnZeroSpeed())
        {
            Seek(target.transform.position);
            Debug.DrawLine(agent.transform.position, target.transform.position, Color.yellow);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + playerDrive.currentSpeed);

        Vector3 lookAheadVector = target.transform.position + target.transform.forward * lookAhead;
        Seek(lookAheadVector);

        Debug.DrawLine(agent.transform.position, lookAheadVector, Color.red);
    }

    void EvadeEntry()
    {
        print("[Bot] EvadeEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.EvadeState);
    }

    void Evade(Vector3 location)
    {
        agent.isStopped = false;

        Vector3 targetDir = target.transform.position - this.transform.position;

        float lookAhead = targetDir.magnitude / (agent.speed + playerDrive.currentSpeed);

        Vector3 lookAheadVector = target.transform.position + target.transform.forward * lookAhead;
        Flee(lookAheadVector);

        Debug.DrawLine(agent.transform.position, lookAheadVector * lookAhead, Color.red);
    }

    void WanderEntry()
    {
        print("[Bot] WanderEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.WanderState);
    }

    void Wander(Vector3 location)
    {
        agent.isStopped = false;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * AIController.ReturnWanderJitter(), 0, 
            Random.Range(-1.0f, 1.0f) * AIController.ReturnWanderJitter());
        wanderTarget.Normalize();
        wanderTarget *= AIController.ReturnWanderRadius();

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, AIController.ReturnWanderDistance());
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void HideEntry()
    {
        print("[Bot] HideEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.HideState);
    }

    void Hide(Vector3 location)
    {
        float hidingDistance = Mathf.Infinity;
        Vector3 selectedHidingSpot = Vector3.zero;
        Vector3 chosenDirection = Vector3.zero;
        GameObject obstacleGameObject = World.Instance.GetHidingSpots()[0];

        for (int hidingSpotIndex = 0; hidingSpotIndex < World.Instance.GetHidingSpots().Length; hidingSpotIndex++)
        {
                Vector3 hideDirection = World.Instance.GetHidingSpots()[hidingSpotIndex].transform.position - target.transform.position;
                Vector3 hidePosition = World.Instance.GetHidingSpots()[hidingSpotIndex].transform.position + hideDirection.normalized * AIController.ReturnHidingClearance();

            if (Vector3.Distance(this.transform.position, hidePosition) < hidingDistance)
            {
                selectedHidingSpot = hidePosition;
                hidingDistance = Vector3.Distance(this.transform.position, hidePosition);

                chosenDirection = hideDirection;        // added with CleverHide
                obstacleGameObject = World.Instance.GetHidingSpots()[hidingSpotIndex]; // added with CleverHide
            }
        }

            Collider hideObstacleCollider = obstacleGameObject.GetComponent<Collider>();
            Ray backwardRay = new Ray(selectedHidingSpot, -chosenDirection.normalized);

            RaycastHit rayCastInfo;

            hideObstacleCollider.Raycast(backwardRay, out rayCastInfo, AIController.ReturnHideRaycastDistance());

            Seek(rayCastInfo.point + chosenDirection.normalized * AIController.ReturnHidingClearance());
    }

    void StalkEntry()
    {
        print("[Bot] StalkEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.StalkState);
    }

    bool onCoolDown = false;

    void Stalk(Vector3 location)
    {
        if (onCoolDown == false)
        {
            if (CanSeeTarget() && TargetCanSeeMe())
            {
                Hide(location);
                onCoolDown = true;
                Invoke("BehaviorCoolDown", AIController.ReturnCoolDownDelay());
            }
            else
            {
                Pursue(location);
            }
        }
    }

    void KeepAwayEntry()
    {
        print("[Bot] KeepAwayEntry()" + gameObject.name);
        UpdateBotBrain(stateSelection.KeepAwayState);
    }

    void KeepAway(Vector3 location)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < AIController.ReturnFleeDistance())
        {
            Evade(location);
        }
        else
        {
            Wander(location);
        }
    }

    bool TargetCanSeeMe()
    {
        Vector3 directionToTarget = transform.position - target.transform.position;
        float lookingAngle = Vector3.Angle(target.transform.forward, directionToTarget);

        if (lookingAngle < AIController.ReturnAngleConeView())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 directionToTarget = target.transform.position - this.transform.position;

        if (Physics.Raycast(transform.position, directionToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    void BehaviorCoolDown()
    {
        onCoolDown = false;
    }
}