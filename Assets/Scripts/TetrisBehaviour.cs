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
	bool mustDrop, canMove;
	SpawnSystem spawnSystem;
	int currentPieceIndex = 10;
	GameObject[] blocks = new GameObject[4];
	public float gravityDelay, lockDelay;
	public bool justMoved;
	int rotationState = 0;

	void Start() {
		grid = new GameObject[width, length];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				grid[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
				grid[i, j].transform.parent = matrix.transform;
				ChangeSprite(i, j);
			}
		}
		spawnSystem = this.gameObject.AddComponent<SpawnSystem>();
	}

	void Update() {
		if (mustSpawn) {
			rotationState = 0;
			if (currentPieceIndex == 10)
				currentPieceIndex = spawnSystem.ChoseNextPiece(1, true);
			else
				currentPieceIndex = spawnSystem.ChoseNextPiece(1, false);
			for (int i = 0; i < 4; i++){
				spawnSystem.GetSpawnArea(currentPieceIndex)[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = spawnSystem.GetSpawnArea(currentPieceIndex)[i];
			}
			mustSpawn = false;
			mustDrop = true;
			canMove = true;
		}
		if (mustDrop) {
			StartCoroutine(Gravity());
		}
		if (canMove && Input.GetAxisRaw("Horizontal") != 0) {
			StartCoroutine(MovePiece(Input.GetAxisRaw("Horizontal")));
		}
		for (int i = 0; i < 4; i++) {
			blocks[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
		}
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow)) {
			RotatePositive();
		}
		if (Input.GetKeyDown(KeyCode.Z)) {
			RotateNegative();
		}
	}

	(bool, Queue<int>, Stack<int>) CheckPositions(int xChange, int yChange, bool movement) {
		bool clear = false;
		Queue<int> q_positions = new Queue<int>();
		Stack<int> s_positions = new Stack<int>();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for (int l = 0; l < 4; l++) {
					if (movement) {
						if (Input.GetAxisRaw("Horizontal") < 0) {
							q_positions.Enqueue(i);
							q_positions.Enqueue(j);
						}
						else {
							s_positions.Push(j);
							s_positions.Push(i);
						}
					}
					else {
						q_positions.Enqueue(i);
						q_positions.Enqueue(j);
					}
					if (grid[i, j] == blocks[l]) {
						if (grid[i + xChange, j + yChange].tag.Contains("Solid")) {
							clear = false;
						}
					}
				}
			}
		}
		return (clear,q_positions,s_positions);
	}
	void RotatePositive() {
		switch (currentPieceIndex, rotationState) {
			case (0,0):
				break;
		}
	}
	void RotateNegative() {

	}
	void WallKick() {

	}

	IEnumerator MovePiece(float direction) {
		int tempX, tempY;
		canMove = false;
		yield return new WaitForSeconds(0.05f);

		(bool, Queue<int>, Stack<int>) data = CheckPositions((int)direction, 0, true);
		
		if (data.Item1) {
			for (int i = 0; i < 4; i++) {
				if (direction < 0) {
					tempX = data.Item2.Dequeue();
					tempY = data.Item2.Dequeue();
				}
				else {
					tempX = data.Item3.Pop();
					tempY = data.Item3.Pop();
				}
				ChangeSprite(tempX, tempY);
				grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY];
			}
			justMoved = true;
		}
		canMove = true;
	}

	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		g_clear = true;

		Queue<int> positions = new Queue<int>();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for(int l = 0; l < 4; l++) {
					if (grid[i,j] == blocks[l]) {
						positions.Enqueue(i);
						positions.Enqueue(j);
						if (grid[i, j - 1].tag.Contains("Solid")) {
							g_clear = false;
						}
					}
				}
			}
		}
		if (g_clear) {
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
			StartCoroutine(LockDelay());
		}
		StopCoroutine(Gravity());
	}

	IEnumerator LockDelay() {
		yield return new WaitForSeconds(lockDelay);
		if (!g_clear && !justMoved) {
			for (int i = 0; i < 4; i++) {
				blocks[i].tag = "Solid";
			}
			mustSpawn = true;
		}
		else {
			justMoved = false;
			mustDrop = true;
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
