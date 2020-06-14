using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
  // [SerializeField] string playerName = "VietHung";
  // [SerializeField] List<GameObject> players = new List<GameObject>();
  public GameObject player;
  public GameObject playerCamera;
  public Transform PlayerSpawnPoint;
  public GameObject enemySpawner;
  // public GameObject labCam;
  public GameObject labUI;
  public GameObject startGameUI;
  public Text statusText;

  void Start()
  {
    // labCam.SetActive(false);
    labUI.SetActive(false);

    // GameObject playerObj = Instantiate(player, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);

    startGameUI.SetActive(true);
    enemySpawner.SetActive(true);
    enemySpawner.GetComponent<EnemySpawner>().target = playerCamera;
  }

}
