using UnityEngine;
using System.Collections;

public class TankControllerBase : MonoBehaviour
{

    [SerializeField]
    private WheelCollider[] wheelColliders = new WheelCollider[4];

    [SerializeField]
    private Vector3 centerOfMassOffset;

    [SerializeField]
    private float maximumSteerAngle = 25.0f;

    [SerializeField]
    private float topSpeed = 120.0f;

    [SerializeField]
    private float brakeTorque = 20000f;

    [SerializeField]
    private float reverseTorque = 500.0f;

    [SerializeField]
    private float slipLimit = 0.3f;

    [SerializeField]
    private float downForce = 100.0f;

    [SerializeField]
    private float fullTorqueOverAllWheels = 1600f;

    [SerializeField]
    [Range(0, 1)]
    private float tractionControl;

    [SerializeField]
    [Range(0, 1)]
    private float steeringHelper;

    private Rigidbody rigidBody;

    private float currentTorque;

    private float oldRotation;

    public float CurrentSpeed
    {
        get
        {
            return this.rigidBody.velocity.magnitude * 2.23693629f;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();
        this.wheelColliders[0].attachedRigidbody.centerOfMass = this.centerOfMassOffset;
        this.currentTorque = this.fullTorqueOverAllWheels - (this.tractionControl * this.fullTorqueOverAllWheels);
    }

    public void Move(float steering, float accel, float footbrake)
    {
        steering = Mathf.Clamp(steering, -1, 1);
        accel = Mathf.Clamp(accel, 0, 1);
        var footBrake = -1 * Mathf.Clamp(footbrake, -1, 0);

        var steerAngle = steering * this.maximumSteerAngle;
        this.wheelColliders[0].steerAngle = steerAngle;
        this.wheelColliders[1].steerAngle = steerAngle;

        this.SteerHelper();
        this.ApplyDrive(accel, footBrake);
        this.CapSpeed();

        this.AddDownForce();
        this.TractionControl();
    }

    private void ApplyDrive(float accel, float footBrake)
    {
        var thrustTorque = accel * (this.currentTorque / 4f);
        for (int i = 0; i < 4; i++)
        {
            this.wheelColliders[i].motorTorque = thrustTorque;
        }

        foreach (var wheelCollider in this.wheelColliders)
        {
            if (this.CurrentSpeed > 5 && Vector3.Angle(this.transform.forward, this.rigidBody.velocity) < 50f)
            {
                wheelCollider.brakeTorque = this.brakeTorque * footBrake;
            }
            else if (footBrake > 0)
            {
                wheelCollider.brakeTorque = 0f;
                wheelCollider.motorTorque = -this.reverseTorque * footBrake;
            }
        }
    }

    private void SteerHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            this.wheelColliders[i].GetGroundHit(out wheelHit);
            if (wheelHit.normal == Vector3.zero)
            {
                return;
            }

            if (Mathf.Abs(this.oldRotation - this.transform.eulerAngles.y) < 10.0f)
            {
                var turnAdjust = (this.transform.eulerAngles.y - this.oldRotation) * this.steeringHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnAdjust, Vector3.up);
                this.rigidBody.velocity = velRotation * this.rigidBody.velocity;
            }
            this.oldRotation = this.transform.eulerAngles.y;
        }
    }

    private void CapSpeed()
    {
        var speed = this.rigidBody.velocity.magnitude;
        speed *= 3.6f;
        if (speed > this.topSpeed)
        {
            this.rigidBody.velocity = (this.topSpeed / 3.6f) * this.rigidBody.velocity.normalized;
        }
    }

    private void AddDownForce()
    {
        this.wheelColliders[0].attachedRigidbody.AddForce(-this.transform.up * this.downForce * this.wheelColliders[0].attachedRigidbody.velocity.magnitude);
    }

    private void TractionControl()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            this.wheelColliders[i].GetGroundHit(out wheelHit);
            this.AdjustTorque(wheelHit.forwardSlip);
        }
    }

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= this.slipLimit && this.currentTorque >= 0)
        {
            this.currentTorque -= 10 * this.tractionControl;
        }
        else
        {
            this.currentTorque += 10 * this.tractionControl;
            if (this.currentTorque > this.fullTorqueOverAllWheels)
            {
                this.currentTorque = this.fullTorqueOverAllWheels;
            }
        }
    }
}
