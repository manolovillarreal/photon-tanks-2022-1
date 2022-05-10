using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PowerUpSystem : MonoBehaviourPun, IOnEventCallback
{

    private const byte CureEventCode = 1;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateCure();
        }
    }

    private void GenerateCure()
    {

        RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All,CachingOption= EventCaching.DoNotCache };

        PhotonNetwork.RaiseEvent(CureEventCode,null,eventOptions, SendOptions.SendReliable);
        
    }
    

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == CureEventCode)
        {
            Debug.Log("Generando CURA");
        }
    }

}
