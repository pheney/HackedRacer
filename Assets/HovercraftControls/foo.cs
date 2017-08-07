using UnityEngine;
using System.Collections;

public class foo {
	
	//	this was my coding test for Janus Research. It went well!!

	public byte[] convertFromRgbaToBgra(byte[] inputBytes, int widthInPixels, int heightInPixels) {
		//	assume: input has been sanity checked
	
		byte[]	returnBytes	=	new byte[inputBytes.Length];
		
		for (int i = 0 ; i < inputBytes.Length ; i+=4) {
			returnBytes[i]		=	inputBytes[i+2];	//	R becomes B
			returnBytes[i+1]	=	inputBytes[i+1];	//	G stays G
			returnBytes[i+2]	=	inputBytes[0];		//	B becomes R
			returnBytes[i+3]	=	inputBytes[i+3];	//	A stays A
		}
		
		return returnBytes;
/*
	* inputBytes is a single dimensional array with 4 bytes per pixel,
	* with the bytes of each pixel represented as RGBA. Please return
	* an array with the same image data, with the bytes of each pixel
	* represented as BGRA.
	* 
	* sample input:
    * RgbaRgbaRgba
    * 
    * sample output:
    * BgraBgraBgra
    * 
	*/
				
		
		
	}
}
