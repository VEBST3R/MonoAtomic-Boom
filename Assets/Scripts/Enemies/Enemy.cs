using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float patrolWaitTime;
    public GameObject projectile;
    public Transform player;
    public Transform[] patrolPoints;
    public float jumpForce;
    public float groundCheckDistance;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private int currentPatrolIndex;
    private bool isPatrolling = false;
    private bool isChasing = false;
    private bool isRetreating = false;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public Transform firePoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        currentPatrolIndex = 0;
        isPatrolling = true;
    }

    void Update()
    {
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

        if (isPatrolling)
        {
            Patrol();
        }
        else if (isChasing)
        {
            Chase();
        }
        else if (isRetreating)
        {
            Retreat();
        }

        // Додана перевірка часу між вистрілами
        if (isChasing && timeBtwShots <= 0)
        {
            Vector2 dir = player.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(projectile, firePoint.position, rotation);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        if (!isGrounded)
        {
            return;
        }
    }


    void Patrol()
    {
        Vector2 patrolVector = patrolPoints[currentPatrolIndex].position - transform.position;
        float dotProduct = Vector2.Dot(patrolVector.normalized, Vector2.right);

        // якщо кут між patrolVector та вектором, спрямованим вправо, менший за 90 градусів,
        // то ворог повинен бути спрямований праворуч, інакше - ліворуч
        if (dotProduct >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, speed * Time.deltaTime);
        }
        else if (patrolWaitTime > 0)
        {
            patrolWaitTime -= Time.deltaTime;
        }
        else
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0;
            }
            patrolWaitTime = 2f;
        }

        if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
        {
            isChasing = true;
            isPatrolling = false;
            isRetreating = false;
        }
    }



    void Chase()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && (Vector2.Distance(transform.position, player.position) > retreatDistance))
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            isRetreating = true;
            isChasing = false;
        }
    }

    void Retreat()
    {
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, speed * Time.deltaTime);
        }
        else
        {
            isRetreating = false;
            isPatrolling = true;
            isChasing = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
   
}





