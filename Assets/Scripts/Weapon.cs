using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public int damage = 20;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                Enemy hitEnemy = hitInfo.transform.gameObject.GetComponent<Enemy>();
                hitEnemy.LoseHealth(damage);

                Debug.Log("Shot enemy " + hitEnemy.health);
            }
        }
    }
}