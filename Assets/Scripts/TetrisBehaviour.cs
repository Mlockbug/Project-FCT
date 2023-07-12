using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TetrisBehaviour : MonoBehaviour {
	int width = 14;
	int length = 25;

	public GameObject[,] grid = new GameObject[14,25];
	public GameObject tile, matrix;
	public Sprite[] sprites;
	bool mustSpawn = true;
	bool mustDrop, canMove, inPlacement;
	SpawnSystem spawnSystem;
	int currentPieceIndex = 10;
	GameObject[] blocks = new GameObject[4];
	public float gravityDelay, lockDelay;
	int rotationState = 0;
	Vector2Int[,,] rotationOfset;
	Vector2Int[,] wallKickOfset, i_wallKickOfset;
	int[] positionsX = new int[8]; 
	int[] positionsY = new int[8];
	Coroutine gravity;
	bool dead;
	GameObject[] ghostBlocks = new GameObject[4];
	int heldPiece = 11;
	bool gh_hitFloor, justHeld, holdDelay;

	void Start() {
		rotationOfset = new Vector2Int[7, 8, 4] { { {new Vector2Int(2, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -2) }, //I piece
												  { new Vector2Int(-2, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 2) },
												  { new Vector2Int(1, -2), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-2, 1) },
												  { new Vector2Int(-1, 2), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, -1) },
												  { new Vector2Int(-2, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 2) },
												  { new Vector2Int(2, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -2) },
												  { new Vector2Int(-1, 2), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, -1) },
												  { new Vector2Int(1, -2), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-2, 1) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(2, 0) },  //J piece
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(-2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, 2) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(-2, 0) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(2, 0) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(0, -2) },  //L piece
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(-2, 0) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(2, 0) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(-2, 0) } },
												{ { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },  //O piece
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) },
												  { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, -2) },  //S piece
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-2, 0) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 0) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, 2) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(0, -2) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 0) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-2, 0) } },
												{ { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },  //T piece
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(-1, -1), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(-1, 1), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(1, -1), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) } },
												{ { new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },  //Z piece
												  { new Vector2Int(-2, 0), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(0, -2), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) },
												  { new Vector2Int(0, 2), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(-2, 0), new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1) },
												  { new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(1, -1), new Vector2Int(-1, -1) },
												  { new Vector2Int(0, 2), new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) },
												  { new Vector2Int(0, -2), new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(-1, 1) } } };
		wallKickOfset = new Vector2Int[8, 4] { { new Vector2Int(-1,0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) },
											   { new Vector2Int(1,0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) },
											   { new Vector2Int(1,0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) },
											   { new Vector2Int(-1,0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2)},
											   { new Vector2Int(1,0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2)},
											   { new Vector2Int(-1,0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2)},
											   { new Vector2Int(-1,0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2)},
											   { new Vector2Int(1,0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2)}};
		i_wallKickOfset = new Vector2Int[8, 4] { { new Vector2Int(-2,0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) },
											   { new Vector2Int(2,0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) },
											   { new Vector2Int(-1,0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) },
											   { new Vector2Int(1,0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1)},
											   { new Vector2Int(2,0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2)},
											   { new Vector2Int(-2,0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2)},
											   { new Vector2Int(1,0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1)},
											   { new Vector2Int(-1,0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1)} };
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
			gravityDelay = 0.5f;
			lockDelay = 1f;
			rotationState = 0;
			if (currentPieceIndex == 10)
				currentPieceIndex = spawnSystem.ChoseNextPiece(1, true);
			else if (!justHeld)
				currentPieceIndex = spawnSystem.ChoseNextPiece(1, false);
			for (int i = 0; i < 4; i++){
				spawnSystem.GetSpawnArea(currentPieceIndex)[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				if (spawnSystem.GetSpawnArea(currentPieceIndex)[i].tag.Contains("Solid")) {
					dead = true;
				}
				blocks[i] = spawnSystem.GetSpawnArea(currentPieceIndex)[i];
				ghostBlocks[i] = blocks[i];
			}
			mustSpawn = false;
			mustDrop = true;
			inPlacement= false;
			canMove = true;
			gh_hitFloor= false;
			justHeld = false;
		}
		if (mustDrop) {
			gravity = StartCoroutine(Gravity());
		}
		if (!gh_hitFloor) {
			Queue<int> positions = CheckPositions(0, 0, "GH").Item2;
			for (int i = 0; i < 4; i++) {
				int tempX = positions.Dequeue();
				int tempY = positions.Dequeue();
				ChangeSprite(tempX, tempY);
				ghostBlocks[i] = blocks[i];

			}
			
			while (!gh_hitFloor) {
				(bool gh_clear2, Queue<int> positions2) = CheckPositions(0, -1, "GH");
				if (gh_clear2) {
					for (int i = 0;i<4; i++) {
						ghostBlocks[i].tag = "EmptyGH";
					}
					for (int i = 0; i < 4; i++) {
						int tempX = positions2.Dequeue();
						int tempY = positions2.Dequeue();
						ChangeSprite(tempX, tempY);
						/*if (grid[tempX, tempY-1].tag.Contains("GH")) {
							tempY++;
						}*/
						grid[tempX, tempY-1].GetComponent<SpriteRenderer>().sprite = sprites[17];
						ghostBlocks[i].tag = "Empty";
						ghostBlocks[i] = grid[tempX, tempY-1];
						
						ghostBlocks[i].name = i.ToString();
					}
				}
				else {
					gh_hitFloor = true;
					for (int i = 0; i < 4; i++) {
						ghostBlocks[i].tag = "EmptyGH";
					}
				}
				for (int i = 0; i < width; i++) {
					for (int j = 0; j < length; j++) {
						if (grid[i, j].tag.Contains("GH")) {
							grid[i,j].GetComponent<SpriteRenderer>().sprite = sprites[17];
						}
					}
				}

			}
		}

		for (int i = 0; i < 4; i++) {
			if (blocks[i] != null) {
				blocks[i].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
			}
		}
		if (!inPlacement) {
			if (Input.GetKeyDown(KeyCode.C) && !holdDelay) {
				holdDelay = true;
				StopCoroutine(gravity);
				if (heldPiece != 11) {
					justHeld = true;
				}
				inPlacement = true;
				int temp = currentPieceIndex;
				currentPieceIndex = heldPiece;
				heldPiece = temp;
				
				mustSpawn = true;
				Queue<int> positions = CheckPositions(0, 0, "GH").Item2;
				for (int i = 0; i < 4; i++) {
					int tempX = positions.Dequeue();
					int tempY = positions.Dequeue();
					ChangeSprite(tempX, tempY);
				}
				Queue<int> positions2 = CheckPositions(0, 0, null).Item2;
				for (int i = 0; i < 4; i++) {
					int tempX = positions2.Dequeue();
					int tempY = positions2.Dequeue();
					grid[tempX, tempY].tag = "Empty";
					ChangeSprite(tempX, tempY);
				}
				spawnSystem.ShowHold(heldPiece);
			}
			if (canMove && Input.GetAxisRaw("Horizontal") != 0) {
				StartCoroutine(MovePiece(Input.GetAxisRaw("Horizontal")));
			}
			if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow)) {
				RotatePositive();
			}
			if (Input.GetKeyDown(KeyCode.Z)) {
				RotateNegative();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				StopCoroutine(gravity);
				gravityDelay = 0.05f;
				lockDelay = 0.25f;
				mustDrop = true;
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow)) {
				StopCoroutine(gravity);
				gravityDelay = 0.5f;
				mustDrop = true;
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				inPlacement = true;
				StopCoroutine(gravity);
				gravityDelay = 0;
				lockDelay = 0;
				mustDrop = true;
			}
		}
	}

	IEnumerator ClearLines(){
		for (int i = 0; i<4; i++) {
			blocks[i] = null;
		}
		for (int i = 1; i < 21; i++) {
			bool full = true;
			for (int j = 0; j < width; j++) {
				if (!grid[j, i].tag.Contains("Solid")) {
					full = false;
				}
			}
			if (full) {
				for (int k = 0; k < width; k++) {
					for (int l = i; l < 21; l++) {
						grid[k, l].name = grid[k, l + 1].name;
						grid[k, l].tag = grid[k, l + 1].tag;
						ChangeSprite(k, l);
					}
				}
				i--;
			}
		}
		yield return new WaitForSeconds(0.2f);
		mustSpawn = true;
    }

	(bool, Queue<int>) CheckPositions(int xChange, int yChange, string condition) {
		bool clear = true;
		Queue<int> q_positions = new Queue<int>();
		int blocksFound = 0;
		GameObject[] cp_blocks;
		if (condition == "GH") {
			cp_blocks = ghostBlocks;
		}
		else {
			cp_blocks = blocks;
		}
		while (blocksFound < 4) {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < length; j++) {
					if (blocksFound != 4 && grid[i, j] == cp_blocks[blocksFound]) {
						q_positions.Enqueue(i);
						q_positions.Enqueue(j);
						try {
							if (condition == "rotate") {
								if (grid[i + rotationOfset[xChange, yChange, blocksFound].x, j + rotationOfset[xChange, yChange, blocksFound].y].tag.Contains("Solid")) {
									clear = false;
								}
							}
							/*else if (condition == "GH"&& (grid[i + xChange, j + yChange].tag.Contains("Solid")|| grid[i + xChange, j + yChange].tag.Contains("GH"))) {
								clear = false;
							}*/
							else if (grid[i + xChange, j + yChange].tag.Contains("Solid")) {
								clear = false;
							}
							blocksFound++;
						}
						catch (IndexOutOfRangeException) {
							clear = false;
							blocksFound++;
							continue;
						}
					}
				}
			}
		}
		return (clear,q_positions);
	}
	void RotatePositive() {
		gh_hitFloor= false;
		int rotationIndex = 0;
		bool wallKicked = false;
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
		(bool, Queue<int>) p_data= CheckPositions(currentPieceIndex, rotationIndex, "rotate");
		for (int i = 0; i < 4; i++) {
			int tempX = p_data.Item2.Dequeue(); positionsX[i] = tempX;
			int tempY = p_data.Item2.Dequeue(); positionsY[i] = tempY;
			ChangeSprite(tempX, tempY);
			grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
			blocks[i] = grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y];
		}
		if (!p_data.Item1) {
			wallKicked = WallKick(rotationIndex);
			if (!wallKicked) {
				for (int i = 0; i< 4;i++) {
					ChangeSprite(positionsX[i] + rotationOfset[currentPieceIndex, rotationIndex, i].x, positionsY[i] + rotationOfset[currentPieceIndex, rotationIndex, i].y);
					grid[positionsX[i], positionsY[i]].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
					blocks[i] = grid[positionsX[i], positionsY[i]];
				}
			}
		}
		if (p_data.Item1 || wallKicked)
			rotationState++;
		if (rotationState == 4)
			rotationState = 0;
	}
	void RotateNegative() {
		gh_hitFloor = false;
		int rotationIndex = 0;
		bool wallKicked = false;
		switch (rotationState) {
			case 0:
				rotationIndex = 7;
				break;
			case 1:
				rotationIndex = 1;
				break;
			case 2:
				rotationIndex = 3;
				break;
			case 3:
				rotationIndex = 5;
				break;
		}
		(bool, Queue<int>) n_data = CheckPositions(currentPieceIndex, rotationIndex, "rotate");
		for (int i = 0; i < 4; i++) {
			int tempX = n_data.Item2.Dequeue(); positionsX[i] = tempX;
			int tempY = n_data.Item2.Dequeue(); positionsY[i] = tempY;
			ChangeSprite(tempX, tempY);
			grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
			blocks[i] = grid[tempX + rotationOfset[currentPieceIndex, rotationIndex, i].x, tempY + rotationOfset[currentPieceIndex, rotationIndex, i].y];
		}
		if (!n_data.Item1) {
			wallKicked = WallKick(rotationIndex);
			if (!wallKicked) {
				for (int i = 0; i < 4; i++) {
					ChangeSprite(positionsX[i] + rotationOfset[currentPieceIndex, rotationIndex, i].x, positionsY[i] + rotationOfset[currentPieceIndex, rotationIndex, i].y);
					grid[positionsX[i], positionsY[i]].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
					blocks[i] = grid[positionsX[i], positionsY[i]];
				}
			}
		}
		if (n_data.Item1 || wallKicked)
			rotationState--;
		if (rotationState == -1) 
			rotationState = 3;
	}
	bool WallKick(int rotation) {
		(bool, Queue<int>) wk_data = (false, new Queue<int>());
		bool cleared = false;
		Vector2Int[,] wallKickData;
		switch (currentPieceIndex) {
			case 0:
				wallKickData = i_wallKickOfset;
				break;
			case 3:
				wallKickData = null;
				break;
			default:
				wallKickData = wallKickOfset;
				break;
		}
		for (int i = 0; i < 4; i++) {
			if (!cleared) {
				wk_data = CheckPositions(wallKickData[rotation, i].x, wallKickData[rotation, i].y, "WK");
				if (wk_data.Item1) {
					for (int j = 0; j < 4; j++) {
						int tempX = wk_data.Item2.Dequeue();
						int tempY = wk_data.Item2.Dequeue();
						ChangeSprite(tempX, tempY);
						grid[tempX + wallKickData[rotation, i].x, tempY + wallKickData[rotation, i].y].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
						blocks[j] = grid[tempX + wallKickData[rotation, i].x, tempY + wallKickData[rotation, i].y];
					}
					cleared = true;
				}
			}
		}
		return cleared;
	}

	IEnumerator MovePiece(float direction) {
		gh_hitFloor = false;
		int tempX, tempY;
		canMove = false;
		yield return new WaitForSeconds(0.05f);

		(bool m_clear, Queue<int> q_positions) = CheckPositions((int)direction, 0, "move");
		
		if (m_clear) {
			for (int i = 0; i < 4; i++) {
				tempX = q_positions.Dequeue();
				tempY = q_positions.Dequeue();
				ChangeSprite(tempX, tempY);
				grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY].GetComponent<SpriteRenderer>().sprite = sprites[currentPieceIndex];
				blocks[i] = grid[tempX + (int)Input.GetAxisRaw("Horizontal"), tempY];
			}
			if (CheckPositions(0, -1, null).Item1) {
				StopCoroutine(LockDelay());
			}
		}
		canMove = true;
	}

	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		(bool g_clear, Queue<int> positions) = CheckPositions(0, -1, null);
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
		float timer = 0;
		while (CheckPositions(0, -1, null).Item1 && timer < lockDelay){
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
        }
		if (dead) {
			SceneManager.LoadScene(0);
		}
		bool clear = CheckPositions(0, -1, null).Item1;
		if (!clear) {
			inPlacement = true;
			for (int i = 0; i < 4; i++) {
				blocks[i].tag = "Solid_";
				blocks[i].name = currentPieceIndex.ToString();
			}
			StartCoroutine(ClearLines());
			holdDelay = false;
			
		}
		else {
			mustDrop = true;
		}
	}

	void ChangeSprite(int cs_width, int cs_length) {
		if (grid[cs_width, cs_length].tag.Contains("Solid_")) {
			grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[Convert.ToInt32(grid[cs_width, cs_length].name)];
		}
		else {
			switch (cs_width) {
				case 0:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[8];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				case 1 when cs_length != 0 && cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[15];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				case 1 when cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[9];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				case 2 when cs_length != 0 && cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[13];
					grid[cs_width, cs_length].tag = "Empty";
					break;
				case 11 when cs_length != 0 && cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[14];
					grid[cs_width, cs_length].tag = "Empty";
					break;
				case 12 when cs_length != 0 && cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[16];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				case 12 when cs_length < 21:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[10];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				case 13:
					grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[8];
					grid[cs_width, cs_length].tag = "Solid";
					break;
				default:
					if (cs_length == 0) {
						grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[7];
						grid[cs_width, cs_length].tag = "Solid";
					}
					else if (cs_length > 20) {
						grid[cs_width, cs_length].GetComponent<SpriteRenderer>().sprite = sprites[8];
						if (cs_width == 1 || cs_width == 12)
							grid[cs_width, cs_length].tag = "Solid";
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
}
