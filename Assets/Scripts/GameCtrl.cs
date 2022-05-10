using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform Player1SpawnPoint;
    [SerializeField]
    private Transform Player2SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
       
        if(playerPrefab == null)
        {
            Debug.LogError("Falta la referencia al player prefab");
        }
        else
        {
            Transform spawnPoint = (PhotonNetwork.IsMasterClient) ? Player1SpawnPoint : Player2SpawnPoint;

            object[] initData = new object[1];
            initData[0] = "DATA INSTANCIACION";

            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity, 0,initData);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
