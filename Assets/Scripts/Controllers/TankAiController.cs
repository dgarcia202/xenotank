using UnityEngine;
using Core;

[RequireComponent(typeof(NavMeshAgent))]
public class TankAiController : MonoBehaviour
{
    private NavMeshAgent agent;

    private StateMachine<TankAiController> stateMachine;

    public Transform player;

    public Transform[] wayPoints;

    public float navigationTargetValidRadius = 1.0f;

    public float attackDistance = 50.0f;

    public float forgetDistance = 150.0f;

    public float gunCoolDownTime = 2.0f;

    [HideInInspector] public int destinationPoint;

    [HideInInspector] public Transform turret;

    public NavMeshAgent Navigation
    {
        get
        {
            return this.agent;
        }
    }

    public StateMachine<TankAiController> StateMachine
    {
        get
        {
            return this.stateMachine;
        }
    }

    void Start()
    {
        this.turret = this.transform.Find("Turret");
        this.agent = this.GetComponent<NavMeshAgent>();
        this.stateMachine = new StateMachine<TankAiController>(this, TankPatrolState.Instance, TankDefaultState.Instance);
    }

    void Update()
    {
        this.stateMachine.Update();
    }
}
