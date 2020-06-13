using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
  private HealthManager healthManager;
  private bool isDestroyed = false;
  public GameObject deadScreen;
  public GameObject reloadButton;
  public GameObject fireButton;


  void Start()
  {
    // healthManager = GetComponent<HealthManager>();
    healthManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HealthManager>();
    // deadScreen = GameObject.Find("UI/StartGameUI/DeadScreen");
  }

  void Update()
  {
    if (!deadScreen)
    {
      deadScreen = GameObject.Find("UI/StartGameUI/DeadScreen");
    }
    if (!reloadButton)
    {
      reloadButton = GameObject.Find("UI/StartGameUI/ReloadButton");
    }
    if (!fireButton)
    {
      fireButton = GameObject.Find("UI/StartGameUI/FireButton");
    }

    if (healthManager.IsDead && !isDestroyed)
    {
      isDestroyed = true;

      StartCoroutine(ShowDeadScreen());

      MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>();

      foreach (MonoBehaviour script in scripts)
      {
        // Disable all weapons
        if (script is WeaponBase)
        {
          DisableWeapon((WeaponBase)script);
        }
        // Deactivate player controls
        else if (script is FirstPersonController)
        {
          DisableController((FirstPersonController)script);
        }
      }

      reloadButton.SetActive(false);
      fireButton.SetActive(false);
    }
  }

  void DisableWeapon(WeaponBase weapon)
  {
    weapon.IsEnabled = false;
  }

  void DisableController(FirstPersonController controller)
  {
    controller.enabled = false;
  }

  IEnumerator ShowDeadScreen()
  {
    deadScreen.SetActive(true);

    Image image = deadScreen.GetComponent<Image>();
    Color origColor = image.color;

    for (float alpha = 0.0f; alpha <= 1.1f; alpha += 0.1f)
    {
      image.color = new Color(origColor.r, origColor.g, origColor.b, alpha);
      yield return new WaitForSeconds(0.1f);
    }

    Time.timeScale = 0;

    yield break;
  }

  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if (hit.gameObject.tag == "BulletCase")
    {
      Physics.IgnoreCollision(GetComponent<Collider>(), hit.gameObject.GetComponent<Collider>());
    }
  }
}
