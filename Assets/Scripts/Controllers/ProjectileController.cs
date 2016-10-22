using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    private void OnCollisionEnter(Collision other)
    {
        this.Explode();
    }

    private void Explode()
    {
        Instantiate(this.explosion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject, 0.1f);
    }
}
