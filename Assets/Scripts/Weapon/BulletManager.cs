using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] private GameObject Bullet_red;
    [SerializeField] private GameObject Bullet_blue;
    [SerializeField] private GameObject Bullet_green;

    Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(3);
        }
    }

    void SetWeapon(int weaponID)
    {
        switch(weaponID)
        {
            case 1:
                weapon.SetBulletPrefab(Bullet_red);
                break;
            case 2:
                weapon.SetBulletPrefab(Bullet_blue);
                break;
            case 3:
                weapon.SetBulletPrefab(Bullet_green);
                break;
        }
    }
}
