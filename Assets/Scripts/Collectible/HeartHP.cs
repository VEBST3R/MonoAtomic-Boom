using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHP : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (HealthManager.health == 5)
            {
                HealthManager.health += 0;
            }
            else
            {
                HealthManager.health++;
            }
            Destroy(gameObject);
        }
    }
}
