using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBehaviour : MonoBehaviour {
	int width = 12;
	int length = 25;

	public GameObject[,] grid = new GameObject[12,25];
	public GameObject tile, matrix;
	public Sprite[] sprites;
	
	public 

	void Start() {
		grid = new GameObject[width, length];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				grid[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
				grid[i, j].transform.parent = matrix.transform;
				switch (i) {
					case 0 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[15];
						break;
					case 0 when j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[9];
						break;
					case 1 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[13];
						break;
					case 10 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[14];
						break;
					case 11 when j != 0 && j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[16];
						break;
					case 11 when j < 21:
						grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[10];
						break;
					default:
						if (j == 0)
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[7];
						else if (j > 20)
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[8];
						else
							grid[i, j].GetComponent<SpriteRenderer>().sprite = sprites[11];
						break;
				}
			}
		}
		this.gameObject.AddComponent<FullRandomSpawn>();
	}

	void Update() {

	}
}
