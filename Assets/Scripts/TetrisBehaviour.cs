using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBehaviour : MonoBehaviour
{
    int width = 12;
    int length = 21;

    GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[width, length];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++) {
                grid[i, j] = Instantiate(gameObject, new Vector3(i, j, 0), Quaternion.identity);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
