using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Linq;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class Connect : MonoBehaviour
{

    Guid hostAllocationId;
    Guid playerAllocationId;
    string allocationRegion = "";
    string relayCode = "n/a";
    string playerId = "Not signed in";
    string autoSelectRegionName = "auto-select (QoS)";
    int regionAutoSelectIndex = 0;
    List<Region> regions = new List<Region>();
    List<string> regionOptions = new List<string>();

    private UnityTransport transport;

    [SerializeField] private int maxConnections = 4;

    async void Start()
    {
        transport = GetComponent<UnityTransport>();
        await UnityServices.InitializeAsync();
        OnSignIn();
    }

    public async void OnSignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = AuthenticationService.Instance.PlayerId;

        Debug.Log($"Signed in. Player ID: {playerId}");
    }

    public async void startHost(){

        Debug.Log("Host - Creating an allocation.");

        // Important: Once the allocation is created, you have ten seconds to BIND
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        hostAllocationId = allocation.AllocationId;
        allocationRegion = allocation.Region;

        Debug.Log($"Host Allocation ID: {hostAllocationId}, region: {allocationRegion}");


        Debug.Log("Host - Getting a join code for my allocation. I would share that join code with the other players so they can join my session.");
        transport.SetHostRelayData(allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData);
        try
        {

            relayCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocationId);
            Debug.Log("Host - Got join code: " + relayCode);
            var dataObject = new DataObject(DataObject.VisibilityOptions.Public, relayCode);
            var data = new Dictionary<string, DataObject>();
            data.Add("JOIN_CODE", dataObject);

            var lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = data
            };
            await Lobbies.Instance.CreateLobbyAsync("Lobby Test", maxConnections, lobbyOptions);
            NetworkManager.Singleton.StartHost();
            Debug.Log("HOST IS STARTING!");
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }

    }

    public async void startClient(){

        Debug.Log("Player - Joining host allocation using join code.");

        try
        {

            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            relayCode = lobby.Data["JOIN_CODE"].Value;
            
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
            playerAllocationId = joinAllocation.AllocationId;
            Debug.Log("Player Allocation ID: " + playerAllocationId);

            transport.SetClientRelayData(joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
            Debug.Log("CLIENT IS STARTING!");
            
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }
}
