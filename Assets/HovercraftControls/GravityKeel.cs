using UnityEngine;
using System.Collections;

public class GravityKeel : MonoBehaviour {
	
	public		float		selfRightRate		=	.25f;	//	deg/sec of self-righting correction
	public		float		selfRightAltitude	=	9;		//	meters, above surface start auto-levelling
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	private		float		_gravity;
	
	
	void Start () {
		_transform	=	transform;
		_rigidbody	=	rigidbody;
		_gravity	=	Physics.gravity.magnitude;
	}
	
	void LateUpdate () {
		if (!Physics.Raycast(_transform.position, Vector3.down, selfRightAltitude)) {
			SelfRightKinematic();
		}
	}
	
	//	Slowly rotate the craft so it is rightside up.
	//	Make no other changes to its orientation.
	//	This only occurs when the craft recieves no control input.
	private void SelfRightKinematic () {
		Quaternion	goalRotation;
		
		goalRotation		=	Quaternion.LookRotation (_transform.TransformDirection(Vector3.up) - _transform.position);
		goalRotation		=	Quaternion.Euler (0, _transform.rotation.eulerAngles.y, 0);
		_transform.rotation	=	Quaternion.Slerp (_transform.rotation, goalRotation, selfRightRate * Time.deltaTime);
	}
	
}
