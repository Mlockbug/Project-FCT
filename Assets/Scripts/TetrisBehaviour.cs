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
	GameObject[] blocks = new GameObject[4];
	public float gravityDelay, lockDelay;
	public bool clear;

	void Start() {
		grid = new GameObject[width, length];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				grid[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
				grid[i, j].transform.parent = matrix.transform;
				ChangeSprite(i, j);
			}
		}
		spawnSystem = this.gameObject.AddComponent<FullRandomSpawn>();
	}

	void Update() {
		if (mustSpawn) {
			currentPieceIndex = spawnSystem.SpawnNewPiece(12).Item1;
			for (int i = 0; i < 4; i++){
				spawnSystem.SpawnNewPiece(currentPieceIndex).Item2[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = spawnSystem.SpawnNewPiece(currentPieceIndex).Item2[i];
			}
			mustSpawn = false;
			mustDrop = true;
		}
		if (mustDrop) {
			StartCoroutine(Gravity());
		}
	}

	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		clear = true;

		Queue<int> positions = new Queue<int>();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for(int l = 0; l < 4; l++) {
					if (grid[i,j] == blocks[l]) {
						positions.Enqueue(i);
						positions.Enqueue(j);
						if (grid[i, j - 1].tag.Contains("Solid")) {
							clear = false;
						}
					}
				}
			}
		}
		if (clear) {
			for (int i = 0; i < 4; i++) {
				int tempX = positions.Dequeue();
				int tempY = positions.Dequeue();
				ChangeSprite(tempX, tempY);
				grid[tempX, tempY - 1].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX, tempY - 1];
			}
			mustDrop = true;
		}
		else {
			for(int i =0; i <4; i++) {
				blocks[i].tag = "Solid";
			}
			StartCoroutine(LockDelay());
		}
		StopCoroutine(Gravity());
	}

	IEnumerator LockDelay() {
		yield return new WaitForSeconds(lockDelay);
		if (!clear) {
			mustSpawn = true;
		}
		StopCoroutine(LockDelay());
	}

	void ChangeSprite(int cs_width, int cs_length) {
		switch (cs_width) {
			case 0 when cs_length != 0 && cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[15];
				grid[cs_width, cs_length].tag = "Solid_W";
				break;
			case 0 when cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[9];
				grid[cs_width, cs_length].tag = "Solid_W";
				break;
			case 1 when cs_length != 0 && cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[13];
				break;
			case 10 when cs_length != 0 && cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[14];
				break;
			case 11 when cs_length != 0 && cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[16];
				grid[cs_width, cs_length].tag = "Solid_W";
				break;
			case 11 when cs_length < 21:
				grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[10];
				grid[cs_width, cs_length].tag = "Solid_W";
				break;
			default:
				if (cs_length == 0) {
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[7];
					grid[cs_width, cs_length].tag = "Solid";
				}
				else if (cs_length > 20) {
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[8];
					if (cs_width == 0 || cs_width == 11)
						grid[cs_width, cs_length].tag = "Solid_W";
					else
						grid[cs_width, cs_length].tag = "Empty";
				}
				else {
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[11];
					grid[cs_width, cs_length].tag = "Empty";
				}
				break;
		}
	}
}
