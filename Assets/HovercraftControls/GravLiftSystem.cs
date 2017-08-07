using UnityEngine;
using System.Collections;

public enum GravityDown {
	Absolute,	//	Vector3.down
	Relative,	//	-transform.up
	Platform	//	Vector3 provided by the gravity platform object
}

[System.Serializable]
public class GravLifter {
}

public class GravLiftSystem : MonoBehaviour {
	
	public	bool		showGizmo			=	false;
	
	public	string		hoverSurfaceLayer	=	"HoverSurfaceLayer";
	public	GravityDown	gravityDown			=	GravityDown.Relative;
	public	float		linearLift			=	1;		//	add lift
	public	float		factorLift			=	1;		//	multiply lift
	public	float		geometricLift		=	1;		//	based on altitude, '2' indicates power^2
	public	Vector3[]	liftPoints;						//	relative locations to apply lift force
	public	float		hoverAltitude		=	2;		//	meters
	
	public	bool		enableGravityKeel	=	true;
	public	float		keelOrientRate		=	2;		//	deg/sec
	
	public	Color		gizmoColor			=	Color.yellow;
	public	float		gizmoSize			=	.1f;	//	meters
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	private		Vector3		_thrustVector;
	private		int			_hoverLayer;
	private		float		_mass;
	private		bool		_debugTick;
	
	void Start () {
		_transform			=	transform;
		_rigidbody			=	rigidbody;
		_mass				=	_rigidbody.mass;
		_hoverLayer			=	1 << LayerMask.NameToLayer(hoverSurfaceLayer);
		//InvokeRepeating ("InvokeDebug", 1, 1);
		_debugTick			=	false;
	}
	
	private void InvokeDebug () {
		//Debug.Log ("Velocity: " + _rigidbody.velocity.magnitude);	
		_debugTick	=	true;
	}
	
	void OnDrawGizmos () {
		if (!showGizmo) return;
		
		foreach (Vector3 v3lp in liftPoints) {
			Gizmos.color	=	gizmoColor;
			Gizmos.DrawSphere (transform.TransformPoint(v3lp), gizmoSize);
			Gizmos.DrawRay (transform.TransformPoint(v3lp), 2* _thrustVector);
		}
	}
	
	void FixedUpdate () {
	
		//	update gravity vector
		
		switch (gravityDown) {
		case GravityDown.Absolute:
			_thrustVector	=	Vector3.up;
			break;
		case GravityDown.Relative:
			_thrustVector	=	_transform.up;
			break;
		}
		
		//	update relative forces on vehicle
		
		foreach (Vector3 v3lp in liftPoints) {
			
			Vector3	worldPosition	=	transform.TransformPoint(v3lp);
			
			Vector3	forceVector		=	_thrustVector;
					forceVector		*=	(_mass + linearLift -1) * factorLift * hoverAltitude;
					forceVector		/=	Mathf.Pow (GetAltitudeAtPoint (_transform.TransformPoint(v3lp)), geometricLift);
			
			_rigidbody.AddForceAtPosition (forceVector, worldPosition, ForceMode.Acceleration);
			
			if (_debugTick) {
				Debug.Log ("forceVector: " + forceVector.magnitude + 
					", altitude: " + UU.Trunc(GetAltitudeAtPoint(_transform.position),2) +
					", divisor: " + Mathf.Pow (GetAltitudeAtPoint (_transform.TransformPoint(v3lp)), geometricLift));
			}
		}
		
		if (enableGravityKeel) {
			RaycastHit	data;
			if (Physics.Raycast(_transform.position, -_thrustVector, out data, 18)) {
				//	rotate toward 'level' as indicated by the surface below
				SelfRightNonKinematic(data.normal);
			} else {
				//	raycast hit nothing, so we migh be flipped over, or very high
				//	so rotate toward the 'universal' level
				SelfRightNonKinematic(Vector3.up);
				
			}
		}	
		
		_debugTick	=	false;
	}
	
//	void LateUpdate () {
//		
//		if (enableGravityKeel && !Physics.Raycast(_transform.position, -_thrustVector)) {
//			SelfRightKinematic();
//		}	
//	}
	
	private float GetAltitudeAtPoint (Vector3 point) {
		RaycastHit	data;
		
		if (Physics.Raycast (point, -_thrustVector, out data, 100, _hoverLayer)) {
			return data.distance;
		} else {
			return Mathf.Infinity;
		}
	}
	
	//	Slowly rotate the craft so it is rightside up.
	//	Make no other changes to its orientation.
	//	This only occurs when the craft recieves no control input.
	private void SelfRightKinematic () {
		Quaternion	goalRotation;
		
		//	todo	this can't be right. it's trying to point the nose 'up'...
		goalRotation		=	Quaternion.LookRotation (_thrustVector);
		_transform.rotation	=	Quaternion.Slerp (_transform.rotation, goalRotation, keelOrientRate * Time.deltaTime);
	}
	
	//	uses forces and torques to rotate the object to a level position
	//	self righting force will apply a torque along a combination of the Z and X axis, but not the Y
	private void XSelfRightNonKinematic (Vector3 upVector) {
		Vector3		zAlign				=	Vector3.Cross (_transform.right, upVector);
		Vector3		xAlign				=	Vector3.Cross (_transform.forward, upVector);
		
		float		thetaZ				=	Mathf.Asin(zAlign.magnitude);
		float		thetaX				=	Mathf.Asin(xAlign.magnitude);
		
		float		deltaTime			=	Time.fixedDeltaTime;	//	since its physics, use fixedDeltaTime
		Vector3		wZ					=	zAlign.normalized * (thetaZ / deltaTime);
		Vector3		wX					=	xAlign.normalized * (thetaX / deltaTime);
		
		Quaternion	q					=	_transform.rotation * _rigidbody.inertiaTensorRotation;
		Vector3		torque				=	q * Vector3.Scale (_rigidbody.inertiaTensor, Quaternion.Inverse(q) * (wZ + wX));
		
		_rigidbody.AddTorque (keelOrientRate * torque, ForceMode.Acceleration);
	}
	
	//	uses forces and torques to rotate the object to a level position
	private void SelfRightNonKinematic (Vector3 upVector) {
		Vector3		crossProduct	=	Vector3.Cross (_transform.up, upVector.normalized);
		float		theta			=	Mathf.Asin (crossProduct.magnitude);
		Vector3		w				=	crossProduct.normalized * theta / Time.fixedDeltaTime;
		Quaternion	q				=	_transform.rotation * _rigidbody.inertiaTensorRotation;
		Vector3		torque			=	q * Vector3.Scale (_rigidbody.inertiaTensor, Quaternion.Inverse(q) * w);
		_rigidbody.AddTorque (keelOrientRate * torque, ForceMode.Acceleration);
	}
	
	//	listeners	//
	
	public void SetPlatformGravityVector (Vector3 gravityVector) {		
		if (!gravityDown.Equals(GravityDown.Platform)) return;
		
		_thrustVector			=	gravityVector;
	}
	
	public void SetDefaultGravity (bool useDefault) {
		
		_rigidbody.useGravity	=	useDefault;
	}
	
	//	listeners -- end	//
}