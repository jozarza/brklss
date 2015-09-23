using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class hit : MonoBehaviour {

	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player") {
			audio.Play ();
		}
	}
}
