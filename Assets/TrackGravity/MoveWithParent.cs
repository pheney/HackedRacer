using UnityEngine;
using System.Collections;

public class MoveWithParent : MonoBehaviour {

	private	Vector3		_offsetPosition					=	Vector3.zero;
	private	Transform	_transform, _parent;
	private	Quaternion	_offsetRotation;
	
	void Start () {
		_transform				=	transform;
		_parent					=	_transform.parent;
		_offsetPosition			=	_transform.localPosition;
		_offsetRotation			=	_transform.localRotation;
		rigidbody.isKinematic	=	true;
		rigidbody.useGravity	=	false;
	}
	
	void Update () {
		_transform.position	=	_parent.position + _offsetPosition;
		_transform.rotation	=	_parent.rotation * _offsetRotation;
	}
}
