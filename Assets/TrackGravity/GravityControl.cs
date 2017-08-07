using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour {

	public		Transform	gravityTarget;					//	the object towards which this object is attracted
	public		float		gravityMagnitude	=	9.81f;	//	m/s per sec
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	private		Vector3		_gravityVector;
	private		float		_maxGravityDistance;
	private		RaycastHit	_raycastData;
	
	void Start () {
		_transform				=	transform;
		_rigidbody				=	rigidbody;
		_rigidbody.useGravity	=	false;
		_maxGravityDistance		=	gravityMagnitude * gravityMagnitude;
	}
	
	void FixedUpdate () {
		if (!gravityTarget) return;
		
		if (Physics.Raycast (_transform.position, _transform.TransformDirection(Vector3.down), out _raycastData, _maxGravityDistance)) {
			_gravityVector		=	-_raycastData.normal * gravityMagnitude * Time.deltaTime;
			Debug.DrawRay (_transform.position, _gravityVector, Color.cyan);
			_rigidbody.AddForce (_gravityVector, ForceMode.Acceleration);
					
		}
	}
	

	//	very basic 'spherical' gravity system
	void XFixedUpdate () {
		if (!gravityTarget) return;
		
		_gravityVector		=	(gravityTarget.position - _transform.position) * gravityMagnitude * Time.deltaTime;
		_rigidbody.AddForce (_gravityVector, ForceMode.Acceleration);
	}
}
