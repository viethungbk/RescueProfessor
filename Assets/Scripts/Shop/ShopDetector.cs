using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDetector : MonoBehaviour
{
  public Transform shootPoint;
  public float detectRange;
  public Text shopText;
  public Text warningText;
  public AudioClip purchasedSound;

  GameObject buyButton;
  bool isPurchasable = true;
  ShopType shopType;
  string shopTitle;
  string shopDesc;
  int shopPrice;

  IEnumerator warningTextCo = null;

  IEnumerator HideWarningText()
  {
    yield return new WaitForSeconds(3f);

    warningText.text = "";
    yield break;
  }

  void Start()
  {
    // shopText = GameObject.Find("UI/StartGameUI/Info/ShopText").GetComponent<Text>();
    // warningText = GameObject.Find("UI/StartGameUI/Info/WarningText").GetComponent<Text>();
  }

  void PrintWarning(string text)
  {
    if (warningTextCo != null) StopCoroutine(warningTextCo);

    warningTextCo = HideWarningText();
    warningText.text = text;

    StartCoroutine(warningTextCo);
  }

  void BuyWeapon(Weapon weapon)
  {
    string weaponName = weapon.ToString();
    WeaponManager weaponManager = transform.Find("WeaponHolder").GetComponent<WeaponManager>();
    GameObject weaponGO = transform.Find("WeaponHolder/" + weaponName).gameObject;
    weaponManager.currentWeaponGO.GetComponent<WeaponBase>().Unload();

    weaponManager.currentWeapon = weapon;
    weaponManager.currentWeaponGO = weaponGO;

    weaponManager.primaryWeapon = weapon;
    weaponManager.primaryWeaponGO = weaponGO;

    WeaponBase weaponBase = weaponManager.currentWeaponGO.GetComponent<WeaponBase>();

    weaponManager.currentWeaponGO.SetActive(true);
    weaponBase.InitAmmo();
    weaponBase.Draw();
  }

  void UpgradeWeapon(WeaponBase weaponBase, ShopType upgradeType)
  {
    switch (upgradeType)
    {
      case ShopType.UPGRADE_DAMAGE:
        weaponBase.upgradeDamage++;
        break;
      case ShopType.UPGRADE_RELOAD:
        weaponBase.upgradeReload++;
        break;
      case ShopType.UPGRADE_RECOIL:
        weaponBase.upgradeRecoil++;
        break;
    }
  }

  int GetAmmoPrice(Weapon weapon)
  {
    int price = 0;

    switch (weapon)
    {
      case Weapon.AKM:
        price = 250;
        break;
      case Weapon.M870:
        price = 200;
        break;
      case Weapon.MP5K:
        price = 150;
        break;
      case Weapon.Glock:
        price = 90;
        break;
      default:
        price = 100;
        break;
    }

    return price;
  }

  int GetUpgradePrice(Weapon weapon, int upgraded)
  {
    int basePrice = 100;

    switch (weapon)
    {
      case Weapon.AKM:
        basePrice = 150;
        break;
      case Weapon.M870:
        basePrice = 100;
        break;
      case Weapon.MP5K:
        basePrice = 75;
        break;
      case Weapon.Glock:
        basePrice = 50;
        break;
    }

    return basePrice * (upgraded + 1);
  }

  void Update()
  {
    if (!shopText)
    {
      shopText = GameObject.Find("UI/StartGameUI/Info/ShopText").GetComponent<Text>();
    }

    if (!warningText)
    {
      warningText = GameObject.Find("UI/StartGameUI/Info/WarningText").GetComponent<Text>();
    }

    if (!buyButton)
    {
      buyButton = GameObject.Find("UI/StartGameUI/BuyButton");
    }

    RaycastHit hit;
    Vector3 position = shootPoint.position;
    position.y += 1;  // Adjust height differences

    // Debug.DrawRay(position, transform.TransformDirection(Vector3.forward * detectRange), Color.red);
    if (Physics.Raycast(position, transform.TransformDirection(Vector3.forward * detectRange), out hit, detectRange))
    {
      if (hit.transform.tag == "Shop")
      {
        buyButton.SetActive(true);

        Shop shop = hit.transform.GetComponent<Shop>();
        shopType = shop.shopType;
        shopTitle = shop.title;
        shopDesc = shop.description;
        shopPrice = shop.price;
        // bool isPurchasable = true;

        WeaponManager weaponManager = transform.Find("WeaponHolder").GetComponent<WeaponManager>();
        WeaponBase weaponBase = weaponManager.currentWeaponGO.GetComponent<WeaponBase>();
        Weapon weapon = weaponManager.currentWeapon;

        if (shopType == ShopType.AMMO)
        {
          shopPrice = GetAmmoPrice(weapon);
          shopText.text = shopTitle + "\n(" + shopPrice + "$)\n\n" + shopDesc + "\n\n";
        }
        else if (shopType == ShopType.UPGRADE_DAMAGE)
        {
          int upgraded = weaponBase.upgradeDamage;

          if (upgraded < 10)
          {
            shopPrice = GetUpgradePrice(weaponManager.currentWeapon, upgraded);
            shopText.text = shopTitle + " Lv" + (upgraded + 1) + "\n(" + shopPrice + "$)\n\n" + shopDesc + "\n\n";
          }
          else
          {
            isPurchasable = false;
            shopText.text = "Your weapon is fully upgraded.";
          }
        }
        else if (shopType == ShopType.UPGRADE_RELOAD)
        {
          int upgraded = weaponBase.upgradeReload;

          if (upgraded < 10)
          {
            shopPrice = GetUpgradePrice(weaponManager.currentWeapon, upgraded);
            shopText.text = shopTitle + " Lv" + (upgraded + 1) + "\n(" + shopPrice + "$)\n\n" + shopDesc + "\n\n";
          }
          else
          {
            isPurchasable = false;
            shopText.text = "Your weapon is fully upgraded.";
          }
        }
        else if (shopType == ShopType.UPGRADE_RECOIL)
        {
          int upgraded = weaponBase.upgradeRecoil;

          if (upgraded < 10)
          {
            shopPrice = GetUpgradePrice(weaponManager.currentWeapon, upgraded);
            shopText.text = shopTitle + " Lv" + (upgraded + 1) + "\n(" + shopPrice + "$)\n\n" + shopDesc + "\n\n";
          }
          else
          {
            isPurchasable = false;
            shopText.text = "Your weapon is fully upgraded.";
          }
        }
        else
        {
          shopText.text = shopTitle + "\n(" + shopPrice + "$)\n\n" + shopDesc + "\n\n";
        }
      }
      // if (isPurchasable && Input.GetKeyDown(KeyCode.F))
      //     {
      //       FundSystem fundSystem = transform.parent.GetComponent<FundSystem>();
      //       int fund = fundSystem.GetFund();

      //       if (fund < shopPrice)
      //       {
      //         PrintWarning("Not enough money!");
      //       }
      //       else
      //       {
      //         bool wasPurchased = true;

      //         if (shopType == ShopType.AMMO)
      //         {
      //           weaponBase.bulletsLeft = weaponBase.startBullets + weaponBase.bulletsPerMag;
      //           weaponBase.UpdateAmmoText();
      //         }
      //         else if (shopType == ShopType.WEAPON_MP5K)
      //         {
      //           if (!weaponManager.HasWeapon(Weapon.MP5K))
      //           {
      //             BuyWeapon(Weapon.MP5K);
      //           }
      //           else
      //           {
      //             wasPurchased = false;
      //             PrintWarning("You already have weapon.");
      //           }
      //         }
      //         else if (shopType == ShopType.WEAPON_AKM)
      //         {
      //           if (!weaponManager.HasWeapon(Weapon.AKM))
      //           {
      //             BuyWeapon(Weapon.AKM);
      //           }
      //           else
      //           {
      //             wasPurchased = false;
      //             PrintWarning("You already have weapon.");
      //           }
      //         }
      //         else if (shopType == ShopType.WEAPON_M870)
      //         {
      //           if (!weaponManager.HasWeapon(Weapon.M870))
      //           {
      //             BuyWeapon(Weapon.M870);
      //           }
      //           else
      //           {
      //             wasPurchased = false;
      //             PrintWarning("You already have weapon.");
      //           }
      //         }
      //         else if (shopType == ShopType.UPGRADE_DAMAGE)
      //         {
      //           if (weaponBase.upgradeDamage >= 10)
      //           {
      //             wasPurchased = false;
      //             PrintWarning("Your weapon is fully upgraded.");
      //           }
      //           else
      //           {
      //             UpgradeWeapon(weaponBase, ShopType.UPGRADE_DAMAGE);
      //           }
      //         }
      //         else if (shopType == ShopType.UPGRADE_RELOAD)
      //         {
      //           if (weaponBase.upgradeReload >= 10)
      //           {
      //             wasPurchased = false;
      //             PrintWarning("Your weapon is fully upgraded.");
      //           }
      //           else
      //           {
      //             UpgradeWeapon(weaponBase, ShopType.UPGRADE_RELOAD);
      //           }
      //         }
      //         else if (shopType == ShopType.UPGRADE_RECOIL)
      //         {
      //           if (weaponBase.upgradeRecoil >= 10)
      //           {
      //             wasPurchased = false;
      //             PrintWarning("Your weapon is fully upgraded.");
      //           }
      //           else
      //           {
      //             UpgradeWeapon(weaponBase, ShopType.UPGRADE_RECOIL);
      //           }
      //         }
      //         else
      //         {
      //           wasPurchased = false;
      //         }

      //         if (wasPurchased)
      //         {
      //           fundSystem.TakeFund(shopPrice);
      //           SoundManager soundManager = transform.Find("SoundManager").GetComponent<SoundManager>();
      //           soundManager.Play(purchasedSound);
      //         }
      //       }
      //     }
      //   }
      // }


      else
      {
        shopText.text = "";
        buyButton.SetActive(false);
      }
    }
  }


  public void Purchase()
  {
    if (isPurchasable)
    {
      WeaponManager weaponManager = transform.Find("WeaponHolder").GetComponent<WeaponManager>();
      WeaponBase weaponBase = weaponManager.currentWeaponGO.GetComponent<WeaponBase>();
      Weapon weapon = weaponManager.currentWeapon;

      FundSystem fundSystem = transform.parent.GetComponent<FundSystem>();
      int fund = fundSystem.GetFund();

      if (fund < shopPrice)
      {
        PrintWarning("Not enough money!");
      }
      else
      {
        bool wasPurchased = true;

        if (shopType == ShopType.AMMO)
        {
          weaponBase.bulletsLeft = weaponBase.startBullets + weaponBase.bulletsPerMag;
          weaponBase.UpdateAmmoText();
        }
        else if (shopType == ShopType.WEAPON_MP5K)
        {
          if (!weaponManager.HasWeapon(Weapon.MP5K))
          {
            BuyWeapon(Weapon.MP5K);
          }
          else
          {
            wasPurchased = false;
            PrintWarning("You already have weapon.");
          }
        }
        else if (shopType == ShopType.WEAPON_AKM)
        {
          if (!weaponManager.HasWeapon(Weapon.AKM))
          {
            BuyWeapon(Weapon.AKM);
          }
          else
          {
            wasPurchased = false;
            PrintWarning("You already have weapon.");
          }
        }
        else if (shopType == ShopType.WEAPON_M870)
        {
          if (!weaponManager.HasWeapon(Weapon.M870))
          {
            BuyWeapon(Weapon.M870);
          }
          else
          {
            wasPurchased = false;
            PrintWarning("You already have weapon.");
          }
        }
        else if (shopType == ShopType.UPGRADE_DAMAGE)
        {
          if (weaponBase.upgradeDamage >= 10)
          {
            wasPurchased = false;
            PrintWarning("Your weapon is fully upgraded.");
          }
          else
          {
            UpgradeWeapon(weaponBase, ShopType.UPGRADE_DAMAGE);
          }
        }
        else if (shopType == ShopType.UPGRADE_RELOAD)
        {
          if (weaponBase.upgradeReload >= 10)
          {
            wasPurchased = false;
            PrintWarning("Your weapon is fully upgraded.");
          }
          else
          {
            UpgradeWeapon(weaponBase, ShopType.UPGRADE_RELOAD);
          }
        }
        else if (shopType == ShopType.UPGRADE_RECOIL)
        {
          if (weaponBase.upgradeRecoil >= 10)
          {
            wasPurchased = false;
            PrintWarning("Your weapon is fully upgraded.");
          }
          else
          {
            UpgradeWeapon(weaponBase, ShopType.UPGRADE_RECOIL);
          }
        }
        else
        {
          wasPurchased = false;
        }

        if (wasPurchased)
        {
          fundSystem.TakeFund(shopPrice);
          SoundManager soundManager = transform.Find("SoundManager").GetComponent<SoundManager>();
          soundManager.Play(purchasedSound);
        }
      }
    }
  }
}
