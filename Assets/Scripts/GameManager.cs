using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject[] tetrominoes;
	public Transform[] nextPieceSpawns;
	GameObject[] tempTetrominoes;
	Rigidbody2D currentPiece;
	GameObject tempPiece, r_tempBlock;
	Queue<GameObject> bag = new Queue<GameObject>();
	GameObject[] nextPieces = new GameObject[5];
	bool mustDrop, refillBag;
	float gravityWait;
	int count;
	void Start() {
		FillBag(true);
	}

	void Update() {
		if (bag.Count == 0) {
			FillBag(false);
		}
		if (currentPiece == null) {
			SpawnNewPiece();
		}
		if (mustDrop) {
			StartCoroutine(Gravity());
		}
	}

	IEnumerator Gravity() {
		yield return new WaitForSeconds(gravityWait);
		if (!mustDrop) {
			currentPiece.MovePosition(new Vector2(0f, -1f));
		}
		StopCoroutine(Gravity());
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
			for (int i = 0; i< 5; i++) {
				nextPieces[i] = bag.Dequeue();
				RefreshNextPieces();
			}
		}
	}
	void SpawnNewPiece() {
		
	}

	void RefreshNextPieces() {
		for (int i = 0; i<5; i++) {
			r_tempBlock = Instantiate(nextPieces[i], nextPieceSpawns[i].position, nextPieceSpawns[i].rotation);
			if (r_tempBlock.name.Contains("O-") || r_tempBlock.name.Contains("I-")){
				r_tempBlock.transform.position += new Vector3(0, 0.5f, 0);
			}
		}
	}
}
