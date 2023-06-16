using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject[] tetrominoes;
	public Transform[] nextPieceSpawns;
	public Transform mainSpawnPoint;
	GameObject[] tempTetrominoes;
	public GameObject currentPiece;
	GameObject tempPiece, r_tempBlock;
	Queue<GameObject> bag = new Queue<GameObject>();
	Queue<GameObject> tempNextPiece = new Queue<GameObject>();
	GameObject[] nextPieces = new GameObject[5];
	bool mustDrop, refillBag;
	int count;
	void Start() {
		FillBag(true);
	}

	void Update() {
		if (bag.Count == 0) {
			FillBag(false);
		}
		if (currentPiece == null) {
			SpawnNewPiece(false);
		}
	}

	
	void FillBag(bool start) {
		tempTetrominoes = tetrominoes;
		while (count < 7) {
			int randomTemp = Random.Range(0, 7);
			if (tempTetrominoes[randomTemp] != null) {
				bag.Enqueue(tempTetrominoes[randomTemp]);
				tempTetrominoes[randomTemp] = null;
				count++;
			}
		}
		count = 0;
		if (start) {
			SpawnNewPiece(start);
			RefreshNextPieces();
		}
	}
	void SpawnNewPiece(bool start) {
		if (start) {
			currentPiece = Instantiate(bag.Dequeue(), mainSpawnPoint.position, mainSpawnPoint.rotation);
			for (int i = 0; i < 5; i++) {
				nextPieces[i] = bag.Dequeue();
			}
		}
		else {
			for (int i = 0; i < 5; i++) {
				Destroy(tempNextPiece.Dequeue());
			}
			currentPiece = Instantiate(nextPieces[0], mainSpawnPoint.position, mainSpawnPoint.rotation);
			nextPieces[0] = null;
			MoveNextPieces();
		}
		if (currentPiece.name.Contains("O-") || currentPiece.name.Contains("I-")) {
			currentPiece.transform.position += new Vector3(0.5f, 0.5f, 0);
		}
		currentPiece.gameObject.AddComponent<TetrominoLogic>();
	}

	void RefreshNextPieces() {
		tempNextPiece.Clear();
		for (int i = 0; i < 5; i++) {
			r_tempBlock = Instantiate(nextPieces[i], nextPieceSpawns[i].position, nextPieceSpawns[i].rotation);
			if (r_tempBlock.name.Contains("O-") || r_tempBlock.name.Contains("I-")) {
				r_tempBlock.transform.position += new Vector3(0, 0.5f, 0);
			}
			tempNextPiece.Enqueue(r_tempBlock);
		}
	}

	void MoveNextPieces() {
		for (int i = 1; i < 5; i++) {
			nextPieces[i - 1] = nextPieces[i];
		}
		nextPieces[4] = bag.Dequeue();
		RefreshNextPieces();
	}
}
