//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;

public class AnimationList : MonoBehaviour 
{
	public string[]	m_animationList;

	public void PlayAnimation(string animationName)
	{
		if( Array.IndexOf<string>(m_animationList, animationName) != -1 )
		{
			GetComponent<Animator>().CrossFade(animationName , 0.1f,0,0.0f);
		}
	}

}
