using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .5f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerHealth = 100;
    public GameObject exit;
    [HideInInspector] public bool playersTurn = true;
    public bool gameOver = false;

    private Text levelText;
    private GameObject levelImage;
    private GameObject retryButton;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    private bool exitOpen = false;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
       
        DontDestroyOnLoad(gameObject);
        
        
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.exitOpen = false;
        instance.level++;
        instance.InitGame();
    }

    public void RestartGame()
    {
        instance.gameOver = false;
        instance.playerHealth = 100;
        Debug.Log("GM: " + instance.playerHealth);
        instance.level = 0;
        instance.playersTurn = true;
        
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        instance.InitGame();

        Time.timeScale = 1;
    }

    void InitGame()
    {
        doingSetup = true;
        enemies.Clear();

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        retryButton = GameObject.Find("RetryButton");
        retryButton.SetActive(false);
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        gameOver = true;
        levelText.text = "You made it to level " + level;
        levelImage.SetActive(true);
        retryButton.SetActive(true);
        //enabled = false;
    }

    void Update()
    {
        if (doingSetup || enemiesMoving)
            return;
        StartCoroutine(MoveEnemies());
        if(enemies.Count <= 0 && !exitOpen)
        {
            Instantiate(exit, new Vector3(Random.Range(1, 9), 9, 0F), Quaternion.identity);
            exitOpen = true;
        }
        Debug.Log(gameOver);
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].health <= 0)
            {
                enemies.RemoveAt(i);
            }
            else if (enemies[i].health > 0)
            {

                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
            
        }

        playersTurn = true;

        enemiesMoving = false;
    }
}
