using UnityEngine;
using System.Collections;

public class GravityField : MonoBehaviour {
	
	public	float	gravityMagnitude	=	9.81f;	//	m/s per sec
	
	void Start () {
		renderer.enabled	=	false;
	}
	
	void OnTriggerEnter (Collider other) {
		//Debug.Log (transform.parent.gameObject.ToString() + " sending gravity data to foreign object called " + other.gameObject.ToString());
		
		if (other.gameObject.layer != gameObject.layer) return;
		other.gameObject.SendMessage ("SetGravityTarget", transform, SendMessageOptions.DontRequireReceiver);
		other.gameObject.SendMessage ("SetGravityMagnitude", gravityMagnitude, SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTriggerExit (Collider other) {
		//Debug.Log (transform.parent.gameObject.ToString() + " sending 'exit well' info to foreign object called " + other.gameObject.ToString());
		
		other.gameObject.SendMessage ("UnsetGravityTarget", transform, SendMessageOptions.DontRequireReceiver);
	}
}
