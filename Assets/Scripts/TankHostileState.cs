using UnityEngine;
using System.Collections;

public class TankHostileState : IState<TankAiController>
{

    private static TankHostileState innerIntance;

    private TankHostileState()
    {
    }

    public static TankHostileState Instance
    {
        get
        {
            return innerIntance ?? (innerIntance = new TankHostileState());
        }
    }

    public void Enter(TankAiController agent)
    {
        agent.Navigation.Stop();
    }

    public void UpdateState(TankAiController agent)
    {
        var relativePos = agent.player.position - agent.turret.transform.position;

        var rotation = Quaternion.LookRotation(relativePos);
        var current = agent.turret.transform.rotation;

        agent.turret.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
    }

    public void Exit(TankAiController agent)
    {
        
    }
}
