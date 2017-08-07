using UnityEngine;
using System.Collections;

public class GravitySphere : MonoBehaviour {
	
	public		float				gravityMagnitude	=	9.81f;		//	m/s/s
	public		float				maxGravityDistance	=	100;		//	meters
	public		Transform			gravityTarget;						//	the thing towards which this is attracted
	
	//	cache	//
	
	private		Transform			_transform;
	private		Rigidbody			_rigidbody;
	private		SphereCollider		_gravityCollider;
	[SerializeField]
	private		Vector3				_gravityVector;
	[SerializeField]
	private		bool				_gravityActive;
	[SerializeField]
	private		int					_gravityLayer;
	
	void Start () {
		_transform					=	transform;
		_rigidbody					=	rigidbody;
		_rigidbody.useGravity		=	false;
		_gravityCollider			=	(SphereCollider)collider;
		_gravityCollider.isTrigger	=	true;
		_gravityVector				=	Vector3.zero;
		_gravityActive				=	false;
		_gravityLayer				=	gravityTarget.gameObject.layer;
	}
	
	// Update is called once per frame
	void Update () {
		if (_gravityActive) {
			//	move toward target object
			_rigidbody.AddForce (_gravityVector * gravityMagnitude * Time.deltaTime, ForceMode.Acceleration);
			
			//	shrink gravity sphere
			_gravityCollider.radius	-=	_rigidbody.velocity.magnitude * Time.deltaTime;
		} else {
			//	grow gravity sphere	
			if (_gravityCollider.radius < maxGravityDistance) {
				_gravityCollider.radius	+=	gravityMagnitude * Time.deltaTime;
			}
		}
	}
	
	void OnTriggerStay (Collider other) {
	
		_gravityActive	=	true;
		
		_gravityVector	=	-other.ClosestPointOnBounds(_transform.position);
	}
	
	void OnTriggerExit () {
		_gravityActive	=	false;
		
	}
	
	void XOnColliderStay (Collision other) {
		//if (other.collider.gameObject.layer != _gravityLayer) return;
		
		_gravityActive	=	true;
		
		//	set gravity vector
		_gravityVector	=	other.contacts[0].point - _transform.position;
		
	}
	
	void XOnCollisionExit (Collision other) {
		//if (other.collider.gameObject.layer != _gravityLayer) return;
		
		_gravityActive	=	false;
	}
}
