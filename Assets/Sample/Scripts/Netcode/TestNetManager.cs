using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobo.Net;

public class TestNetManager : NetworkManager
{
    public static TestNetManager TestInstance;
    protected override void Awake()
    {
        base.Awake();
        TestInstance = this;
    }

    [Header("Prefabs")]
    public GameObject localPlayer;
    public GameObject remotePlayer;


    protected override void Start()
    {
        base.Start();
        //client.ClientConnected += SpawnPlayer;
        //client.Connected += () => SpawnPlayer(client);
        client.ClientDisconnected += SomeClientDisconnected;
        client.ClientConnected += SomeClientConnected;
        client.Disconnected += ThisClientDisconnected;
        client.Connected += ThisClientConnected;

        server.ClientConnected += SomeClientConnectedToServer;
        server.ClientDisconnected += SomeClientDisconnectedFromServer;
    }

    void SomeClientDisconnected(Client c)
    {
        Player.Remove(c);
    }

    void SomeClientConnected(Client c)
    {
        Player.Add(c, remotePlayer);
    }

    void ThisClientDisconnected()
    {
        Player.RemoveAll();
    }

    void ThisClientConnected()
    {
        Player.Add(Client.This, localPlayer);

        /*
        Player existingPlayer = FindObjectOfType<Player>();
        if (existingPlayer == null)
        {
            Player.Add(Client.This, localPlayer);
        }
        else
        {
            // You can still run around without being on a server, this will keep that object
            //Player.AddOfflineLocalPlayer(Client.This, existingPlayer);
        }
        */
    }

    void SomeClientConnectedToServer(S_Client c)
    {

    }

    void SomeClientDisconnectedFromServer(S_Client c)
    {

    }

    static void Log(string msg)
    {
        Debug.Log("FOR TEST: " + msg);
    }
}