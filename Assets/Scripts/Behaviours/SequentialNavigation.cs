using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class SequentialNavigation : MonoBehaviour
{

    public Transform[] wayPoints;

    public float targetValidRadius = 1.0f;

    private int destinationPoint;

    private NavMeshAgent agent;

    void Start()
	{
	    this.agent = this.GetComponent<NavMeshAgent>();
	}
	
	void GoToNextPoint() {

	    if (this.wayPoints.Length == 0)
	    {
	        return;
	    }

	    this.agent.destination = this.wayPoints[this.destinationPoint].position;
	    this.destinationPoint = (this.destinationPoint + 1) % this.wayPoints.Length;
	}

    void Update()
    {
        if (this.agent.remainingDistance < this.targetValidRadius)
        {
            this.GoToNextPoint();
        }
    }
}
