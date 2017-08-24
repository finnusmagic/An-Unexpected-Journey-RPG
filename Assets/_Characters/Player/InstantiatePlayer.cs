using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Database;
using RPG.Characters;
using UnityEngine.SceneManagement;

public class InstantiatePlayer : MonoBehaviour {

    public GameObject playerMesh;

    public Mesh warriorMesh;
    public Mesh archerMesh;
    public Mesh mageMesh;

    public GameObject Inventory;


    private void Start()
    {
        InstantiatePlayerModel();

        Inventory.GetComponent<Inventory>();

        AudioManager audioManager = AudioManager.instance;
        audioManager.StopMusic();
        Scene scene = SceneManager.GetActiveScene();
        audioManager.PlayMusic(scene.name);
    }

    private void InstantiatePlayerModel()
    {
        if (GameInfo.PlayerModel == 0)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = warriorMesh;

            Inventory.GetComponent<Inventory>().addItemToInventory(1);
            Inventory.GetComponent<Inventory>().addItemToInventory(2);
            Inventory.GetComponent<Inventory>().addItemToInventory(3);
            Inventory.GetComponent<Inventory>().addItemToInventory(7);
            Inventory.GetComponent<Inventory>().addItemToInventory(4, 4);
            Inventory.GetComponent<Inventory>().addItemToInventory(5, 4);
        }
        if (GameInfo.PlayerModel == 1)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = archerMesh;
            Inventory.GetComponent<Inventory>().addItemToInventory(2);
            Inventory.GetComponent<Inventory>().addItemToInventory(4, 2);
            Inventory.GetComponent<Inventory>().addItemToInventory(5);
        }
        if (GameInfo.PlayerModel == 2)
        {
            playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = mageMesh;
            Inventory.GetComponent<Inventory>().addItemToInventory(3);
            Inventory.GetComponent<Inventory>().addItemToInventory(4);
            Inventory.GetComponent<Inventory>().addItemToInventory(5, 2);
        }
    }
}
