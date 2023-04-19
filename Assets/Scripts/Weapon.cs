using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public int damage = 20;
    public GameObject hitImpact;

    private Animator animator;

        void Start()
    {
        animator = GetComponent<Animator>();

    }
        void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        animator.SetTrigger("GunShoot");

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                Enemy hitEnemy = hitInfo.transform.gameObject.GetComponent<Enemy>();
                hitEnemy.GetShot(damage);

                Instantiate(hitImpact, hitInfo.point, Quaternion.identity);

                Debug.Log("Shot enemy " + hitEnemy.health);
            }
        }
    }
}