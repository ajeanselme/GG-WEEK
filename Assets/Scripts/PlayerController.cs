using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public Slider healthBar;

    public float speed;

    [Range(1, 30)]
    public float jumpVelocity = 5;
    public float fallSpeed = 1f;

    private bool _isGrounded = true;

    Rigidbody2D rb;

    public float fireballManaCost = 2;
    public float fireballSpeed = 1f;
    public GameObject fireballPrefab;

    public float glideManaCost = 1;


    ManaManager _manaManager;

    bool _canGlide = true;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _manaManager = GetComponent<ManaManager>();

        AddHealth(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Jump(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpellFireball();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseGem();
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


    public void Jump(bool force)
    {
        if (force) _isGrounded = true;

        if (_isGrounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            _isGrounded = false;
            _canGlide = true;
        } else if(rb.velocity.y < 0f && Mathf.Abs(rb.velocity.y) > fallSpeed)
        {
            if(_manaManager.currentMana > 0 && _canGlide)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * fallSpeed);
                _manaManager.AddMana(-glideManaCost * Time.deltaTime);
            } else
            {
                _canGlide = false;
            }
        }
    }


    void SpellFireball()
    {
        if(_manaManager.currentMana > fireballManaCost)
        {
            GameObject _ball = Instantiate(fireballPrefab, transform.position, transform.rotation);
            _ball.GetComponent<Fireball>().owner = gameObject;
            _ball.GetComponent<Fireball>().speed = fireballSpeed;
            _ball.GetComponent<Fireball>().Move(true);

            _manaManager.AddMana(-fireballManaCost);
        } else
        {
            Debug.Log("Not enough mana");
        }
    }

    void UseGem()
    {
        _manaManager.SetHasGem(false, null);
    }



    public void AddHealth(float value)
    {
        if (currentHealth + value < maxHealth)
        {
            if (currentHealth + value > 0)
            {
                currentHealth += value;
                healthBar.value = currentHealth / maxHealth;
            }
            else
            {
                KillPlayer();
            }

        }
        else
        {
            currentHealth = maxHealth;
            healthBar.value = 1f;

        }
    }


    public void KillPlayer()
    {
        Debug.Log("u ded");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Fireball"))
        {
            if (collision.gameObject.GetComponent<Fireball>().owner.tag.Equals("Shooter"))
            {
                AddHealth(-collision.gameObject.GetComponent<Fireball>().owner.GetComponent<EnemyShooter>().damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
