using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    /*public int columns;
    public int rows;*/
    public Count wallCount = new Count (5,9);
    public Count foodCount = new Count (1,5);
    public GameObject exit;
    public GameObject boss;
    public GameObject pistol;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] ballTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public int columns, rows;

    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3>();

    void InitialiseList(int columns, int rows)
    {
        gridPositions.Clear();

        for(int i=1; i < columns - 1; i++)
        {
            for(int j=1; j<rows-1; j++)
            {
                 gridPositions.Add(new Vector3(i,j,0f));
            }
        }
            
    }

    void BoardSetup(int columns, int rows)
    {
        boardHolder = new GameObject ("Board").transform;

        for(int i=-1; i < columns + 1; i++)
        {
            for(int j=-1; j<rows+1; j++)
            {
                 GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
                 if(i == -1 || i == columns || j == -1 || j == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                
                GameObject instance = Instantiate(toInstantiate, new Vector3(i,j,0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
        
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range (minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
            Instantiate (tileChoice, randomPosition, Quaternion.identity);
        }
    }


   public void SetupScene(int level)
   {
        if (level < 3)
        {
            Debug.Log(level);
            columns = Random.Range(level + 5, level + 7);
            rows = Random.Range(level + 5, level + 7);
            Debug.Log("c: " + columns + " r: " + rows);
        }
        else if (level > 3 && level != 5)
        {
            columns = Random.Range(level + 3, level + 5);
            rows = Random.Range(level + 3, level + 5);
        }
        else if (level == 5)
        {
            columns = level + 1;
            rows = level + 1;
        }

        BoardSetup(columns, rows);
        InitialiseList(columns, rows);
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        if (level != 5 && level != 10)
        {
            int enemyCount = (int)Mathf.Log(level + 1, 2f);
            LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        }
        if (level == 3)
        {
            Instantiate(pistol, new Vector3(Random.Range(1, columns - 1), 1, 0F), Quaternion.identity);
            //Debug.Log("pistol x: " + pistol.transform.position.x + "pistol y: " + pistol.transform.position.y);
        }
        if (level == 5)
        {
            Instantiate(boss, new Vector3(Random.Range(1, columns - 1), rows - 2, 0F), Quaternion.identity);
        }
        if (level > 5)
        {
            int ballCount = Random.Range(1, level);
            LayoutObjectAtRandom(ballTiles, ballCount, ballCount);
        }
        if (level == 10)
        {
            Instantiate(boss, new Vector3(Random.Range(1, columns - 1), rows - 2, 0F), Quaternion.identity);
        }
    }
}
