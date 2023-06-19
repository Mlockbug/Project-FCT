using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullRandomSpawn : MonoBehaviour
{
    TetrisBehaviour manager;
    GameObject[,] grid;
    GameObject[,] spawnArea;
    GameObject[] area;

    void Start()
    {
        manager = GameObject.Find("Game manager").GetComponent<TetrisBehaviour>();
        grid = manager.grid;
        spawnArea = new GameObject[,] { { grid[4, 22], grid[5, 22], grid[6,22], grid[7,22]}, { grid[4, 23], grid[5, 23], grid[6, 23], grid[7, 23]} };
        SpawnNewPiece();
    }

    
    void Update()
    {
        
    }

    void SpawnNewPiece() {
        int piece = Random.Range(0, 7);
        Debug.Log(piece);
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
        foreach (GameObject x in area) {
            x.GetComponent<SpriteRenderer>().sprite = manager.sprites[piece];
		}
	}
}
