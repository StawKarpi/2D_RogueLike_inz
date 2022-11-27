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

    

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
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

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Wall>(xDir, yDir);
       
    }

    public void LoseHealth(int loss)
    {
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
            /*else if (hitObject.gameObject.tag == "Wall")
            {
                Debug.Log("Wall collision!");
                Wall wall = hitObject.gameObject.GetComponent<Wall>();
                animator.SetTrigger("enemyAttack");

                wall.DamageWall(1);
            }*/
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
