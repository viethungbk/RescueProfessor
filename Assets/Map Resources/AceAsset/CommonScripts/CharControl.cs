//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CharControl : MonoBehaviour 
{
	[System.Serializable]
	public struct Action
	{
		public string	m_name;		//Action Animation Name
		public KeyCode	m_keyCode;	//Input Keycode
	};


	//---------------
	// public
	//---------------

	public string	m_characterName;

	public bool		m_enableControl = true;
	public float	m_turnSpeed = 10.0f;
	public float	m_moveSpeed = 2.0f;
	public float	m_runSpeedScale = 2.0f;	

	public Vector3	m_attackOffset = Vector3.zero;
	public float	m_attackRadius = 1.0f;
	public float	m_airTime = 0.0f;

	public GameObject	m_hitEffect = null;
	public GameObject	m_cameraTarget = null;

	public string[]		m_damageReaction;
	public Action[]		m_actionList;

	//---------------
	// private
	//---------------
	private CharacterController m_char = null;
	private Animator	m_ani = null;
	private Vector3		m_moveDir = Vector3.zero;
	private bool		m_isRun = false;
	private float		m_moveSpeedScale = 1.0f;



	void Awake()
	{
		m_ani = GetComponent<Animator>();
		m_char = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () 
	{
		if( m_enableControl == true )
		{
			Select();
		}
	}


	public void Select()
	{
		m_enableControl = true;
		Message.Broadcast("OnChangeCharControl", this);
	}
	

	// Update is called once per frame
	void Update () 
	{
		//------------------
		//Parameters Reset
		//------------------
		m_moveDir = Vector3.zero;

		//on air
		m_airTime = m_char.isGrounded ? 0.0f : m_airTime + Time.deltaTime;

		//------------------
		//Update Control
		//------------------
		if( m_enableControl == true )
		{
			m_moveSpeedScale = m_ani.GetFloat("SpeedScale");
			UpdateControl();
		}

		//------------------
		//Parameters sync
		//------------------
		float speed = m_moveDir.magnitude;
		if( m_isRun == true ) speed *= m_runSpeedScale;

		m_ani.SetFloat("Speed", speed, 0.05f, Time.deltaTime);
		m_ani.SetFloat("AirTime", m_airTime);
		m_ani.SetBool("Ground", m_char.isGrounded);
	}

	private void UpdateControl()
	{
		UpdateMoveControl();
		UpdateActionControl();
	}

	private void UpdateMoveControl()
	{
		Vector3 dir = Vector3.zero;
		Vector3 move = Vector3.zero;

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		dir.x = h;
		dir.z = v;

		dir = Camera.main.transform.TransformDirection(dir);
		dir.y = 0.0f;
		dir = Vector3.ClampMagnitude(dir, 1.0f);

		m_moveDir = dir;

		//Run key check
		m_isRun = Input.GetKey(KeyCode.LeftShift);

		// run
		if( m_isRun == true )
		{
			dir *= m_runSpeedScale;
		}

		// default gravity
		move = Vector3.down * 0.05f * Time.deltaTime;

		// turn
		if( dir.magnitude > 0.01f )
		{
			transform.forward = Vector3.RotateTowards(transform.forward, dir, m_turnSpeed * Time.deltaTime, m_turnSpeed);
			move += dir * Time.deltaTime * m_moveSpeed * m_moveSpeedScale;
		}

		// jump
		if( Input.GetButtonDown("Jump") == true && m_char.isGrounded == true )
		{
			m_ani.SetTrigger("Jump");
		}

		// move
		if( move != Vector3.zero )
		{
			m_char.Move(move);
		}
	
	}


	// Check Action Input
	private void UpdateActionControl()
	{
		int actionValue = 0;

		for(int i = 0 ; i < m_actionList.Length ; i ++ )
		{
			if( Input.GetKey(m_actionList[i].m_keyCode) == true )
			{
				actionValue = i + 1;
				break;
			}
		}

		m_ani.SetInteger("Action", actionValue);
	}


	void EventSkill(string skillName)
	{
		SendMessage(skillName, SendMessageOptions.DontRequireReceiver);
	}


	void EventAttack()
	{
		Vector3 center = transform.TransformPoint(m_attackOffset);
		float radius = m_attackRadius;


		Debug.DrawRay(center, transform.forward ,Color.red , 0.5f);

		Collider[] cols = Physics.OverlapSphere(center, radius);

		foreach( Collider col in cols )
		{
			CharControl charControl  = col.GetComponent<CharControl>();
			if( charControl == null )
				continue;

			if( charControl == this)
				continue;

			charControl.TakeDamage(this, center , transform.forward , 1.0f);
		}
	}

	public void TakeDamage(CharControl other,Vector3 hitPosition,  Vector3 hitDirection, float amount)
	{
		//-------------------------
		// Please enter your code.
		// hp calculation
		// animation reaction
		// ...
		//-------------------------

		//----------------------
		// For example
		//----------------------

		//--------------------
		// direction
		if( other != null )
		{
			transform.forward = -other.transform.forward;
		}
		else
		{
			hitDirection.y = 0.0f;
			transform.forward = -hitDirection.normalized;
		}

		//--------------------
		// animation
		string reaction = m_damageReaction[Random.Range(0, m_damageReaction.Length-1)];
		m_ani.CrossFade(reaction, 0.1f, 0, 0.0f);

		//--------------------
		// hitFX
		GameObject.Instantiate(m_hitEffect, hitPosition, Quaternion.identity);
	}


	public string GetHelpText()
	{
		string text = "";

		foreach( Action action in m_actionList )
		{
			text += action.m_keyCode.ToString() + " : " + action.m_name + "\n";
		}

		return text;
	}
}
