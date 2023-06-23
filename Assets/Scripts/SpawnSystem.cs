using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    TetrisBehaviour manager;
    GameObject[,] grid;
    GameObject[,] spawnArea;
    GameObject[] area;
    int[] nextPieces = new int[5];
    GameObject[] nextPieceSpawns = new GameObject[5];
    Vector2[,] nextPieceSpawnsTileLocations;
    Queue<int> bag = new Queue<int>();
    Queue<GameObject> nextPiecesObjects = new Queue<GameObject>();

    void Start() {
		nextPieceSpawnsTileLocations = new Vector2[7, 4] { { new Vector2(-0.5f,0f), new Vector2(-1.5f, 0f), new Vector2(0.5f, 0f), new Vector2(1.5f, 0f) },
                                                           { new Vector2(0f,0f), new Vector2(-1f, 0f), new Vector2(-1f, 1f), new Vector2(1f, 0f) },
                                                           { new Vector2(0f,0f), new Vector2(-1f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0f) },
                                                           { new Vector2(-0.5f,-0.5f), new Vector2(-0.5f, 0.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f) },
                                                           { new Vector2(-1f,-0.5f), new Vector2(1f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, -0.5f) },
                                                           { new Vector2(-1f,-0.5f), new Vector2(0f, -0.5f), new Vector2(0f, 0.5f), new Vector2(1f, -0.5f) },
                                                           { new Vector2(-1f,0.5f), new Vector2(0f, 0.5f), new Vector2(0f, -0.5f), new Vector2(1f, -0.5f) }};
		manager = GameObject.Find("Game manager").GetComponent<TetrisBehaviour>();
        grid = manager.grid;
        spawnArea = new GameObject[,] { { grid[4, 22], grid[5, 22], grid[6,22], grid[7,22]}, { grid[4, 23], grid[5, 23], grid[6, 23], grid[7, 23]} };
        for (int i = 0;  i < 5; i++) {
            nextPieceSpawns[i] = GameObject.Find("Next pieces").transform.GetChild(i).gameObject;
		}
    }

    void Update()
    {

    }

    public int ChoseNextPiece(int spawnMode, bool start) {
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
                if (start) {
                    for (int i = 1; i < 5; i++) {
                        nextPieces[i] = bag.Dequeue();
                    }
                }
                SortNextPieces();
                SpawnNextPieces();
                block = nextPieces[0];
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

    public void SortNextPieces() {
        for (int i = 1; i < 5; i++) {
            nextPieces[i - 1] = nextPieces[i];
		}
        nextPieces[4] = bag.Dequeue();
        SpawnNextPieces();
	}

    public void SpawnNextPieces() {
        while (nextPiecesObjects.Count > 0) {
            Destroy(nextPiecesObjects.Dequeue());
		}
        for (int i = 0; i <5; i++) {
            for (int j = 0; j<4; j++) {
                GameObject temp = Instantiate(manager.tile, nextPieceSpawns[i].transform.position + new Vector3(nextPieceSpawnsTileLocations[nextPieces[i], j].x, nextPieceSpawnsTileLocations[nextPieces[i], j].y, 0), Quaternion.identity);
                nextPiecesObjects.Enqueue(temp);
                temp.GetComponent<SpriteRenderer>().sprite = manager.sprites[nextPieces[i]];
			}
		}
	}
}
