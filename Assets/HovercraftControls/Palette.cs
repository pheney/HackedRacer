using UnityEngine;
using System.Collections;

public static class Palette {
	
	//	From the Unity API on colors:
	
	//	Each color component is a floating point value with a range from 0 to 1.
	//	Components (r,g,b) define a color in RGB color space. Alpha component (a) 
	//	defines transparency - alpha of one is completely opaque, alpha of zero 
	//	is completely transparent.
	
	#region Basics
	public	static	Color	Orange			=	new Color(1			,	203f/255	,	0			);
	public	static	Color	RoyalBlue		=	new Color(0			,	190f/255	,	1			);
	public	static	Color	Aqua			=	new Color(0			,	1			,	180f/255	);
	public	static	Color	Magenta			=	new Color(1			,	0			,	1			);
	public	static	Color	Brown			=	new Color(.5f		,	.25f		,	0			);
	public	static	Color	LightBrown		=	UU.Lighten01(Brown, .1f);
	#endregion Basics
	
	#region Metals
	public	static	Color	Gold			=	new Color(1			,	188f/255	,	0			);
	public	static	Color	Copper			=	new Color(201f/255	,	174f/255	,	93f/255		);
	public	static	Color	Bronze			=	new Color(205f/255	,	127f/255	,	50f/255		);
	public	static	Color	SilverLight		=	new Color(225f/255	,	226f/255	,	227f/255	);
	public	static	Color	SilverDark		=	new Color(201f/255	,	192f/255	,	187f/255	);
	public	static	Color	BlueSilver		=	new Color(131f/255	,	150f/255	,	156f/255	);
	public	static	Color	Titanium		=	new Color(182f/255	,	175f/255	,	169f/255	);
	public	static	Color	TitaniumWhite	=	new Color(252f/255	,	1			,	240f/255	);
	public	static	Color	TitaniumYellow	=	new Color(238f/255	,	230f/255	,	0			);
	public	static	Color	Platinum		=	new Color(229f/255	,	228f/255	,	226f/255	);
	#endregion Metals
	
	#region Darks
	public	static	Color	Olive			=	new Color(29f/255	,	69f/255		,	0			);
	public	static	Color	Violet			=	new Color(79f/255	,	0			,	133f/255	);
	public	static	Color	Maroon			=	new Color(152f/255	,	0			,	46f/255		);
	public	static	Color	BurntRed		=	new Color(152f/255	,	0			,	0			);
	public	static	Color	Purple			=	new Color(200f/255	,	0			,	200f/255	);
	public	static	Color	Khaki			=	new	Color(189f/255	,	183f/255	,	107f/255	);
	public	static	Color	SeaGreen		=	new Color(60f/255	,	179f/255	,	113f/255	);
	public	static	Color	SlateGreen		=	new Color(46f/255	,	139f/255	,	87f/255		);
	public	static	Color	SlateBlue		=	new Color(113f/255	,	128f/255	,	128f/255	);
	#endregion Darks
	
	#region Pastels
	public	static	Color	ElectricBlue	=	new Color(200f/255	,	1			,	1			);
	public	static	Color	Pink			=	new Color(1			,	200f/255	,	1			);
	public	static	Color	PastelYellow	=	new Color(1			,	1			,	200f/255	);
	
	public	static	Color	PastelRed		=	new Color(1			,	200f/255	,	200f/255	);
	public	static	Color	PastelGreen		=	new Color(200f/255	,	1			,	200f/255	);
	public	static	Color	PastelBlue		=	new Color(200f/255	,	200f/255	,	1			);
	#endregion Pastels
	
	#region Greys
	public	static	Color	Grey10			=	new Color(.1f		,	.1f			,	.1f			);
	public	static	Color	Grey20			=	new Color(.2f		,	.2f			,	.2f			);
	public	static	Color	Grey40			=	new Color(.4f		,	.4f			,	.4f			);
	public	static	Color	Grey60			=	new Color(.6f		,	.6f			,	.6f			);
	public	static	Color	Grey80			=	new Color(.8f		,	.8f			,	.8f			);
	public	static	Color	Grey90			=	new Color(.9f		,	.9f			,	.9f			);
	#endregion Greys
}
