using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBehaviour : MonoBehaviour {
	int width = 12;
	int length = 25;

	public GameObject[,] grid = new GameObject[12,25];
	public GameObject tile, matrix;
	public Sprite[] sprites;
	bool mustSpawn = true;
	bool mustDrop;
	FullRandomSpawn spawnSystem;
	int currentPieceIndex;
	GameObject[] blocks;
	GameObject block1, block2, block3, block4;
	float gravityDelay;

	void Start() {
		grid = new GameObject[width, length];
		blocks = new GameObject[] { block1, block2, block3, block4 };
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				grid[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
				grid[i, j].transform.parent = matrix.transform;
				switch (i) {
					case 0 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[15];
						grid[i, j].tag = "Solid_W";
						break;
					case 0 when j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[9];
						grid[i, j].tag = "Solid_W";
						break;
					case 1 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[13];
						break;
					case 10 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[14];
						break;
					case 11 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[16];
						grid[i, j].tag = "Solid_W";
						break;
					case 11 when j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[10];
						grid[i, j].tag = "Solid_W";
						break;
					default:
						if (j == 0) {
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[7];
							grid[i, j].tag = "Solid";
						}
						else if (j > 20) {
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[8];
							if (i == 0 || i == 11)
								grid[i, j].tag = "Solid_W";
							else
								grid[i, j].tag = "Empty";
						}
						else {
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[11];
							grid[i, j].tag = "Empty";
						}
						break;
				}
			}
		}
		spawnSystem = this.gameObject.AddComponent<FullRandomSpawn>();
	}

	void Update() {
		if (mustSpawn) {
			currentPieceIndex = spawnSystem.SpawnNewPiece(12).Item1;
			for (int i = 0; i < 4; i++){
				spawnSystem.SpawnNewPiece(currentPieceIndex).Item2[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				spawnSystem.SpawnNewPiece(currentPieceIndex).Item2[i] = blocks[i];
			}
			mustSpawn = false;
		}
		if (mustDrop) {
			StartCoroutine(Gravity());
		}
	}

	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		bool notClear = false;
		foreach(GameObject x in blocks) {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < length; j++) {
					for(int l = 0; l < 4; l++) {
						if (grid[i,j] == blocks[l] && grid[i,j+1].tag != "Empty") {
							notClear = true;
						}
					}
				}
			}
		}
		if (!notClear) {
			//drop
		}
		mustDrop = true;
		StopCoroutine(Gravity());
	}
}
