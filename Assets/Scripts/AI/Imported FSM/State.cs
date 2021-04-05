using UnityEngine;
using UnityEngine.AI; 

public class State
{
    public enum STATE
    {
        IDLE, THINKING, ROAMING, LOOKNGFORMORE, GREED, DEAD, KILL, SLEEP
    };

    // 'Events' - where we are in the running of a STATE.
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name; // To store the name of the STATE.
    protected EVENT stage; // To store the stage the EVENT is in.
    protected GameObject npc; // To store the NPC game object.
    protected Animator anim; // To store the Animator component.
    protected Transform player; // To store the transform of the player. This will let the guard know where the player is, so it can face the player and know whether it should be shooting or chasing (depending on the distance).
    protected State nextState; // This is NOT the enum above, it's the state that gets to run after the one currently running (so if IDLE was then going to PATROL, nextState would be PATROL).
    protected NavMeshAgent agent; // To store the NPC NavMeshAgent component.

    // Constructor for State
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    // Phases as you go through the state.
    public virtual void Enter() { stage = EVENT.UPDATE; } // Runs first whenever you come into a state and sets the stage to whatever is next, so it will know later on in the process where it's going.
    public virtual void Update() { stage = EVENT.UPDATE; } // Once you are in UPDATE, you want to stay in UPDATE until it throws you out.
    public virtual void Exit() { stage = EVENT.EXIT; } // Uses EXIT so it knows what to run and clean up after itself.

    // The method that will get run from outside and progress the state through each of the different stages.
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Notice that this method returns a 'state'.
        }
        return this; // If we're not returning the nextState, then return the same state.
    }

    // Can the NPC see the player, using a simple Line Of Sight calculation?
    public bool CanSeePlayer()
    {
        return false; // NPC CANNOT see the player.
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - player.position; // Provides the vector from the player to the NPC.
        float angle = Vector3.Angle(direction, npc.transform.forward); // Provide angle of sight.

        // If player is close enough to the NPC AND within the visible viewing angle...
        if (direction.magnitude < 2 && angle < 30)
        {
            return true; // Player IS behind the NPC.
        }
        return false; // Player IS NOT behind the NPC.
    }

    public bool CanAttackPlayer()
    {
        return false; // NPC IS NOT close enough to the player to attack.
    }
}

// Constructor for Idle state.
public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE; // Set name of current state.
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle"); // Sets any current animation state back to Idle.
        base.Enter(); // Sets stage to UPDATE.
    }
    public override void Update()
    {
        if (CanSeePlayer())
        {
 //           nextState = new Pursue(npc, agent, anim, player);
 //           stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
        // The only place where Update can break out of itself. Set chance of breaking out at 10%.
        else if(Random.Range(0,100) < 10)
        {
//            nextState = new Roaming(npc, agent, anim, player);
//            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle"); // Makes sure that any events queued up for Idle are cleared out.
        base.Exit();
    }
}

// Constructor for Patrol state.
public class Roaming : State
{
    int currentIndex = -1;
    public Roaming(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ROAMING; // Set name of current state.
        agent.speed = 2; // How fast your character moves ONLY if it has a path. Not used in Idle state since agent is stationary.
        agent.isStopped = false; // Start and stop agent on current path using this bool.
    }

    public override void Enter()
    {
        // Calculate the closest Waypoint

        anim.SetTrigger("isWalking"); // Start agent walking animation.
        base.Enter();
    }

    public override void Update()
    {
        // Check if agent hasn't finished walking between waypoints.
            // If agent has reached end of waypoint list, go back to the first one, otherwise move to the next one.
 
        if (CanSeePlayer())
        {
 //           nextState = new Pursue(npc, agent, anim, player);
 //           stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }

        else if (IsPlayerBehind())
        {
//            nextState = new RunAway(npc, agent, anim, player);
//            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking"); // Makes sure that any events queued up for Walking are cleared out.
        base.Exit();
    }
}

public class LookingForMore : State
{
    public LookingForMore(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.LOOKNGFORMORE; // State set to match what NPC is doing.
        agent.speed = 5; // Speed set to make sure NPC appears to be running.
        agent.isStopped = false; // Set bool to determine NPC is moving.
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning"); // Set running trigger to change animation.
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);  // Set goal for NPC to reach but navmesh processing might not have taken place, so...
        if(agent.hasPath)                       // ...check if agent has a path yet.
        {
            if (CanAttackPlayer())
            {
 //               nextState = new Attack(npc, agent, anim, player); // If NPC can attack player, set correct nextState.
 //               stage = EVENT.EXIT; // Set stage correctly as we are finished with Pursue state.
            }
            // If NPC can't see the player, switch back to Patrol state.
            else if (!CanSeePlayer())
            {
//                nextState = new Patrol(npc, agent, anim, player); // If NPC can't see player, set correct nextState.
//                stage = EVENT.EXIT; // Set stage correctly as we are finished with Pursue state.
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning"); // Makes sure that any events queued up for Running are cleared out.
        base.Exit();
    }
}

public class Kill : State
{
    float rotationSpeed = 2.0f; // Set speed that NPC will rotate around to face player.
    AudioSource shoot; // To store the AudioSource component.
    public Kill(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.KILL; // Set name to correct state.
        shoot = _npc.GetComponent<AudioSource>(); // Get AudioSource component for shooting sound.
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting"); // Set shooting trigger to change animation.
        agent.isStopped = true; // Stop NPC so he can shoot.
        shoot.Play(); // Play shooting sound.
        base.Enter();
    }

    public override void Update()
    {
        // Calculate direction and angle to player.
        Vector3 direction = player.position - npc.transform.position; // Provides the vector from the NPC to the player.
        float angle = Vector3.Angle(direction, npc.transform.forward); // Provide angle of sight.
        direction.y = 0; // Prevent character from tilting.

        // Rotate NPC to always face the player that he's attacking.
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                            Quaternion.LookRotation(direction),
                                            Time.deltaTime * rotationSpeed);

        if(!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player); // If NPC can't attack player, set correct nextState.
            stage = EVENT.EXIT; // Set stage correctly as we are finished with Attack state.
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting"); // Makes sure that any events queued up for Shooting are cleared out.
        shoot.Stop(); // Stop shooting sound.
        base.Exit();
    }
}