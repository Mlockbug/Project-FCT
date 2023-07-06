using System;
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
	Vector2Int[,,] rotationOfset;

	void Start() {
		rotationOfset = new Vector2Int[7, 8, 4] { { {new Vector2Int(2, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -2) },
												  { new Vector2Int(-2, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 2) },
												  { new Vector2Int(1, -2), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-2, 1) },
												  { new Vector2Int(-1, 2), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 2) },
												  { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -2) },
												  { new Vector2Int(-1, 2), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, -1) },
												  { new Vector2Int(1, -2), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-2, 1) } },
												{ { new Vector2Int(1, 1), new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(-2, 0), new Vector2Int(0, 0), new Vector2Int(1, 1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, -2), new Vector2Int(0, 0), new Vector2Int(-1, 1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 2), new Vector2Int(0, 0), new Vector2Int(1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(-2, 0), new Vector2Int(0, 0), new Vector2Int(1, 1) },
												  { new Vector2Int(1, 1), new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 2), new Vector2Int(0, 0), new Vector2Int(1, -1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, -2), new Vector2Int(0, 0), new Vector2Int(-1, -1) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(-2, 0) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(-2, 0) } },
												{ { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-2, 0) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 0) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-2, 0) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) } },
												{ { new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(-2, 0), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(0, -2), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) },
												  { new Vector2Int(0, 2), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) } } };
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
		if (Input.GetKey(KeyCode.L)) {
			StopCoroutine(Gravity());
			mustDrop = false;
		}
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
			blocks[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
		}
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow)) {
			RotatePositive();
		}
		if (Input.GetKeyDown(KeyCode.Z)) {
			RotateNegative();
		}
	}

	(bool, Queue<int>, Stack<int>) CheckPositions(int xChange, int yChange, string condition) {
		bool clear = true;
		Queue<int> q_positions = new Queue<int>();
		Stack<int> s_positions = new Stack<int>();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for (int l = 0; l < 4; l++) {
					if (grid[i, j] == blocks[l]) {
						if (condition == "move" && xChange> 0) {
							s_positions.Push(j);
							s_positions.Push(i);
						}
						else {
							Debug.Log(i);
							Debug.Log(j);
							q_positions.Enqueue(i);
							q_positions.Enqueue(j);
						}
						if (condition == "rotate") {
							if (grid[i + rotationOfset[xChange,yChange,l].x, j + rotationOfset[xChange,yChange,l].y].tag.Contains("Solid")) {
								clear = false;
							}
						}
						else if(grid[i + xChange, j + yChange].tag.Contains("Solid")) {
							clear = false;
						}
					}
				}
			}
		}
		return (clear,q_positions,s_positions);
	}
	void RotatePositive() {
		int rotationIndex = 0;
		switch (rotationState) {
			case 0:
				rotationIndex = 0;
				break;
			case 1:
				rotationIndex = 2;
				break;
			case 2:
				rotationIndex = 4;
				break;
			case 3:
				rotationIndex = 6;
				break;
		}
		(bool, Queue<int>, Stack<int>) p_data= CheckPositions(currentPieceIndex, rotationIndex, "rotate");
		if (p_data.Item1) {
			for (int i = 0; i < 4; i++) {
				int tempX = p_data.Item2.Dequeue();
				int tempY = p_data.Item2.Dequeue();
				ChangeSprite(tempX, tempY);
				grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y];
			}
			rotationState++;
		}
		if (rotationState == 4) {
			rotationState = 0;
		}
	}
	void RotateNegative() {
		int rotationIndex = 0;
		switch (rotationState) {
			case 0:
				rotationIndex = 7;
				break;
			case 1:
				rotationIndex = 5;
				break;
			case 2:
				rotationIndex = 3;
				break;
			case 3:
				rotationIndex = 1;
				break;
		}
		(bool, Queue<int>, Stack<int>) n_data = CheckPositions(currentPieceIndex, rotationIndex, "rotate");
		if (n_data.Item1) {
			for (int i = 0; i < 4; i++) {
				int tempX = n_data.Item2.Dequeue();
				int tempY = n_data.Item2.Dequeue();
				ChangeSprite(tempX, tempY);
				grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y];
			}
			rotationState--;
		}
		if (rotationState == -1) {
			rotationState = 3;
		}
	}
	void WallKick() {

	}

	IEnumerator MovePiece(float direction) {
		int tempX, tempY;
		canMove = false;
		yield return new WaitForSeconds(0.05f);

		(bool m_clear, Queue<int> q_positions, Stack<int> s_positions) = CheckPositions((int)direction, 0, "move");
		
		if (m_clear) {
			for (int i = 0; i < 4; i++) {
				if (direction < 0) {
					tempX = q_positions.Dequeue();
					tempY = q_positions.Dequeue();
				}
				else {
					tempX = s_positions.Pop();
					tempY = s_positions.Pop();
				}
				ChangeSprite(tempX, tempY);
				grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY];
				if (direction< 0) {
					//Array.Reverse(blocks);
				}
			}
			justMoved = true;
		}
		canMove = true;
	}

	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		(bool g_clear, Queue<int> positions, Stack<int> notUsed) = CheckPositions(0, -1, null);
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
			StartCoroutine(LockDelay(g_clear));
		}
		StopCoroutine(Gravity());
	}

	IEnumerator LockDelay(bool clear) {
		yield return new WaitForSeconds(lockDelay);
		if (!clear && !justMoved) {
			for (int i = 0; i < 4; i++) {
				blocks[i].tag = "Solid";
			}
			mustSpawn = true;
		}
		else {
			justMoved = false;
			mustDrop = true;
		}
		StopCoroutine(LockDelay(clear));
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
