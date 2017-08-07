using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class UU {
	
	public	static	int		FRAMERATE		=	(int)(1 / Time.fixedDeltaTime);
	public	static	float	FRAMELENGTH		=	1000 / FRAMERATE;
	
	public	static	Vector3	VERTICAL_MASK	=	Vector3.forward + Vector3.up;
	public	static	Vector3	XY_MASK			=	Vector3.right + Vector3.up;
	public	static	Vector3	HORIZONTAL_MASK	=	Vector3.forward + Vector3.right;
	
	public	static	string	BUILD_TIME {
		get {	return "Build: 2023 220513"; }
		//	following works for pc build only, not webplayer or NaCl
		//get {	return "Build: + " System.IO.File.GetLastWriteTime(Application.srcValue).ToString(); }
	}
	
	//
	//	preferences and options	//
	//
	#region Preferences and options
	
	public static void SaveSettings (string key, int data) {
		PlayerPrefs.SetInt (key, data);
	}
	
	public static void SaveSettings (string key, float data) {
		PlayerPrefs.SetFloat (key, data);
	}
	
	public static void SaveSettings (string key, string data) {
		PlayerPrefs.SetString (key, data);
	}
	
	public static void LoadSettings (string key, out int data) {
		data	=	PlayerPrefs.GetInt (key);
	}
	
	public static void LoadSettings (string key, out float data) {
		data	=	PlayerPrefs.GetFloat (key);
	}
	
	public static void LoadSettings (string key, out string data) {
		data	=	PlayerPrefs.GetString (key);
	}
	
	#endregion Preference and options
	
	//
	//	GUI	information	//
	//
	#region GUI Info
	/// <summary>
	/// Checks if the GUI was used during this frame
	/// </summary>
	/// <returns>
	/// Returns true if anything was clicked, moved, etc.
	/// </returns>
	public static bool GuiInUse () {
		return GUIUtility.hotControl != 0;
	}
	
	/// <summary>
	/// Gets the string name of the GUI element the user used.
	/// </summary>
	/// <returns>
	/// The string name of the GUI element. Note, this value must be set just before
	/// the gui element using GUI.SetNextControlName("foo")
	/// </returns>
	public static string GetNameOfGuiInUse () {
		return GUI.GetNameOfFocusedControl();	
	}
	
	//	TODO
	public static bool CursorOverControl (string controlName) {	
		return false;
	}
	#endregion GUI Info
	
	//
	//	User Input
	//
	#region User Input
	
	public static Vector2 MousePositionToVector2 () {
		return MousePositionToVector2 (Input.mousePosition);	
	}
	
	public static Vector2 MousePositionToVector2 (Vector3 mousePosition) {
		return new Vector2 (mousePosition.x, mousePosition.y);
	}
	
	public static LayerMask LayerNameToMask (string layerName) {
		return 1 << LayerMask.NameToLayer(layerName);	
	}
	
	//	checks for input like CTRL-X and the like
	public static bool ComboInput (KeyCode commandKey, KeyCode alphaKey) {
		return Input.GetKey(commandKey) && Input.GetKeyDown (alphaKey);
	}
	
	public static RaycastHit GetMouseOverObject (Vector3 mousePosition) {
		RaycastHit	hitInfo;
		Physics.Raycast (Camera.main.ScreenPointToRay(mousePosition), out hitInfo);

		return hitInfo;
	}
	
	public static RaycastHit GetMouseOverObjectInLayer (string layerName) {
		return GetMouseOverObjectInLayer (Input.mousePosition, layerName);
	}
	
	public static RaycastHit GetMouseOverObjectInLayer (Vector3 mousePosition, string layerName) {
		RaycastHit	hitInfo;
		LayerMask	mask	=	1 << LayerMask.NameToLayer(layerName);
		Physics.Raycast (Camera.main.ScreenPointToRay(mousePosition), out hitInfo, Mathf.Infinity, mask);

		return hitInfo;
	}
	
	public static RaycastHit GetMouseOverObjectInLayer (Vector3 mousePosition, int layerNumber) {
		return GetMouseOverObjectInLayer (mousePosition, LayerMask.LayerToName(layerNumber));
	}
	
	public static RaycastHit GetMouseOverObjectWithTag (Vector3 mousePosition, string tag) {
		RaycastHit	hitInfo	=	GetMouseOverObject (mousePosition);
		if (hitInfo.transform.tag.Equals(tag)) return hitInfo;
		return new RaycastHit();
	}
	#endregion User Input
	
	//
	//	Sorting	//
	//
	#region Sorting
	//	TODO
	public static List<GameObject> SortByRange (List<GameObject> list, Transform target) {
		return list;
	}
	
	//	TODO
	public static int CompareDistance (Transform lhs, Transform rhs) { 
		return 0;
	}
	
	public static int ComparePlatform (Transform lhs, Transform rhs) {
	
		if (lhs.tag.Equals(rhs.tag)) return 0;
		if (lhs.tag.Equals("EnemyAircraft")) return 1;
		else return -1;
	}
	
	public static int CompareMarchingOrder (Transform lhs, Transform rhs) {
		//	lower instance ID means it is newer, and so should 'behind' the higher ID
		//	this is counterintuitive, but was confirmed by empirical evidence
		if (lhs.GetInstanceID() > rhs.GetInstanceID()) return 1;
		if (rhs.GetInstanceID() < rhs.GetInstanceID()) return -1;
		else return 0;
	}
	
//	public static int CompareSpeed (Transform lhs, Transform rhs) {
//		BasicThrust	lht	=	lhs.GetComponent<BasicThrust>();
//		BasicThrust	rht	=	rhs.GetComponent<BasicThrust>();
//		
//		if (lht.thrust < rht.thrust) return 1;
//		if (lht.thrust == rht.thrust) return 0;
//		else return -1;
//	}
	
//	public static int CompareStrength (Transform lhs, Transform rhs) {
//		Destruct	lhd	=	lhs.GetComponent<Destruct>();
//		Destruct	rhd	=	rhs.GetComponent<Destruct>();
//		
//		if (lhd.damageCapacity < rhd.damageCapacity) return 1;
//		if (lhd.damageCapacity == rhd.damageCapacity) return 0;
//		else return -1;
//	}
	#endregion Sorting
	
	//
	//	Selecting Random Elements from Lists	//
	//
	#region Selecting Elements from Lists
	public static GameObject RandomElementN (List<GameObject> list, int topNgroup) {
		if (list.Count == 0) return null;
		
		return list[Random.Range(0, Mathf.Clamp(list.Count, 1, topNgroup))];
	}
		
	public static Transform RandomElementN (List<Transform> list, int topNgroup) {
		if (list.Count == 0) return null;
		
		return list[Random.Range(0, Mathf.Clamp(list.Count, 1, topNgroup))];
	}
	
	public static Transform RandomElement (List<Transform> list) {
		return list[Random.Range(0, list.Count)];	
	}
		
	public static T[] RandomizeArray<T> (T[] array) {
	
		for (int i = 0 ; i < array.Length ; i++) {
			T	firstValue		=	array[i];
			int	swapIndex		=	Random.Range (i, array.Length);
			array[i]			=	array[swapIndex];
			array[swapIndex]	=	firstValue;
		}
		
		return array;
	}
	
	public static bool ArrayHasElement<T> (T[] array, T element) {
		for (int i = array.Length ; i > 0 ; i--) {
			if (array[i-1].Equals(element)) return true;
		}
		return false;
	}
	
	public static bool ElementInArray<T> (T element, T[] array) {
		return ArrayHasElement<T> (array, element);	
	}
	
	#endregion Selecting Elements from Lists
	
	//
	//	Environment	//
	//
	#region Environment
	public static void SetFog (FogData fogData) {
		RenderSettings.fog				=	fogData.fogEnabled;
		RenderSettings.fogColor			=	fogData.fogColor;
		RenderSettings.fogMode			=	fogData.fogMode;
		RenderSettings.fogDensity		=	fogData.fogDensity;
		RenderSettings.fogStartDistance	=	fogData.linearFogStart;
		RenderSettings.fogEndDistance	=	fogData.linearFogEnd;
		RenderSettings.ambientLight		=	fogData.ambientColor;
	}
	#endregion Environment
	
	//
	//	Raycasting	//
	//
	#region Raycasting
	
	public static bool OnScreen (GameObject gameObject) {
		return OnScreen (gameObject.transform.position);	
	}
	
	public static bool OnScreen (Transform transform) {
		return OnScreen (transform.position);	
	}

	//	TODO
	public static bool OnScreen (Vector3 position) {
		return true;
	}
	
	#endregion Raycasting
	
	//
	//	Math and Time
	//
	#region Math and Time
	
	public static string FormatTime () {
		return FormatTime (Time.time);
	}
	
	public static string FormatTimeB () {
		return FormatTime (Time.time) + " : ";
	}
	
	//	input 184905.35 (seconds) --> returns 2:03:21:45.35 (string)
	public static string FormatTime (float seconds) {
		
		int	dec			=	(int)((seconds *100) % 100);	//	dec = 184905.35 * 100 (18490535), %100 (35)
		int	remainder	=	(int)Trunc (seconds, 0);		//	rem = trunc (184905.35,2) = 184905
		
		int	sec			=	remainder % 60;					//	sec = 184905 % 60 = 45
		remainder		-=	sec;							//	rem = 192 - 45 = 184860
		
		remainder		/=	60;								//	rem = 184860 / 60 = 3081
		int	min			=	remainder % 60;					//	min = 3081 % 60 = 21
		remainder		-=	min;							//	rem = 3081 - 21 = 3060
		
		remainder		/=	60;								//	rem = 3060 / 60 = 51
		int	hr			=	remainder % 24;					//	hr  = 51 % 24 = 3
		remainder		-=	hr;								//	rem = 51 - 3 = 48
		
		int	day			=	remainder / 24;					//	day = 48 / 24 = 2
		
		string fTime	=	dec.ToString();
		while (fTime.Length < 2)	fTime	=	"0" + fTime;
		
		fTime	=	sec + "." + fTime;
		if (min > 0 || hr > 0 || day > 0) {
			while (fTime.Length < 5)	fTime	=	"0" + fTime;
			
			fTime	=	min + ":" + fTime;
			if (hr > 0 || day > 0) {
				while (fTime.Length < 8)	fTime	=	"0" + fTime;
				
				fTime	=	hr + ":" + fTime;
				if (day > 0) {
					while (fTime.Length < 11)	fTime	=	"0" + fTime;
					
					fTime	=	day + ":" + fTime;
				}
			}
		}
		return fTime;	
	}
	
	//	input 123.456 --> returns 123
	public static int Trunc (float number) {
		return (int)Trunc (number, 0);
	}
	
	//	input 123.456 (decimalPlaces: 1) --> returns 123.4
	public static float Trunc (float number, float decimalPlaces) {
		float	multiplizer	=	Mathf.Pow(10,decimalPlaces);
		
		return ((int)(number * multiplizer))/ multiplizer;
	}
	
	//	input 123.456 --> returns .456
	public static float TruncInv (float number) {
		return number - Mathf.CeilToInt(number);
	}
	
	#endregion Math and Time
	
	//
	//	Colors
	//
	#region Colors
	
	public static Color Lighten01 (Color color, float percentLight) {
		return new Color ((1 - color.r) * percentLight + color.r,
						(1 - color.g) * percentLight + color.g,
						(1 - color.b) * percentLight + color.b,
						color.a);
	}
	
	public static Color Lighten (Color color) {
		return Lighten01 (color, .5f);
	}
	
	public static Color Darken01 (Color color, float percentDark) {
		return new Color (color.r * (1-percentDark),
						color.g * (1-percentDark), 
						color.b * (1-percentDark),
						color.a);
	}
	
	public static Color Darken (Color color) {
		return Darken01 (color, .5f);
	}
	
	public static Color AdjustOpacity01 (Color color, float opacity) {
		return new Color (color.r, color.g, color.b, Mathf.Clamp01(color.a + opacity));
	}
		
	public static Color SetOpacity01 (Color color, float opacity) {
		return new Color (color.r, color.g, color.b, Mathf.Clamp01(opacity));
	}
	
	public static Color RandomColor (Color startColor, Color endColor) {
		return new Color (Random.Range (startColor.r, endColor.r),
						Random.Range (startColor.g, endColor.g),
						Random.Range (startColor.b, endColor.b),
						(startColor.a + endColor.a) * .5f);	
	}
	
	public static Color RandomColor () {
		return RandomColor (Color.white, Color.black);	
	}
	
	public static Color RandomColor (Color color, float saturationVariance) {
		return RandomColor (Lighten01(color, saturationVariance), Darken01(color, saturationVariance));
	}
	
	//	Normalizes the brightest component to a value of 200/255,
	//	adjusts the other values accordingly. No effect on alpha.
	public static Color Pastel (Color color) {
		float	highestValue	=	Mathf.Max (new float[] { color.r, color.g, color.b });
		
		return new Color (color.r * 200 / highestValue,
						color.g * 200 / highestValue, 
						color.b * 200 / highestValue,
						color.a);
	}
	
	public static Color Desaturate (Color color) {
		float	highestValue	=	Mathf.Max (new float[] { color.r, color.g, color.b });
		return new Color (highestValue, highestValue, highestValue, color.a);
	}
	
	public static Color Saturate (Color color) {
		float	highestValue	=	Mathf.Max (new float[] { color.r, color.g, color.b });
		
		return new Color (color.r * 255 / highestValue,
						color.g * 255 / highestValue, 
						color.b * 255 / highestValue,
						color.a);
	}
	
	//	ratio of 0 indicates 100% left color, 1 indicates 100% right color
	public static Color BlendColors (Color lhs, Color rhs, float ratio) {
		return new Color ((1-ratio) * lhs.r + ratio * rhs.r,
						(1-ratio) * lhs.g + ratio * rhs.g,
						(1-ratio) * lhs.b + ratio * rhs.b,
						(1-ratio) * lhs.a + ratio * rhs.a);	
	}
	
	public static Color AverageColors (Color lhs, Color rhs) {
		return BlendColors(lhs, rhs, .5f);
	}
	
	#endregion Colors
	
	//
	//	Chance Generators / "dice"	//
	//
	#region Chance Generators
	public static bool PercentChance (float percentSuccess) {
		percentSuccess	=	Mathf.Clamp (percentSuccess, 0, 100);
		return Chance (percentSuccess, 100 - percentSuccess);
	}
	
	public static bool ChanceInMin (int win, int lose) {
		return ChanceInSec (win, lose) && Chance (1, FRAMERATE);
	}
	
	public static bool ChanceInSec (int win, int lose) {
		return Chance (win, lose) && Chance (1, FRAMERATE);
	}
	
	public static bool Chance (int win, int lose) {
		return Random.Range(0, win + lose) < win;
	}
	
	public static bool Chance (float win, float lose) {
		return Random.Range (0, win + lose) < win;
	}
	#endregion Change Generators
	
	//
	//	Random Values	//
	//
	#region Random Values
	
	public static int RandomSign () {
		return Random.Range(0,2)*2-1;
	}
	
	public static bool RandomBool () {
		return Chance (1,1);
	}
	
	public static Vector3 RandomVector () {
		return RandomVector(1).normalized;
	}
	
	public static Vector3 RandomVector (float size) {
		return	Random.Range (-size, size) * Vector3.right +
				Random.Range (-size, size) * Vector3.up +
				Random.Range (-size, size) * Vector3.forward;
	}
	
	public static Vector3 RandomVector (float size, Vector3 rotationMask) {
		Vector3	v	=	RandomVector (size);
				v.Scale(rotationMask);
		return v;
	}
	
	public static Vector3 RandomOrthoVector () {
		switch (Random.Range(0,3)) {
		case 0:
			return Vector3.forward;
		case 1:
			return Vector3.right;
		case 2:
			return Vector3.up;
		default:
			return Vector3.zero;
		}
	}
	
	public static Vector3 RandomZOrthoVector () {
		switch (Random.Range(0,4)) {
		case 0:
			return Vector3.forward;
		case 1:
			return Vector3.right;
		case 2:
			return Vector3.up;
		default:
			return Vector3.zero;
		}
	}
	
	public static Quaternion RandomQuaternion () {
		return RandomQuaternion (Vector3.one);
	}
	
	public static Quaternion RandomQuaternion (Vector3 rotationMask) {
		Vector3	randomVector	=	RandomVector(360);
				randomVector.Scale(rotationMask);
		return Quaternion.Euler (randomVector);
	}
	
	//	TODO
	public static Vector3 RandomPointInsidePolygon (Vector3[] pointList) {
		return Vector3.zero;
	}
	
	//	returns a random point between 2 randomly selected points
	public static Vector3 RandomPointOnPolygonEdge (Vector3[] pointList) {
		if (pointList.Length == 0) return Vector3.zero;
		if (pointList.Length == 1) return pointList[0];
		
		Vector3	p1	=	pointList [Random.Range(0, pointList.Length)];
		Vector3 p2	=	p1;
		while (p1.Equals(p2)) {
			p2	=	pointList [Random.Range(0, pointList.Length)];
		}
		
		return p1 + Random.value * (p2 - p1);
	}
	
	public static Vector3[] ContactPointsToVector3Array (ContactPoint[] contactPoints) {
		Vector3[]	pointList	=	new Vector3[contactPoints.Length];
		
		for (int i = pointList.Length ; i > 0 ; i--) {
			pointList[i-1]	=	contactPoints[i-1].point;
		}
		
		return pointList;
	}
	
	#endregion Random Values
	
	//
	//	GameObject Utilities	//
	//
	#region GameObject Utilities
	
	public static void SendMessageToArray (GameObject[] array, string message) {
		for (int i = array.Length ; i > 0 ; i--) 
			array[i-1].SendMessage(message);
	}
	
	public static void SendMessageToList (List<Transform> list, string message) {
		for (int i = list.Count ; i > 0 ; i--) 
			list[i-1].SendMessage(message);
	}
	
	public static Transform GetRoot (Transform transform) {
		Transform	t	=	transform;
		
		while (t.parent) t	=	t.parent;
		
		return t;
	}
	
	public static Transform GetRoot (Collider collider) {
		return GetRoot (collider.transform);
	}
	
	public static Transform GetRoot (Collision collisionData) {
		return GetRoot (collisionData.transform);
	}
	
	public static GameObject GetRoot (GameObject gameObject) {
		return GetRoot (gameObject.transform).gameObject;
	}
	
	public static Transform[] FindSubcomponentsWithTag (Transform rootTransform, string tag) {
		Transform[]		transformArray	=	rootTransform.GetComponentsInChildren<Transform>();
		List<Transform>	transformList	=	new List<Transform>();
		
		for (int i = transformArray.Length ; i > 0 ; i--)
			if (transformArray[i-1].tag.Equals(tag))
				transformList.Add (transformArray[i-1]);
		
		return transformList.ToArray();
	}
	#endregion GameObject Utilities
	
	//
	//	Strings	//
	//
	#region Strings
	
	public static string[] EnumToStringArray (System.Type enumeratedType) {
		return System.Enum.GetNames (enumeratedType);
	}
	
	//	TODO
	public static string EnumToString (System.Type enumeratedType) {
		return "null";	
	}
	
	public static string EnumToString (System.Type enumeratedType, int index) {
		return System.Enum.GetName(enumeratedType, index);
	}
	#endregion Strings
	
	//
	//	Vectors	//
	//
	#region Vectors
	public static Vector3 Scale (Vector3 vector, Vector3 scalingVector) {
	
		Vector3	v	=	vector;
				v.Scale(scalingVector);
		return v;
	}
	
	//	when passed a vector such as (0, 3.5, 0), this returns 3.5
	//	if more than one dimension is non-zero, it returns the value of the dimension of greatest magnitude
	//	when passed a vector such as (0, -1.5, .75), this returns -1.5
	public static float GetNonZero (Vector3 v) {
		if (Vector3.zero.Equals(v)) return 0;
		
		float[] d = new float[3] { v.x, v.y, v.z };
		float	min	=	Mathf.Min (d);
		float	max	=	Mathf.Max (d);
		
		if (Mathf.Abs (min) > Mathf.Abs (max)) {
			return min;
		} else {
			return max;
		}
	}
	
	//	TODO
	//	extends Direction vector from Position until it is off screen by an amount of Distance
	//	and returns the vector position of that endpoint
	public static Vector3 GetOffscreenEndpoint (Vector3 position, Vector3 direction, float distance) {
		return Vector3.zero;
	}
	#endregion Vectors
	
	//
	//	Angles	//
	//
	#region Angles
	public static float SignedAngleFromCOS (float cosTheta) {
		return Mathf.Rad2Deg * Mathf.Acos(cosTheta) * cosTheta / Mathf.Abs(cosTheta);	
	}
	
	//	Changed from "NormalizeAngle" to "BoundAngle" while working on SimpleTurretDefense project
	public static float BoundAngle (float angle) {
		float	a	=	angle;
		while (a < 0) a	+=	360;
		while (a >=360)	a-=	360;
		return a;
	}
	
	public static bool AimedWithinTolerance (Transform firer, Transform target, float toleranceCos) {
		return Vector3.Dot (firer.forward, (target.position - firer.position).normalized) > toleranceCos;
	}
	
	public static bool AimedWithinTolerance (Transform firer, Vector3 targetPoint, float toleranceCos) {
		return Vector3.Dot (firer.forward, (targetPoint - firer.position).normalized) > toleranceCos;
	}
	
//	public static Vector3 LeadTarget (Transform firer, Transform target, float shotSpeed) {
//		
//		float	targetDistance	=	Vector3.Magnitude (firer.position - target.position);
//		float	leadTime		=	targetDistance / shotSpeed;
//		BasicThrust	targetThrustData	=	target.GetComponent<BasicThrust>();
//		float	targetThrust	=	2.5f;	//	default of 2.5 m/s
//		
//		try {
//			targetThrust	=	targetThrustData.thrust;
//		} catch (System.NullReferenceException e) {
//			//	target not using the BasicThrust class for movement
//			//	so do nothing and just use the default data
//			Debug.Log (FormatTime() + " : " + "default target thrust used (" + targetThrust + ")");
//		}
//		Vector3	targetVelocity	=	target.forward * targetThrust;	//	for now, we take 1 m/s as the velocity
//		
//		//	aim point is targetVelocity * leadTime + targetPosition
//		
//		return targetVelocity * leadTime + target.position;
//	}
	
	
	public static float Deg2DotProduct (float degrees) {
		return Mathf.Cos (degrees * Mathf.Deg2Rad);
	}
	#endregion Angles
}

[System.Serializable]
public class FogData {
	public	bool	fogEnabled;
	public	Color	fogColor;
	public	FogMode	fogMode;
	public	float	fogDensity;
	public	float	linearFogStart;
	public	float	linearFogEnd;
	public	Color	ambientColor;
}

public struct KeyCombo {
	public	KeyCode[]	keys;
}