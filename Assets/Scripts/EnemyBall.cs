using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MovingObject
{
    public int playerDamage = 50;
    public int health = 100;
    public LayerMask objectsLayer;
    private Animator animator;

    private int direction = 0;
    private bool skipMove;
    private int attackRange = 1;

    

    protected override void Start()
    {
        GameManager.instance.AddBallToList(this);
        animator = GetComponent<Animator>();
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {

        base.AttemptMove<T>(xDir, yDir);
        Attack();
        Debug.Log("Ball move");
    }

    public void MoveEnemyBall()
    {
        int xDir = 0;
        int yDir = 0;

        switch (direction)
        {
            case 0:
                break;
            case 1: 
                yDir = 1;
                break;
            case 2:
                xDir = 1;
                break;
            case 3:
                yDir = -1;
                break;
            case 4:
                xDir = -1;
                break;



        }

        Debug.Log("Ball X: " + xDir + " Ball Y: " + yDir);

        AttemptMove<Wall>(xDir, yDir);
        ChangeDirection();

    }

    public void LoseHealth(int loss)
    {
        health -= loss;

        if (health <= 0)
        {
            animator.SetTrigger("ballExplode");
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Debug.Log($"Animation over");
                gameObject.SetActive(false);
            }
            
        }
    }

    public void ChangeDirection()
    {
        direction = Random.Range(1, 5);
        Debug.Log("New direction: " + direction);

    }

    public void Attack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, objectsLayer);

        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.gameObject.tag == "Player")
            {
                Player player = hitObject.gameObject.GetComponent<Player>();
                
                player.LoseHealth(playerDamage);

                Debug.Log("Ball hit player ");
                this.LoseHealth(100); 
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
        /*Player hitPlayer = component as Player;
        Debug.Log("Ball-player collision!");
        
        hitPlayer.LoseHealth(playerDamage);
        
            //Wall hitWall = component as Wall;

            //hitWall.DamageWall(1);*/

        
        
    }
}
