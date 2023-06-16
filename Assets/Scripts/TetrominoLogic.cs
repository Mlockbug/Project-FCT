using System.Collections;
using UnityEngine;

public class TetrominoLogic : MonoBehaviour {
	float gravityDelay = 2f;
	bool mustDrop = true;
	GameManager manager;
	// Start is called before the first frame update
	void Start() {
		manager = FindObjectOfType<GameManager>();
	}

	// Update is called once per frame
	void Update() {
		if (mustDrop) {
			StartCoroutine(Gravity());
		}
		if (Input.GetAxisRaw("Vertical") < 0) {
			if (gravityDelay != 1f) {
				StopCoroutine(Gravity());
				mustDrop = true;
			}
			gravityDelay = 1f;
		}
		else
			gravityDelay = 2f;
	}
	IEnumerator Gravity() {
		mustDrop = false;
		yield return new WaitForSeconds(gravityDelay);
		transform.position += (new Vector3(0f, -1f, 0f));
		mustDrop = true;
		StopCoroutine(Gravity());
	}
}
