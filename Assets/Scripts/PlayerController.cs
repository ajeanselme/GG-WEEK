using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    [Range(1, 10)]
    public float jumpVelocity = 5;
    public float fallSpeed = 1f;

    private bool _isGrounded = true;

    Rigidbody2D rb;

    public GameObject fireballPrefab;

    public float rotate;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpellFireball();
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
        } else if(rb.velocity.y < 0f && Mathf.Abs(rb.velocity.y) > fallSpeed)
        {
            Debug.Log("plane");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * fallSpeed);
        }
    }


    void SpellFireball()
    {
        GameObject _ball = Instantiate(fireballPrefab, transform.position, transform.rotation);
    }
}
