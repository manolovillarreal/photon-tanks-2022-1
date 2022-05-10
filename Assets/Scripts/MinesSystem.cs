using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesSystem : MonoBehaviourPun
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        if (photonView.IsMine && Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("Enter");
            photonView.RPC("SetMine", RpcTarget.AllBuffered);
        } 
    }

    [PunRPC]
    void SetMine()
    {
        Debug.Log("Colocando una mina");
        
        
        
    }

}
