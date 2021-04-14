using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoomba : MonoBehaviour
{

    public float damage = 1f;
    public float speed = 1f;

    public float playerHealAmount = 1f;


    public GameObject pointA;
    public GameObject pointB;

    private Vector3 _tresholdA;
    private Vector3 _tresholdB;

    bool _movingRight = true;

    private void Start()
    {
        _tresholdA = pointA.transform.position;
        _tresholdB = pointB.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 movement = Vector2.zero;

        if (_movingRight)
        {
            if(transform.position.x < _tresholdB.x)
            {
                movement = new Vector2(1, 0);
            } else
            {
                _movingRight = false;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        } else
        {

            if (transform.position.x > _tresholdA.x)
            {
                movement = new Vector2(-1, 0);
            }
            else
            {
                _movingRight = true;
                GetComponent<SpriteRenderer>().flipX = false;

            }
        }

        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Fireball"))
        {
            if (collision.gameObject.GetComponent<Fireball>().owner.tag.Equals("Player"))
            {
                float random = Random.Range(0f, 1f);
                if (random > 0.5f)
                {
                    collision.gameObject.GetComponent<Fireball>().owner.GetComponent<PlayerController>().AddHealth(playerHealAmount);
                }
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }

            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            if (collision.otherCollider.name.Equals("GoombaTop"))
            {
                collision.gameObject.GetComponent<PlayerController>().Jump(true);
            } else
            {
                collision.gameObject.GetComponent<PlayerController>().AddHealth(-damage);
            }
            Destroy(gameObject);
        }
    }
}
