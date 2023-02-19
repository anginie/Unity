using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button BtnStartHost;
    public Button BtnStartServer;
    public Button BtnStartClient;
    public Button BtnMatch;
    public Button BtnSearch;
    public Text TextLog;

    public GameObject PlayerPrefab;

    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    { 
        GameObject netGo = GameObject.Find("NetworkManager");
        networkManager = netGo.GetComponent<NetworkManager>();

        BtnStartHost.onClick.AddListener(() => networkManager.StartHost());
        BtnStartServer.onClick.AddListener(() => networkManager.StartServer());
        BtnStartClient.onClick.AddListener(() => networkManager.StartClient());

        networkManager.OnServerStarted += OnNetServerStarted;
        networkManager.OnClientConnectedCallback += OnNetClientConnected;
        networkManager.OnClientDisconnectCallback += OnNetClientDisconnect;
        networkManager.OnTransportFailure += OnNetTransportFailure;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PlayerPrefab.transform.Translate(Vector3.up);
            //PlayerPrefab.transform.localPosition += Vector3.up;
        }
    }
    
    void OnNetServerStarted()
    {
        DoLog("Server Started");
    }

    void OnNetClientConnected(ulong u)
    {
        DoLog("Client Connected " + u);
    }

    void OnNetClientDisconnect(ulong u)
    {
        DoLog("Client disConnect " + u);
    }

    void OnNetTransportFailure()
    {
        DoLog("Transport Failure");
    }

    void DoLog(string log)
    {
        Debug.Log(log);
        TextLog.text += log + "\n";
    }
}
