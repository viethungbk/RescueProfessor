using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBtnController : MonoBehaviour
{
  private GameObject weaponHolder;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (!weaponHolder)
    {
      weaponHolder = GameObject.Find("WeaponHolder");
    }
  }

  public void Fire()
  {
    weaponHolder.GetComponent<WeaponManager>().currentWeaponGO.GetComponent<WeaponBase>().Fire();
  }
}
