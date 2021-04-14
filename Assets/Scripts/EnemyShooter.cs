using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float damage = 1f;
    public float playerHealAmount = 1f;

    public GameObject fireballPrefab;
    public float fireRate = 1f;
    public float fireballSpeed = 1f;

    public Transform shootPoint;

    float _nextFire;

    void Update()
    {
        if(Time.time > _nextFire)
        {
            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, transform.TransformDirection(Vector2.left), 10f);
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.tag.Equals("Player"))
                {
                    _nextFire = Time.time + fireRate;
                    ShootFireball();
                }
            }
        }
    }


    void ShootFireball()
    {

        GameObject _ball = Instantiate(fireballPrefab, transform.position, transform.rotation);
        _ball.GetComponent<Fireball>().owner = gameObject;
        _ball.GetComponent<Fireball>().speed = fireballSpeed;
        _ball.GetComponent<Fireball>().Move(false);

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
            if (collision.otherCollider.name.Equals("ShooterTop"))
            {
                collision.gameObject.GetComponent<PlayerController>().Jump(true);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().AddHealth(-damage);
            }
            Destroy(gameObject);
        }
    }
}
