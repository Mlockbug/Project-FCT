using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject[] tetrominoes, tempTetrominoes;
	Rigidbody2D currentPiece;
	GameObject tempPiece;
	Queue<GameObject> bag, nextPieces;
	bool mustDrop;
	float gravityWait;
	int count;
	void Start() {

	}

	void Update() {
		if (bag == null) {
			tempTetrominoes = tetrominoes;
			while (count  < 7) {
				int randomTemp = Random.Range(0, 7);
				if (tempTetrominoes[randomTemp] != null) {
					bag.Enqueue(tempTetrominoes[randomTemp]);
					tempTetrominoes[randomTemp] = null;
					count++;
				}
			}
			count = 0;
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

	void SpawnNewPiece() {
		tempPiece = bag.Dequeue();
		nextPieces.Dequeue();
	}
}
