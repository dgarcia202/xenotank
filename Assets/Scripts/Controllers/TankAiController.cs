using UnityEngine;
using UnityEngine.UI;
using Core;

[RequireComponent(typeof(NavMeshAgent))]
public class TankAiController : MonoBehaviour
{
    private NavMeshAgent agent;

    private StateMachine<TankAiController> stateMachine;

    [SerializeField] private GameObject explosion;

    [SerializeField] private GameObject shootPoint;

    private float coolDownState = 0.0f;

    public Transform player;

    public Transform[] wayPoints;

    public float navigationTargetValidRadius = 1.0f;

    public float attackDistance = 50.0f;

    public float forgetDistance = 150.0f;

    public float gunCoolDownTime = 2.0f;

    [HideInInspector] public int destinationPoint;

    [HideInInspector] public Transform turret;

    public Rigidbody projectile;

    public float shootForce = (float)1e+04;

    public float coolDownTime = 5.0f;

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
        if (this.coolDownState > 0.0f)
        {
            this.coolDownState -= Time.fixedTime;
        }

        this.stateMachine.Update();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Shell"))
        {
            this.Explode();
        }
    }

    public void Shoot()
    {
        var newProjectile = Instantiate(this.projectile, this.shootPoint.transform.position, Quaternion.identity) as Rigidbody;
        newProjectile.gameObject.transform.Rotate(Vector3.right, 90f);
        newProjectile.AddForce(this.shootPoint.transform.forward * this.shootForce);
        this.coolDownState = this.coolDownTime;
    }

    private void Explode()
    {
        Instantiate(this.explosion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject, 0.1f);
    }
}
