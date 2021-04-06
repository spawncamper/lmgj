using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{    
    public enum stateSelection
    {
        KeepAwayState, FleeState, SeekState, IdleState, WanderState, HideState
        // PursueState, EvadeState, StalkState
    }

    [SerializeField] stateSelection selectedState;
    [SerializeField] float delayAIcalculation = 1f;
    [SerializeField] bool manualControl = true;
    [SerializeField] float coolDownDelay = 5f;
    [SerializeField] float rightAngle = 90f;
    [SerializeField] float playerAngleConeView = 90f;
    [SerializeField] float smallestAngleToPursue = 20f;
    [SerializeField] float hidingClearance = 5f;
    [SerializeField] float hideRaycastDistance = 100f;
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float wanderDistance = 10f;
    [SerializeField] float wanderJitter = 1f;
    [SerializeField] float fleeDistance = 10f;
    [SerializeField] float zeroSpeed = 0.1f;

    stateSelection currentState;

    public delegate void FleeStateEntry();
    public static event FleeStateEntry FleeStateEntryEvent;

    public delegate void SeekStateEntry();
    public static event SeekStateEntry SeekStateEntryEvent;

    public delegate void IdleStateEntry();
    public static event IdleStateEntry IdleStateEntryEvent;

    public delegate void PursueStateEntry();
    public static event PursueStateEntry PursueStateEntryEvent;

    public delegate void EvadeStateEntry();
    public static event EvadeStateEntry EvadeStateEntryEvent;

    public delegate void WanderStateEntry();
    public static event WanderStateEntry WanderStateEntryEvent;

    public delegate void HideStateEntry();
    public static event WanderStateEntry HideStateEntryEvent;

    public delegate void StalkStateEntry();
    public static event StalkStateEntry StalkStateEntryEvent;

    public delegate void KeepAwayStateEntry();
    public static event KeepAwayStateEntry KeepAwayStateEntryEvent;

    IEnumerator Start()
    {
        if (currentState != stateSelection.IdleState)
        {
            Debug.LogWarning("[AIController] other than IdleState is selected at Start");
        }

        while (manualControl)
        {
                yield return UpdateAIStateOnce();
        }

        while (!manualControl)
        {
            print("[AIController] Started Automatic routine");
            
            selectedState = stateSelection.SeekState;
            StartCoroutine(UpdateAIStateOnce());
            yield return new WaitForSeconds(delayAIcalculation * 5);

            selectedState = stateSelection.IdleState;
            StartCoroutine(UpdateAIStateOnce());
            yield return new WaitForSeconds(delayAIcalculation * 5);

            selectedState = stateSelection.KeepAwayState;
            StartCoroutine(UpdateAIStateOnce());
            yield return new WaitForSeconds(delayAIcalculation * 10);
        }
    }

    IEnumerator UpdateAIStateOnce()
    {
        currentState = selectedState;

        if (currentState == stateSelection.FleeState)
        {
            if (FleeStateEntryEvent != null)
                FleeStateEntryEvent();
        }
        else if (currentState == stateSelection.SeekState)
        {
            if (SeekStateEntryEvent != null)
                SeekStateEntryEvent();
        }
        else if (currentState == stateSelection.IdleState)
        {
            if (IdleStateEntryEvent != null)
                IdleStateEntryEvent();
        }
 //       else if (currentState == stateSelection.PursueState)
 //       {
 //           if (PursueStateEntryEvent != null)
 //               PursueStateEntryEvent();
 //       }
 //       else if (currentState == stateSelection.EvadeState)
 //       {
 //           if (EvadeStateEntryEvent != null)
 //               EvadeStateEntryEvent();
 //       }
        else if (currentState == stateSelection.WanderState)
        {
            if (WanderStateEntryEvent != null)
                WanderStateEntryEvent();
        }
        else if (currentState == stateSelection.HideState)
        {
            if (HideStateEntryEvent != null)
                HideStateEntryEvent();
        }
//        else if (currentState == stateSelection.StalkState)
//        {
//            if (StalkStateEntryEvent != null)
//                StalkStateEntryEvent();
//        }
        else if (currentState == stateSelection.KeepAwayState)
        {
            if (KeepAwayStateEntryEvent != null)
                KeepAwayStateEntryEvent();
        }
        else
        {
            Debug.LogError("[AIController] Update() on object " + gameObject.name + "default case");
        }

        yield return new WaitForSeconds(delayAIcalculation);
    }

    public float ReturnRightAngle()
    {
        return rightAngle;
    }

    public float ReturnHidingClearance()
    {
        return hidingClearance;
    }

    public float ReturnCoolDownDelay()
    {
        return coolDownDelay;
    }

    public float ReturnWanderRadius()
    {
        return wanderRadius;
    }

    public float ReturnWanderDistance()
    {
        return wanderDistance;
    }

    public float ReturnWanderJitter()
    {
        return wanderJitter;
    }

    public float ReturnHideRaycastDistance()
    {
        return hideRaycastDistance;
    }

    public float ReturnZeroSpeed()
    {
        return zeroSpeed;
    }

    public float ReturnSmallestAngleToPursue()
    {
        return smallestAngleToPursue;
    }

    public float ReturnFleeDistance()
    {
        return fleeDistance;
    }

    public float ReturnAngleConeView()
    {
        return playerAngleConeView;
    }
}