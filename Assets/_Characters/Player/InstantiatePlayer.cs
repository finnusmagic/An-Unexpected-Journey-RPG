using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour {

    public GameObject playerMesh;

    public Mesh warriorMesh;
    public Mesh archerMesh;
    public Mesh mageMesh;


    private void Start()
    {
        InstantiatePlayerModel();
    }

    private void InstantiatePlayerModel()
    {
        if (GameInfo.PlayerModel == 0)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = warriorMesh;
        }
        if (GameInfo.PlayerModel == 1)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = archerMesh;
        }
        if (GameInfo.PlayerModel == 2)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = mageMesh;
        }
    }
}
