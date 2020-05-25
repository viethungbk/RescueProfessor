//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ZombieSkill : MonoBehaviour 
{
	public GameObject m_axeHead;
	public GameObject m_axeHand;

	public GameObject m_axeProjectile;

	public int m_exp = 0;
	public int m_expUp = 10;
	public int m_level = 0;

	// Use this for initialization
	void Start () 
	{
		ResetAxe();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void ResetAxe()
	{
		m_axeHand.SetActive(false);
		m_axeHead.SetActive(true);
	}

	void TakeHandAxe()
	{
		CancelInvoke("ResetAxe");

		m_axeHand.SetActive(true);
		m_axeHead.SetActive(false);
	}

	void ThrowAxe()
	{
		m_axeHand.SetActive(false);
		m_axeHead.SetActive(false);

		Invoke("ResetAxe", 4.0f);

		m_exp++;
		if( m_exp > m_expUp )
		{
			m_exp = 0;
			m_level++;
		}


		CreateProjectile(m_axeHand.transform.position, m_axeHand.transform.rotation, gameObject.transform.forward);

		for( int f=0; f < m_level; f++ )
		{
			Vector3 rand = Random.insideUnitSphere;
			rand.y *= 0.5f;
			rand *= 0.5f;

			CreateProjectile(m_axeHand.transform.position, m_axeHand.transform.rotation, gameObject.transform.forward + rand);
		}
	}

	private void CreateProjectile(Vector3 position, Quaternion rotation, Vector3 direction)
	{
		if( m_axeProjectile != null )
		{
			GameObject handAxe = GameObject.Instantiate(m_axeProjectile, position, rotation) as GameObject;
			Projectile projectile = handAxe.GetComponent<Projectile>();
			projectile.m_direction = direction.normalized;
			projectile.m_owner = gameObject;
		}
	}

}
