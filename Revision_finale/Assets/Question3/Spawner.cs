using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {
    public GameObject playerRespawnManagerPrefab;
    public GameObject middleOscillatorPrefab;
    public GameObject side1;
    public GameObject side2;
    void Start()
    {
        CmdSpawnManager();
        CmdSpawnOscillator();
    }

    [Command]
    public void CmdSpawnManager()
    {
        GameObject playerRespawnManager = (GameObject)Instantiate(playerRespawnManagerPrefab);
        NetworkServer.Spawn(playerRespawnManager);
    }

    [Command]
    public void CmdSpawnOscillator()
    {
        GameObject middleOscillator = (GameObject)Instantiate(middleOscillatorPrefab, side1.transform);
        middleOscillator.transform.localPosition = new Vector3(12.5f, 0, 23);
        NetworkServer.Spawn(middleOscillator);

        middleOscillator = (GameObject)Instantiate(middleOscillatorPrefab, side2.transform);
        middleOscillator.transform.localPosition = new Vector3(12.5f, 0, 23);
        NetworkServer.Spawn(middleOscillator);
    }
}
