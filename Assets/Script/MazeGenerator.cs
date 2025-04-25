using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell cell;

    [SerializeField]
    private int width;

    [SerializeField] 
    private int depth;

    [SerializeField]
    private GameObject chestPrefab;

    [SerializeField]
    private GameObject stairsPrefab;

    [SerializeField]
    private int chestCount = 10;

    [SerializeField]
    private int stairsCount = 5;




    private MazeCell[,] mazeGrid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mazeGrid = new MazeCell[width, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                mazeGrid[x,z] = Instantiate(cell, new Vector3(x,0,z), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0,0]);

        List<MazeCell> deadEnds = GetDeadEnds().OrderBy(_ => Random.value).ToList();

        int totalSpawnCount = Mathf.Min(chestCount + stairsCount, deadEnds.Count);
        int actualChestCount = Mathf.Min(chestCount, totalSpawnCount);
        int actualStairsCount = Mathf.Min(stairsCount, totalSpawnCount - actualChestCount);

        SpawnChest(deadEnds.Take(chestCount).ToList());
        SpawnStairs(deadEnds.Skip(chestCount).Take(stairsCount).ToList());

        if (chestCount + stairsCount > deadEnds.Count)
        {
            Debug.LogWarning($"Requested {chestCount + stairsCount} spawns but only {deadEnds.Count} dead ends available. Some spawns were skipped.");
        }


    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1,10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < width)
        {
            var cellToRight = mazeGrid[x + 1,z];

            if (cellToRight.IsVisited == false) { yield return cellToRight; }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false) { yield return cellToLeft; }
        }

        if (z + 1 < depth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false) { yield return cellToFront; }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false) { yield return cellToBack; }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) { return; }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void SpawnChest(List<MazeCell> chestCells)
    {
        foreach (var cell in chestCells)
        {
            Vector3 position = cell.transform.position + new Vector3(0, 0, 0);
            Instantiate(chestPrefab, position, Quaternion.Euler(270, 270, 0));
        }
    }

    private void SpawnStairs(List<MazeCell> stairsCells)
    {
        foreach (var cell in stairsCells)
        {
            Vector3 position = cell.transform.position + new Vector3(0, 0, 0);
            Instantiate(stairsPrefab, position, Quaternion.Euler(-90, 0, 0));
        }
    }

    private List<MazeCell> GetDeadEnds()
    {
        List<MazeCell> deadEnds = new List<MazeCell>();

        foreach (var cell in mazeGrid)
        {
            if (cell.IsVisited && cell != mazeGrid[0, 0] && cell.GetOpenWallCount() == 1)
            {
                deadEnds.Add(cell);
            }
        }

        return deadEnds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
