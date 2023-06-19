using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullRandomSpawn : MonoBehaviour
{
    TetrisBehaviour manager;
    GameObject[,] grid;
    Sprite[] sprites;
    GameObject[,] spawnArea = new GameObject[4,2];

    void Start()
    {
        manager = GameObject.Find("Game manager").GetComponent<TetrisBehaviour>();
        grid = manager.grid;
        spawnArea = { { grid[4, 23], grid[5, 23], grid[6,23], grid[7,23]}, { grid[4, 23], grid[5, 23], grid[6, 23], grid[7, 23]} };
    }

    
    void Update()
    {
        
    }

    int SpawnNewPiece() {
        int piece = Random.Range(0, 7);
        switch (piece) {
            case 0:
                break;
		}
        return piece;
	}
}
