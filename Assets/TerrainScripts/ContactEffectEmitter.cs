using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ParticleSystem))]
public class ContactEffectEmitter : MonoBehaviour {
	
	//	cache	//
	
	private		ParticleSystem		_particles;
	
	void Start () {
		_particles	=	particleSystem;
		_particles.emissionRate	=	0;
	}
	
	void OnCollisionStay (Collision other) {
		
		float	impactMagnitude		=	other.gameObject.rigidbody.velocity.magnitude;
		
		for (int i = 1+(int)(impactMagnitude * .1f * Random.value) ; i > 0 && impactMagnitude > 0.1f ; i--) {
			Vector3	impactVelocity		=	UU.RandomVector(1);
					impactVelocity.y	=	Mathf.Abs(impactVelocity.y);

			Vector3	contactPosition		=	UU.RandomPointOnPolygonEdge(UU.ContactPointsToVector3Array (other.contacts));
			float	particleSize		=	Random.Range(1f, 4f);
			float	particleLifetime	=	Random.Range(1, 5);
			Color	particleColor		=	UU.RandomColor (Palette.Brown, .2f);
			
			_particles.Emit (contactPosition, impactVelocity, particleSize, particleLifetime, particleColor);
		}
	}
}
