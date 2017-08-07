using UnityEngine;
using System.Collections;

public class LiftThruster : MonoBehaviour {
	
	public	bool	showGUI				=	true;
	public	string	stringId			=	"n/a";
	public	Vector2	guiPosition			=	Vector2.zero;
	
	public	float	hoverAltitude		=	1.5f;	//	meters
	public	float	thrustAmpFactor		=	1;		//	multplies thrust power
	public	bool	addNeutralBuoyancy	=	false;
	public	bool	addAltitudeBuoyancy	=	true;
	public	Vector3	relativePosition	=	Vector3.zero;
	public	bool	showGizmo			=	false;
	public	Color	gizmoColor			=	Color.red;
	public	float	gizmoSize			=	.1f;	//	meters, radius
	
	private	float	altBuoyancyFactor	=	.1f;
	private	float	_altitude;
	
	//	cache	//
	
	private		Transform	_transform;
	private		Rigidbody	_rigidbody;
	private		Vector3		_relativeDownVector;
	private		Vector3		_buoyancyVector;
	[SerializeField]
	private		float		_gravity;
	
	void Start () {
		_transform		=	transform;
		_rigidbody		=	rigidbody;
		_gravity		=	Physics.gravity.magnitude;
		_buoyancyVector	=	Vector3.zero;
	}
	
	void OnGUI () {
		if (!showGUI) return;
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(guiPosition.x);
		GUILayout.BeginVertical();
		GUILayout.Space(guiPosition.y);
		GUILayout.Label (stringId + "( " + UU.Trunc(_altitude,1) + " : " + UU.Trunc(_buoyancyVector.magnitude,1) + ")");
		
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}
	
	void OnDrawGizmos () {
		if (!showGizmo) return;
		
		Gizmos.color	=	gizmoColor;
		Gizmos.DrawSphere (transform.TransformPoint (relativePosition), gizmoSize);
		Gizmos.DrawRay (transform.TransformPoint(relativePosition), _buoyancyVector);
	}
	
	void FixedUpdate () {
		_relativeDownVector	=	_transform.TransformDirection(Vector3.down);
		_altitude			=	GetAltitude();
		if (addNeutralBuoyancy)		ApplyNeutralBuoyancy();
		if (addAltitudeBuoyancy)	ApplyAltitudeBuoyancy();
	}
	
	private void ApplyNeutralBuoyancy () {
		_buoyancyVector	=	-_relativeDownVector * _gravity * Random.Range(.99f, 1.01f) * thrustAmpFactor;
		
		_rigidbody.AddForceAtPosition (_buoyancyVector, _transform.TransformPoint(relativePosition), ForceMode.Acceleration);
	}
	
	private void ApplyAltitudeBuoyancy () {
		_buoyancyVector	=	Vector3.zero;
		
		altBuoyancyFactor	=	Mathf.Abs(hoverAltitude - _altitude) / hoverAltitude;
		altBuoyancyFactor	*=	altBuoyancyFactor;
		
		if (_altitude < hoverAltitude) {
			_buoyancyVector		=	-_relativeDownVector * _gravity * altBuoyancyFactor * thrustAmpFactor;
			_rigidbody.AddForceAtPosition (_buoyancyVector, _transform.TransformPoint(relativePosition));
			return;
		} else {
//			_buoyancyVector		=	-_relativeDownVector * _gravity * -altBuoyancyFactor * thrustAmpFactor;
//			_rigidbody.AddForceAtPosition (_buoyancyVector,  _transform.TransformPoint(relativePosition), ForceMode.Force);
		}
	}
	
	private float GetAltitude () {
		
		Ray			downRay	=	new Ray (_transform.TransformPoint(relativePosition), _relativeDownVector);
		RaycastHit	data;
		if (Physics.Raycast (downRay, out data, hoverAltitude)) {
			return data.distance;
		} else {
			if (Physics.Raycast (_transform.TransformPoint(relativePosition), Vector3.down, out data)) {
				return data.distance;	
			} else {
				return hoverAltitude;	
			}
		}
	}
}