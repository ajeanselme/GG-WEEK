using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    [Range(1, 10)]
    public float jumpVelocity = 5;

    private bool _isGrounded = true;

    Rigidbody2D rb;
    Collider2D _collider;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Vector3 Movement = new Vector3(1, 0, 0);

        transform.position += Movement * speed * Time.deltaTime;

        Camera.main.transform.position = new Vector3(transform.position.x, 0, Camera.main.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }


    public void Jump()
    {
        if (_isGrounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            _isGrounded = false;
        }
    }
}
