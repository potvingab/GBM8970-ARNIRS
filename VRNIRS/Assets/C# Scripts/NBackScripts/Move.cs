using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    
	// Update is called once per frame
	void Update () {
        float slower = 0.03f;
        transform.Translate(Vector3.forward * VariablesHolder.GameSpeed * VariablesHolder.speed * slower);
    }
}
