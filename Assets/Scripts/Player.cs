using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int damage = 20;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int attackRange = 1;
    public float restartLevelDelay = 1f;
    public Text healthText;
    public LayerMask enemyLayer;
    
    private bool facingLeft = false;
    private bool checkDirection;
    private Animator animator;
    private int health;
    
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        health = GameManager.instance.playerHealth;

        Debug.Log("P: " + health);

        healthText.text = "Health" + health;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerHealth = health;
    }

    void Update()
    {

        checkDirection = facingLeft;
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Enemy>(horizontal, vertical);

        if(horizontal < 0)
        {
            facingLeft = true;
        }

        if(horizontal > 0)
        {
            facingLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        if(facingLeft != checkDirection)
        {
            Flip();
        }

    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void Attack()
    {
        healthText.text = "Health: " + health;

        animator.SetTrigger("playerChop");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                Enemy hitEnemy = enemy.gameObject.GetComponent<Enemy>();
                hitEnemy.LoseHealth(damage);

                Debug.Log("Hit enemy " + hitEnemy.health);
            }
            else if (enemy.gameObject.tag == "Ball")
            {
                EnemyBall hitEnemyBall = enemy.gameObject.GetComponent<EnemyBall>();
                hitEnemyBall.LoseHealth(damage);

                Debug.Log("Hit enemy ball " + hitEnemyBall.health);
            }

        }

        CheckIfGameOver();

        //GameManager.instance.playersTurn = false;


    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        healthText.text = "Health: " + health;

        RaycastHit2D hit;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();

        //GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if(other.tag == "Food")
        {
            health += pointsPerFood;
            healthText.text = "+" + pointsPerFood + "Health: " + health;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoseHealth(int loss)
    {
        animator.SetTrigger("playerHit");
        health -= loss;
        healthText.text = "-" + loss + " Health: " + health;
        Debug.Log("Hit!");
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            health = 100;
            GameManager.instance.GameOver();
            
        }
            
    }
}
