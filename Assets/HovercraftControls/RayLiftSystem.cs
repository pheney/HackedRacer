using UnityEngine;
using System.Collections;

public class RayLiftSystem : MonoBehaviour {
	
	public	KeyCode		disableKey			=	KeyCode.Space;
	public	float		hoverAltitude		=	2;						//	meters
	public	ForceMode	hoverMode			=	ForceMode.Acceleration;
	
	public	float		autolevelRate		=	1;						//	torque multiple
	public	float		autolevelMaxAlt		=	3;						//	meters
	public	ForceMode	autolevelMode		=	ForceMode.Acceleration;
	public	Vector3		autolevelPoint		=	Vector3.forward;
	
	public	float		velocityBoostFactor	=	1;					//	multiple of forward speed added as lift
	
	//	cache	//
	
	private	Transform	_transform;
	private	Rigidbody	_rigidbody;
	private	float		_mass;
	private	float		_drag;
	private	RaycastHit	_data;
	private	bool		_autolevelActive;
	
	void Start () {
		_transform			=	transform;
		_rigidbody			=	rigidbody;
		_mass				=	_rigidbody.mass;
		_drag				=	_rigidbody.drag;
		AutolevelActive(true);
	}
	
	void FixedUpdate () {
		
		//	while the 'disable' key is held down, do the following:
		//	disable all lift-system effects, such as autolevelling, altitude management, etc
		//	activate normal gravity
		if (Input.GetKey(disableKey)) {
			_rigidbody.AddForce (Vector3.down * 9.81f, ForceMode.Acceleration);
			_rigidbody.drag	=	0;
			return;
		}
		
		if (Input.GetKeyUp(disableKey)) {
			_rigidbody.drag	=	_drag;
		}
		
		float		forwardVelocity	=	_transform.InverseTransformDirection(_rigidbody.velocity).z;
					forwardVelocity	*=	velocityBoostFactor;
		
		if (_autolevelActive) {
			//	autolevel the craft
			if (_data.distance < autolevelMaxAlt + forwardVelocity) {
				if (Physics.Raycast (_transform.TransformPoint(autolevelPoint), Vector3.down, out _data)) {
					//	align with terrain
					LevelOut (_data.normal, autolevelRate);
				}
			} else {
				//	align with horizontal plane
				LevelOut (Vector3.up, .05f * .1f);
			}
		}
		
		//	manage altitude adjustment
		if (Physics.Raycast (_transform.position, Vector3.down, out _data)) {
			AdjustAltitude(_data.distance);
		}
		
		//	increase altitude as speed increases
		_rigidbody.AddForce(forwardVelocity * _transform.up, ForceMode.Acceleration);
	}
	
	private void LevelOut (Vector3 up, float torqueMultiple) {
		Vector3		crossProduct	=	Vector3.Cross (_transform.up, up.normalized);
		float		theta			=	Mathf.Asin (crossProduct.magnitude);
		Vector3		w				=	crossProduct.normalized * theta / Time.fixedDeltaTime;
		Quaternion	q				=	_transform.rotation * _rigidbody.inertiaTensorRotation;
		Vector3		torque			=	q * Vector3.Scale (_rigidbody.inertiaTensor, Quaternion.Inverse(q) * w);
		_rigidbody.AddTorque (torqueMultiple * _mass * torque, autolevelMode);
	}
	
	private void AdjustAltitude (float currentAltitude) {
		Vector3		pushDirection	=	(currentAltitude > hoverAltitude) ? -_transform.up : _transform.up;
		float		force			=	Mathf.Abs(currentAltitude - hoverAltitude);
		
		if (pushDirection.Equals(-_transform.up) && 
			force > 9.81f) {
			force	=	9.81f;
		}
		
		_rigidbody.AddForce (pushDirection * force, hoverMode);
	}
	
	//	listeners	//
	
	public void AutolevelActive (bool active) {
		_autolevelActive	=	active;
	}
}
