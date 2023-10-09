using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public enum HandType { Left, Right }
public class OWIHandManager : UdonSharpBehaviour
{
    public GameObject leftHandPrefab;  // Drag your Left Hand prefab here in the Unity Editor
    public GameObject rightHandPrefab; // Drag your Right Hand prefab here in the Unity Editor
    public int maxPlayers = 128;
    private int[] playerIds;
    private bool[] isGameObjectActive;
    private GameObject[] leftHands;  // Stores the instantiated left hand objects for cleanup
    private GameObject[] rightHands; // Stores the instantiated right hand objects for cleanup
    private int availableIndex;
    

    private VRCPlayerApi[] playerQueue;
    private int queueEnd = 0;
    private void Start()
    {
        playerIds = new int[maxPlayers];
        isGameObjectActive = new bool[maxPlayers];
        leftHands = new GameObject[maxPlayers];
        rightHands = new GameObject[maxPlayers];
        playerQueue = new VRCPlayerApi[maxPlayers];
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (queueEnd < maxPlayers)
        {
            playerQueue[queueEnd] = player;
            queueEnd++;
        }
        else
        {
            Debug.LogError("Player queue full!");
        }
    }

    private void Update()
    {
        HandleQueue();
    }

    private void HandleQueue()
    {
        if (queueEnd > 0)
        {
            VRCPlayerApi player = playerQueue[0];
            availableIndex = FindFirstAvailableIndex();

            if (availableIndex != -1)
            {
                playerIds[availableIndex] = player.playerId;
                isGameObjectActive[availableIndex] = true;
                HandInstantiate(availableIndex, player);

                // Shift the array left
                for (int i = 0; i < queueEnd - 1; i++)
                {
                    playerQueue[i] = playerQueue[i + 1];
                }

                queueEnd--;
            }
            else
            {
                Debug.LogError("No available space for new players' hands!");
            }
        }
    }

    public void HandInstantiate(int availableIndex, VRCPlayerApi player)
    {
        // Instantiate the hand prefabs for the player
        leftHands[availableIndex] = Instantiate(leftHandPrefab, transform);
        rightHands[availableIndex] = Instantiate(rightHandPrefab, transform);

        // Pass the VRCPlayerApi reference to the instantiated prefabs
        leftHands[availableIndex].GetComponent<OWIHandInteraction>().Initialize(player, HandType.Left);
        rightHands[availableIndex].GetComponent<OWIHandInteraction>().Initialize(player, HandType.Right);
    }
    
    private int FindFirstAvailableIndex()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (!isGameObjectActive[i])
            {
                return i;
            }
        }
        return -1; // if all GameObjects are active
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        int index = Array.IndexOf(playerIds, player.playerId);
        if (index != -1)
        {
            isGameObjectActive[index] = false;
            playerIds[index] = 0; // reset playerId

            // Clean up the instantiated prefabs when the player leaves
            Destroy(leftHands[index]);
            Destroy(rightHands[index]);
        }
    }
}
