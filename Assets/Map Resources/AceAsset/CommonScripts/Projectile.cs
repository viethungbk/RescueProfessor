//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public	float		m_lifeTime	= 10.0f;
	public	float		m_speed		= 3.0f;
	public	Vector3		m_rotation = Vector3.zero;
	public	Vector3		m_direction = Vector3.zero;

	public	float		m_speedMin		= 3.0f;
	public	float		m_speedMax		= 3.0f;
	
	public	Vector3		m_rotationMin = Vector3.zero;
	public	Vector3		m_rotationMax = Vector3.zero;

	public float		m_hitRadius = 0.5f;
	public GameObject	m_owner = null;
	bool				m_moveStop = false;

	// Use this for initialization
	void Start () 
	{
		m_moveStop = false;

		m_speed = Random.Range(m_speedMin, m_speedMax);
		m_rotation.x = Random.Range(m_rotationMin.x, m_rotationMax.x);
		m_rotation.y = Random.Range(m_rotationMin.y, m_rotationMax.y);
		m_rotation.z = Random.Range(m_rotationMin.z, m_rotationMax.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_lifeTime -= Time.deltaTime;
		if( m_lifeTime <= 0.0f ) Destroy(gameObject);

		if( m_moveStop == true )
			return;

		transform.Rotate(m_rotation * Time.deltaTime);

		transform.position += m_direction * m_speed * Time.deltaTime;
		//transform.localEulerAngles += m_rotation * Time.deltaTime;

		CheckHit();

	}

	void CheckHit()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, m_hitRadius);

		foreach( Collider col in cols )
		{
			if( col.isTrigger == true )
				continue;

			CharControl charControl = col.GetComponent<CharControl>();
			if( charControl == null )
			{
				m_moveStop = true;
				return;
			}

			if( charControl.gameObject == m_owner )
				continue;

			charControl.TakeDamage(null, transform.position, m_direction, 1.0f);
			Destroy(gameObject);
			return;
		}


	}

}
