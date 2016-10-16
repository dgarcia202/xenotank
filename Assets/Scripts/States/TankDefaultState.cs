using UnityEngine;
using Core;

public class TankDefaultState : IState<TankAiController>
{

    private static TankDefaultState innerIntance;

    private TankDefaultState()
    {
    }

    public static TankDefaultState Instance
    {
        get
        {
            return innerIntance ?? (innerIntance = new TankDefaultState());
        }
    }

    public void Enter(TankAiController agent)
    {
    }

    public void UpdateState(TankAiController agent)
    {
        var distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);
        if (distanceToPlayer >= agent.forgetDistance && agent.StateMachine.CurrentState != TankPatrolState.Instance)
        {
            agent.StateMachine.ChangeState(TankPatrolState.Instance);
        }
    }

    public void Exit(TankAiController agent)
    {

    }
}
