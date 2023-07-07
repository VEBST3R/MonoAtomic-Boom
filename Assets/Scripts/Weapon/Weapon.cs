using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public float attackCooldown;
    public float cooldownTimer = Mathf.Infinity;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && cooldownTimer > attackCooldown && characterController2D.m_Grounded)
        {
            Shoot();
            animator.SetBool("IsShooting", true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("IsShooting", false);
        }

        cooldownTimer += Time.deltaTime;
    }

    public void SetBulletPrefab(GameObject newBullet)
    {
        bulletPrefab = newBullet;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        cooldownTimer = 0;
    }

}
