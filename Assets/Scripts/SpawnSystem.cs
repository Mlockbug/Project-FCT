using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    TetrisBehaviour manager;
    GameObject[,] grid;
    GameObject[,] spawnArea;
    public GameObject[] sprites;
    public GameObject tile;
    GameObject[] area;
    GameObject[] nextPieces;
    GameObject[] nextPieceSpawns;
    Queue<int> bag = new Queue<int>();

    void Start()
    {
        manager = GameObject.Find("Game manager").GetComponent<TetrisBehaviour>();
        grid = manager.grid;
        spawnArea = new GameObject[,] { { grid[4, 22], grid[5, 22], grid[6,22], grid[7,22]}, { grid[4, 23], grid[5, 23], grid[6, 23], grid[7, 23]} };
    }

    
    void Update()
    {
        
    }

    public int ChoseNextPiece(int spawnMode) {
        int block = 0;
        switch (spawnMode) {
            case 0:
                block = Random.Range(0, 7);
                break;
            case 1:
                int count = 0;
                int[] tetrominoesIndex = new int[] {0,1,2,3,4,5,6};
                if (bag.Count == 0) {
                    while (count < 7) {
                        int randomTemp = Random.Range(0, 7);
                        if (tetrominoesIndex[randomTemp] != 10) {
                            bag.Enqueue(tetrominoesIndex[randomTemp]);
                            tetrominoesIndex[randomTemp] = 10;
                            count++;
                        }
                    }
                }
                block = bag.Dequeue();
                break;
        }
        return block;
	}

    public GameObject[] GetSpawnArea(int piece) {
        switch (piece) {
            case 0:
                area = new GameObject[] { spawnArea[0, 0], spawnArea[0, 1], spawnArea[0, 2], spawnArea[0, 3] };
                break;
            case 1:
                area = new GameObject[] { spawnArea[0, 0], spawnArea[0, 1], spawnArea[0, 2], spawnArea[1, 0] };
                break;
            case 2:
                area = new GameObject[] { spawnArea[0, 0], spawnArea[0, 1], spawnArea[0, 2], spawnArea[1, 2] };
                break;
            case 3:
                area = new GameObject[] { spawnArea[0, 1], spawnArea[0, 2], spawnArea[1, 1], spawnArea[1, 2] };
                break;
            case 4:
                area = new GameObject[] { spawnArea[0, 0], spawnArea[0, 1], spawnArea[1, 1], spawnArea[1, 2] };
                break;
            case 5:
                area = new GameObject[] { spawnArea[0, 0], spawnArea[0, 1], spawnArea[1, 1], spawnArea[0, 2] };
                break;
            case 6:
                area = new GameObject[] { spawnArea[1, 0], spawnArea[0, 1], spawnArea[1, 1], spawnArea[0, 2] };
                break;
        }
        return area;
    }

    public void NextPieces() {

	}
}
