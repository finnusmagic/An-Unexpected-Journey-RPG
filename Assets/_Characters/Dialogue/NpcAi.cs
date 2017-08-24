using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class NpcAi : MonoBehaviour {

    public string npcName;
    public string[] dialogue;
    bool hasTalked = false;

    public void StartDialogue()
    {
        DialogueSystem.Instance.AddNewDialogue(dialogue, npcName);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!hasTalked)
            {
                ThirdPersonCamera camera = FindObjectOfType<ThirdPersonCamera>();
                camera.transform.LookAt(this.transform);
                StartDialogue();
                hasTalked = true;
            }
        }
    }
}
