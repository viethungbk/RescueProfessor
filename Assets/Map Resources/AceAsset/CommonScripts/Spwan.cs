//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Spwan : MonoBehaviour 
{
	public	bool	m_mainCharacter = false;

	GameObject		m_character = null;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	public void DeleteCharacter()
	{
		if( m_character != null )
		{
			GameObject.DestroyImmediate(m_character);
		}
	}


	public void SpwanCharacter(GameObject prefab)
	{
		DeleteCharacter();

		GameObject obj = GameObject.Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		m_character = obj;

		CharControl cc = obj.GetComponent<CharControl>();

		if( cc != null )
		{
			if( m_mainCharacter == true )
			{
				cc.Select();
			}
			else
			{
				cc.m_enableControl = false;
			}
		}
	}

	public void AnimationTest(string aniName)
	{
		if( m_character == null )
			return;

		AnimationList aniList = m_character.GetComponent<AnimationList>();
		aniList.PlayAnimation(aniName);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = m_mainCharacter ? Color.blue : Color.white ;
		Gizmos.DrawSphere(transform.position, 0.1f);
		Gizmos.DrawRay(transform.position, transform.forward);
	}

}
