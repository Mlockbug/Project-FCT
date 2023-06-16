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
			Debug.Log("fdggfhhjgljhlk;fsd");
			StartCoroutine(Gravity());
		}
		if (Input.GetAxisRaw("Vertical") == -1) {
			gravityDelay = 0.2f;
		}
		else
			gravityDelay = 1f;
	}
	IEnumerator Gravity() {
		mustDrop = false;

		yield return new WaitForSeconds(gravityDelay);
		transform.position += (new Vector3(0f, -1f, 0f));
		mustDrop = true;
		StopCoroutine(Gravity());
	}
}
