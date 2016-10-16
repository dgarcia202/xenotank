using UnityEngine;
using System.Collections;

public class PlayerTurretController : MonoBehaviour
{
    [SerializeField] private float turretTurnSpeed = 18.0f;

    [SerializeField] private float barrelTurnSpeed = 10.8f;

    [SerializeField] private GameObject shootPoint;

    public GameObject turret;

    public GameObject barrel;

    public Rigidbody projectile;
    
    public float minBarrelAngle = -9.74f;

    public float maxBarrelAngle = 7.54f;

    public float shootForce = (float) 1e+04;

    void Start()
    {
    }

    public void Move(bool[] input)
    {
        if (input[2] && !input[3])
        {
            this.turret.transform.Rotate(Vector3.up, -(this.turretTurnSpeed * Time.deltaTime));
        }

        if (!input[2] && input[3])
        {
            this.turret.transform.Rotate(Vector3.up, this.turretTurnSpeed * Time.deltaTime);
        }

        var linearizedAngle = this.barrel.transform.rotation.eulerAngles.x > 180.0f
                                  ? -(360.0f - this.barrel.transform.rotation.eulerAngles.x)
                                  : this.barrel.transform.rotation.eulerAngles.x;

        if (input[0] && !input[1] && linearizedAngle > this.minBarrelAngle)
        {
            this.barrel.transform.Rotate(Vector3.right, -(this.barrelTurnSpeed * Time.deltaTime));
        }

        if (!input[0] && input[1] && linearizedAngle < this.maxBarrelAngle)
        {
            this.barrel.transform.Rotate(Vector3.right, this.barrelTurnSpeed * Time.deltaTime);
        }
    }

    public void ResetTurretRotation()
    {
        this.turret.transform.Rotate(Vector3.up, -this.turret.transform.localEulerAngles.y);
    }

    public void ResetBarrelRotation()
    {
        this.barrel.transform.Rotate(Vector3.right, -this.barrel.transform.localEulerAngles.x);
    }

    public void Shoot()
    {
        Rigidbody newProjectile = Instantiate(this.projectile, this.shootPoint.transform.position, Quaternion.identity) as Rigidbody;

        newProjectile.gameObject.transform.Rotate(Vector3.right, 90f);

        newProjectile.AddForce(this.shootPoint.transform.forward * this.shootForce);
    }
}
