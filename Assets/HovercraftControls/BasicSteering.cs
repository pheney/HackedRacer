using UnityEngine;
using System.Collections;

public class BasicSteering : MonoBehaviour {
	
	public		float		accelerationImpulse			=	1;			//	'gravities'
	public		float		turnImpulse					=	1;			//	'gravities' of torque
	
	public		float		maxForwardVelocity			=	20;			//	m/s
	public		float		maxRotationalVelocity		=	1.5f;		//	m/s
	
	public		float		bankSteeringFactor			=	1;			//	multiple, applied during banking
	public		float		orthogonalDrag				=	1.5f;
	public		float		turnVelocityPreservation	=	2;			//	factor
	
	public		bool		showVelocityVector			=	true;
	public		Color		velocityVectorColor			=	Color.green;
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	private		float		_gravity;
	private		float		_forwardVelocity;
	private		float		_rotationalVelocity;
	
	
	void Start () {
		_transform			=	transform;
		_rigidbody			=	rigidbody;
		_gravity			=	Physics.gravity.magnitude;
		_forwardVelocity	=	0;
		_rotationalVelocity	=	0;
	}
	
	void OnDrawGizmos () {
		if (!showVelocityVector) return;
		
		Gizmos.color	=	velocityVectorColor;
		Gizmos.DrawRay (transform.position, rigidbody.velocity);
	}
	
	void Update () {
		ApplyInput(GetUserInput());
	}
	
	private Vector3 GetUserInput () {
	
		if (Input.GetKey(KeyCode.Space)) {
			return Vector3.zero;
		}
		
		float	hInput	=	Input.GetAxis ("Vertical");
				//hInput	*=	Mathf.Abs(hInput);
		float	vInput	=	Input.GetAxis ("Horizontal");
				//vInput	*=	Mathf.Abs(vInput);
		float	tInput	=	Input.GetAxis("Throttle");
				//tInput	*=	Mathf.Abs(tInput);
		
		return Vector3.right * hInput + Vector3.up * vInput + Vector3.forward * tInput;
	}
	
	void ApplyInput (Vector3 userInput) {
		
		//	z-axis is the throttle control
		if ( (userInput.z > 0 && _forwardVelocity < maxForwardVelocity) ||
			(userInput.z < 0 && _forwardVelocity > -maxForwardVelocity * .25f) ) {
			_rigidbody.AddForce (_transform.forward * accelerationImpulse * _gravity * userInput.z, ForceMode.Acceleration);
		}
		
		//	y-axis is yaw control (left-right)
		if ( (userInput.y > 0 && _rotationalVelocity < maxRotationalVelocity) ||
			 (userInput.y < 0 && _rotationalVelocity > -maxRotationalVelocity) ) {
			_rigidbody.AddTorque (_transform.up * turnImpulse * _gravity * userInput.y, ForceMode.Acceleration);
		}
		
		//	x-axis is pitch control (up-down)
		if ( (userInput.x > 0 && _rotationalVelocity < maxRotationalVelocity) ||
			 (userInput.x < 0 && _rotationalVelocity > -maxRotationalVelocity) ) {
			_rigidbody.AddTorque (_transform.right * turnImpulse * _gravity * userInput.x, ForceMode.Force);
			SendMessage ("AutolevelActive", false);
		} else {
			SendMessage ("AutolevelActive", true);
		}
	}
	
	void FixedUpdate () {
	
		_forwardVelocity	=	_transform.InverseTransformDirection(_rigidbody.velocity).x;
		_rotationalVelocity	=	_transform.InverseTransformDirection(_rigidbody.angularVelocity).y;
		
		//	pitch nose downward in direction of the banking angle
//		float	_roll	=	Vector3.Dot(_transform.TransformDirection(Vector3.left), Vector3.up);
//				_roll	*=	Mathf.Abs(_roll);
//		_rigidbody.AddTorque(_transform.up * turnImpulse * _gravity * _roll * bankSteeringFactor, ForceMode.Acceleration);
		
		//	apply wind-resistance / drag depending on aspect of craft
		float	_aspect	=	Vector3.Dot(_transform.TransformDirection(Vector3.left), _rigidbody.velocity.normalized);
		//	slows old velocity
		_rigidbody.AddForce (-_rigidbody.velocity * Mathf.Abs(_aspect) * turnVelocityPreservation * orthogonalDrag, ForceMode.Acceleration);
		//	boosts velocity in new direction
		_rigidbody.AddForce (_transform.forward * -_aspect * turnVelocityPreservation * _forwardVelocity, ForceMode.Acceleration);
	}
}
