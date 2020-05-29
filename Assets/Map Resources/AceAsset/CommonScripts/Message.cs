//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

static class Message
{
	static public void Broadcast(string funcName)
	{
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for( int i = 0, imax = gos.Length; i < imax; ++i ) gos[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
	}

	static public void Broadcast(string funcName, object param)
	{
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for( int i = 0, imax = gos.Length; i < imax; ++i ) gos[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
	}

}
