using System;

using UnityEngine;
using System.Collections;

using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerTankController))]
[RequireComponent(typeof(PlayerTurretController))]
public class UserInputManager : MonoBehaviour
{

    private PlayerTankController tankController;

    private PlayerTurretController turretController;

	// Use this for initialization
	void Awake ()
	{
	    this.tankController = this.GetComponent<PlayerTankController>();
	    this.turretController = this.GetComponent<PlayerTurretController>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

	    var turretKeyState = new Boolean[4]
	                             {
	                                 Input.GetKey(KeyCode.I),   // UP
                                     Input.GetKey(KeyCode.K),   // DOWN
                                     Input.GetKey(KeyCode.J),   // LEFT
	                                 Input.GetKey(KeyCode.L)    // RIGHT
	                             };

        this.tankController.Move(h, v, v);

	    if (Input.GetKey(KeyCode.M))
	    {
	        this.turretController.ResetTurretRotation();
	    }

        if (Input.GetKey(KeyCode.N))
        {
            this.turretController.ResetBarrelRotation();
        }

        this.turretController.Move(turretKeyState);

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        this.turretController.Shoot();
	    }
	}
}
