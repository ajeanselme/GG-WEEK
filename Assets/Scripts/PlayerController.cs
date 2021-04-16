using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO.Ports;

public class PlayerController : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public Slider healthBar;

    [Header("Movements")]
    public float speed;

    [Range(1, 30)]
    public float jumpVelocity = 5;
    public float fallSpeed = 1f;

    private bool _isGrounded = true;

    Rigidbody2D rb;

    [Header("Fireball")]
    public float fireballManaCost = 2;
    public float fireballSpeed = 1f;
    public GameObject fireballPrefab;

    [Header("Dash")]
    public float dashManaCost = 3f;
    public float dashMultiplier = 3f;
    public float dashTime = 1f;
    private float _dashCooldown;
    private float _currentDashMultiplier = 1f;
    private bool _isDashing = false;


    [Header("Glide")]
    public float glideManaCost = 1;


    ManaManager _manaManager;

    bool _canGlide = true;
    bool _isGliding = false;

    SerialPort stream = new SerialPort("COM4", 9600);


    float _combo = 0;
    List<string> comboList = new List<string>();

    [Header("Wheel")]
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject runeAir;
    public GameObject runeFire;
    public GameObject runeDash;
    public GameObject runeMana;


    private Animator _animator;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _manaManager = GetComponent<ManaManager>();
        _animator = GetComponent<Animator>();

        AddHealth(0);


        stream.ReadTimeout = 0;
        stream.DtrEnable = true;
        stream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Jump(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpellFireball();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SpellDash();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseGem();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            KillPlayer();
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Movement("up");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Movement("right");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Movement("down");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Movement("left");
        }


        if (Input.GetKeyDown(KeyCode.M))
        {
            stream.Close();
            stream = new SerialPort("COM4", 9600);

            stream.ReadTimeout = 0;
            stream.DtrEnable = true;
            stream.Open();

            Debug.Log("reset");
        }

        try
        {
            string s = stream.ReadLine();
            Debug.Log(s);
            Movement(s);
        }
        catch (System.Exception)
        {
        }

        if (_isGliding)
        {
            if (_isGrounded)
            {
                _isGliding = false;
                runeAir.SetActive(false);
            }
            else
            {
                Jump(false);
            }
        }
    }

    bool _alting = false;

    public void Movement(string mov)
    {

        if(!_alting)
        {
            if (mov.Equals("up"))
            {
                if (_isGrounded)
                {
                    Jump(false);
                }
                else
                {
                    _isGliding = true;
                    runeAir.SetActive(true);
                }
            }
            else if (mov.Equals("right"))
            {
                SpellDash();
            }
            else if (mov.Equals("left"))
            {
                Debug.Log("charging");
                _alting = true;
                runeAir.SetActive(false);
                runeDash.SetActive(false);
                wheel1.SetActive(false);
                wheel2.SetActive(true);
            }
            else if (mov.Equals("down"))
            {
                if (_isGliding)
                {
                    _isGliding = false;
                    runeAir.SetActive(false);
                }
            }
        }
        else
        {
            if (mov.Equals("up"))
            {
                UseGem();
            }
            else if (mov.Equals("right"))
            {
                SpellFireball();
            }
            else
            {
                _alting = false;
                runeFire.SetActive(false);
                runeMana.SetActive(false);
                wheel2.SetActive(false);
                wheel1.SetActive(true);
            }
        }


/*        if (Time.time > _combo)
        {
            Debug.Log("reset alt");

            _alting = false;
            runeFire.SetActive(false);
            runeMana.SetActive(false);
            wheel2.SetActive(false);
            wheel1.SetActive(true);
            _combo = Time.time + 3000f;
        }*/

    }


    private void FixedUpdate()
    {

        if (_isDashing)
        {
            if(Time.time > _dashCooldown)
            {
                //end dash
                _currentDashMultiplier = 1f;
                _isDashing = false;
                runeDash.SetActive(false);

            }
        }

        Vector3 Movement = new Vector3(1, 0, 0);

        transform.position += Movement * speed * _currentDashMultiplier * Time.deltaTime;

        Camera.main.transform.position = new Vector3(transform.position.x, 0, Camera.main.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
            _animator.SetBool("isGround", true);
        }
    }


    public void Jump(bool force)
    {
        if (force)
        {
            _isGrounded = true;
            _animator.SetBool("isGround", true);

        }

        if (_isGrounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            _isGrounded = false;
            _canGlide = true;

            _animator.SetBool("isGround", false);

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

            runeFire.SetActive(true);
            StartCoroutine(ResetFireballRune(.5f));

        }
        else
        {
            Debug.Log("Not enough mana");
        }
    }

    void SpellDash()
    {
        if (_manaManager.currentMana > fireballManaCost)
        {
            if (!_isDashing)
            {
                _isDashing = true;
                _currentDashMultiplier = dashMultiplier;
                _dashCooldown = Time.time + dashTime;

                _manaManager.AddMana(-dashManaCost);

                runeDash.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Not enough mana");
        }
    }

    IEnumerator ResetFireballRune(float time)
    {
        yield return new WaitForSeconds(time);

        runeFire.SetActive(false);
    }
    IEnumerator ResetManaRune(float time)
    {
        yield return new WaitForSeconds(time);

        runeMana.SetActive(false);
    }

    void UseGem()
    {
        _manaManager.SetHasGem(false, null, true);

        runeMana.SetActive(true);

        StartCoroutine(ResetManaRune(.5f));
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
        if (SceneManager.GetActiveScene().name.Equals("Level1"))
        {
            SceneManager.LoadScene("DeathScreen1");
        } else
        {
            SceneManager.LoadScene("DeathScreen2");

        }
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
