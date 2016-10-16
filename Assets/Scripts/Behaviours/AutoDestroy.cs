using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{

    public float timeToLive;

	void Start ()
	{
	    Destroy(this.gameObject, this.timeToLive);
	}
}
