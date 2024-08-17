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

        client.Connected += () => LogC("Connected");
        client.Disconnected += () => LogC("Disconnected");
        client.ClientConnected += (c) => LogC(c.Username + " connected");
        client.ClientDisconnected += (c) => LogC(c.Username + " disconnected");
        client.ConnectionFailed += () => LogC("Connected failed");

        server.ClientConnected += (c) => LogS(c.Username + " connected");
        server.ClientDisconnected += (c) => LogS(c.Username + " disconnected");
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

    void LogC(string msg)
    {
        string backend = useSteamTransport ? "Steam" : "Sockets";
        Debug.Log($"Client ({backend}): {msg}");
    }

    void LogS(string msg)
    {
        string backend = useSteamTransport ? "Steam" : "Sockets";
        Debug.Log($"Server ({backend}): {msg}");
    }
}