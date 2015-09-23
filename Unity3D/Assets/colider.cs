using UnityEngine;
using System.Collections;

public class colider : MonoBehaviour {
	int totalCollision = 0;
	// Use this for initialization
	void Start () {
		Debug.Log("begin:" );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "prop_powerCube")
		{

		}
		totalCollision+=1;
		Destroy(col.gameObject);
		Debug.Log("choques:" + totalCollision);
	}
}
