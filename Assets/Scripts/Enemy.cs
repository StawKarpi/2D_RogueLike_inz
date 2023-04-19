using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;
    public int health = 100;
    public LayerMask objectsLayer;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    private int attackRange = 1;
    private bool facingRight = false;
    private bool checkDirection;



    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    void Update()
    {
        checkDirection = facingRight;

    

        
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        Attack();

        skipMove = true;
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }


    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) == 0)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //Debug.Log("Player Y: " + target.position.y + " Enemy Y: " + transform.position.y);

        if (xDir > 0)
        {
            this.facingRight = true;
            Debug.Log("Facing left");
        }

        if (xDir < 0)
        {
            this.facingRight = false;
            Debug.Log("Facing right");
        }

        if (facingRight != checkDirection)
        {
            Flip();
        }

        AttemptMove<Wall>(xDir, yDir);
       
    }

    public void LoseHealth(int loss)
    {
        animator.SetTrigger("enemyHit");

        health -= loss;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void GetShot(int loss)
    {
        animator.SetTrigger("enemyShot");

        health -= loss;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Attack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, objectsLayer);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.gameObject.tag == "Player")
            {
                Player player = hitObject.gameObject.GetComponent<Player>();
                if (GameManager.instance.gameOver == false)
                {
                    player.LoseHealth(playerDamage);
                    animator.SetTrigger("enemyAttack");

                }
                
            }
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        /*if (component.tag == "Player")
        {
            Debug.Log("Player detected");
            Player hitPlayer = component as Player;
            if (GameManager.instance.gameOver == false)
            {
                animator.SetTrigger("enemyAttack");

                hitPlayer.LoseHealth(playerDamage);
            }
        } */
            Wall hitWall = component as Wall;
            animator.SetTrigger("enemyAttack");

            hitWall.DamageWall(1);

        
        
    }
}
