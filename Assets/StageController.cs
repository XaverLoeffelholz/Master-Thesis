using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.N))
        {
            transform.Rotate(0, 1f, 0);
        }
        if (Input.GetKey(KeyCode.M))
        {
            transform.Rotate(0, -1f, 0);
        }
    }
}
