using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform vfxHit;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("Exploding Bullet"))
        {
            bulletRigidbody.isKinematic = true;
            StartCoroutine(WaitForExplode());
        }
        else
        {
            Instantiate(vfxHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
       
    IEnumerator WaitForExplode()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(vfxHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
