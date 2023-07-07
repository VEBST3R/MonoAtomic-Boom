using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int health;

    public GameObject deathEffect;
    public float maxSpeed = 10f;
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public GameObject projectile;
    public Transform player;
    public float jumpForce;
    public float groundCheckDistance;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public Transform firePoint;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private int numEnemiesSpawned = 0;
    private Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (health <= 350)
        {
            SpawnEnemies();

        }
        if (health <= 250)
        {
            anim.SetTrigger("stageTwo");
            SpawnEnemies();
        }
          
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 playerDirection = (player.position - transform.position).normalized;

        if (distanceToPlayer < stoppingDistance)
        {
            speed = Mathf.Lerp(speed, 0, (stoppingDistance - distanceToPlayer) / stoppingDistance);
        }
        else
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime);
        }

        if (distanceToPlayer < retreatDistance)
        {
            timeBtwShots = startTimeBtwShots;

            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            float retreatSpeed = Mathf.Lerp(0, maxSpeed, (retreatDistance - distanceToPlayer) / retreatDistance);
            speed = Mathf.Lerp(speed, retreatSpeed, Time.deltaTime);
            transform.position -= playerDirection * retreatSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += playerDirection * speed * Time.deltaTime;
        }

        if (timeBtwShots <= 0)
        {
            GetComponent<Animator>().SetBool("IsAttacking", true);

            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;

            if (timeBtwShots < startTimeBtwShots / 2)
            {
                GetComponent<Animator>().SetBool("IsAttacking", false);
            }
        }

        if (!isGrounded)
        {
            return;
        }


    }

    public void TakeDamage(int damage)
    {
        
            health -= damage;

            if (health <= 0)
            {
                Die();
                SceneManager.LoadScene(7);
            }
    }
    void SpawnEnemies()
    {
        if (numEnemiesSpawned < 3)
        {
            int randomPrefabIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomPrefabIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
            numEnemiesSpawned++;
        }
    }


    public void Attack()
    {
        Vector2 dir = player.position - firePoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(projectile, firePoint.position, rotation);
        timeBtwShots = startTimeBtwShots;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
