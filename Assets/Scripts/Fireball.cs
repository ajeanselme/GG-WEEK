using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
        _rb.velocity = transform.right * speed;
    }


    private void DestroyProjectile()
    {
        // Instantiate particles

        Destroy(gameObject);
    }
}
