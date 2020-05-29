//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour 
{
	public GameObject	m_target = null;
	public float		m_smooth = 1.0f;

	Vector3 m_lastTargetPosition = Vector3.zero;

	Vector3		m_defaultPosition = Vector3.zero;
	Quaternion	m_defaultRotation = Quaternion.identity;



	public void Follow(bool on)
	{
		if( on == false )
		{
			transform.position = m_defaultPosition;
			transform.rotation = m_defaultRotation;
			enabled = false;
		}
		else
		{
			enabled = true;
		}
	}

	public void SetTarget(GameObject obj)
	{
		m_target = obj;
	}

	void OnChangeCharControl(CharControl character)
	{
		if( character == null )
			return;

		if( character.m_cameraTarget != null )
		{
			SetTarget(character.m_cameraTarget);
		}
		else
		{
			SetTarget(character.gameObject);
		}
	}


	// Use this for initialization
	void Start () 
	{
		m_defaultPosition = transform.position;
		m_defaultRotation = transform.rotation;

		if( m_target == null )
		{
			m_lastTargetPosition = transform.position + transform.forward;
			return;
		}

		m_lastTargetPosition = m_target.transform.position;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_target == null )
			return;

		m_lastTargetPosition = Vector3.Lerp(m_lastTargetPosition, m_target.transform.position, m_smooth * Time.deltaTime);
		transform.LookAt(m_lastTargetPosition );
	}
}
