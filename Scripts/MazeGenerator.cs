
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public Location mazeStart = new Location(0,0);
	private float nextActionTime = 0.0f;
 	public float period = 0.1f;

    Grid levelOne;
    GameObject wallPrefab;

    void Start()
    {
        levelOne = new Grid(mazeWidth, mazeHeight);
        generateMaze(levelOne, mazeStart);

        wallPrefab = Resources.Load<GameObject>("Wall");
        BuildMaze();
    }

 
	void Update () 
	{
		if (Time.time > nextActionTime ) 
		{
			nextActionTime += period;
			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            Debug.Log("Found " + walls.Length + " walls.");
            foreach(GameObject wall in walls)
            {
                Destroy(wall);
            }

            mazeWidth = (int)Random.Range(20, 30);
            mazeHeight = (int)Random.Range(20, 30);
            levelOne = new Grid(mazeWidth, mazeHeight);
            generateMaze(levelOne, mazeStart);
            BuildMaze();		}
	 }
      
 

    void BuildMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Connections currentCell = levelOne.cells[x, y];
                if (levelOne.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                   
                }
            }
        }
    }

    void generateMaze(Level level, Location start)
    {
        Stack<Location> locations = new Stack<Location>();
        locations.Push(start);
        level.startAt(start);

        while (locations.Count > 0)
        {
            Location current = locations.Peek();

            Location next = level.makeConnection(current);
            if (next != null)
            {
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}
