using UnityEngine;
using System.Collections;

public class GravitySensor : MonoBehaviour {
	
	[SerializeField]
	private		Transform	_gravityTarget;					//	the object towards which this object is attracted
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	[SerializeField]
	private		Vector3		_gravityVector;
	
	void Start () {
		_transform				=	transform;
		_rigidbody				=	_transform.parent.rigidbody;
		_rigidbody.useGravity	=	false;
		collider.isTrigger		=	true;
	}
	
	//	gravity targets ARE always aligned with +Z axis as the "anti" gravity direction
	void FixedUpdate () {
		if (!_gravityTarget) return;
		
		_rigidbody.AddForce (_gravityVector, ForceMode.Acceleration);
	}
	
	//	listeners	//
	
	//	called when entering the trigger of an object with a GravityField class attached
	public void SetGravityTarget (Transform _gravityTarget) {
		this._gravityTarget		=	_gravityTarget;
		SendMessageUpwards ("SetDefaultGravity", false, SendMessageOptions.DontRequireReceiver);
		SendMessageUpwards ("SetPlatformGravityVector", _gravityTarget.TransformDirection(Vector3.down), SendMessageOptions.DontRequireReceiver);
	}
	
	//	called when entering the trigger of an object with a GravityField class attached
	public void SetGravityMagnitude (float gravityMagnitude) {
		this._gravityVector		=	_gravityTarget.TransformDirection(Vector3.down) * gravityMagnitude;
		SendMessageUpwards ("SetPlatformGravityMagnitude", gravityMagnitude, SendMessageOptions.DontRequireReceiver);
	}
	
	//	called when leaving the trigger of an object with a GravityField class attached
	public void UnsetGravityTarget (Transform _oldGravityTarget) {
		if (!_gravityTarget.Equals(_oldGravityTarget)) return;
		
		//	if we get to hear, it means the gravity target that sent this message is the current gravity
		//	target. it also means that a new gravity target has not replaced the current gravity target.
		//	so what that means is that the craft is no longer in any explicit gravity field and we must
		//	let the system know to activate the default gravity.
		
		_gravityTarget	=	null;
		_gravityVector	=	Vector3.up * 9.81f;	//	activate default gravity
		//Debug.Log ("gravity target unset!");
		
		//	let the receiver know we have left all gravity fields
		SendMessageUpwards ("SetDefaultGravity", true, SendMessageOptions.DontRequireReceiver);
	}
}
