using UnityEngine;
using System.Collections;

public class TankPatrolState : IState<TankAiController>
{

    private static TankPatrolState innerIntance;

    private TankPatrolState()
    {
    }

    public static TankPatrolState Instance
    {
        get
        {
            return innerIntance ?? (innerIntance = new TankPatrolState());
        }
    }

    public void Enter(TankAiController agent)
    {
        agent.Navigation.Resume();
    }

    public void UpdateState(TankAiController agent)
    {
        var distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);
        if (distanceToPlayer <= agent.attackDistance)
        {
            agent.StateMachine.ChangeState(TankHostileState.Instance);
        }

        if (agent.Navigation.remainingDistance < agent.navigationTargetValidRadius)
        {
            this.GoToNextPoint(agent);
        }
    }

    public void Exit(TankAiController agent)
    {

    }

    private void GoToNextPoint(TankAiController agent)
    {

        if (agent.wayPoints.Length == 0)
        {
            return;
        }

        agent.Navigation.destination = agent.wayPoints[agent.destinationPoint].position;
        agent.destinationPoint = (agent.destinationPoint + 1) % agent.wayPoints.Length;
    }
}
