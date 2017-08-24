using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour {
    public static DialogueSystem Instance { get; set; }

    public GameObject dialogueBox;
    public string npcName;
    public List<string> dialogueLines = new List<string>();

    Button continueButton;
    Text dialogueText, nameText;
    int dialogueIndex;

    private void Awake()
    {
        continueButton = dialogueBox.transform.Find("Continue Button").GetComponent<Button>();
        dialogueText = dialogueBox.transform.Find("Dialogue Text").GetComponent<Text>();
        nameText = dialogueBox.transform.Find("Name").GetChild(0).GetComponent<Text>();
        dialogueBox.SetActive(false);

        continueButton.onClick.AddListener(delegate { ContinueDialogue(); });

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddNewDialogue(string[] lines, string npcName)
    {
        ThirdPersonCamera camera = FindObjectOfType<ThirdPersonCamera>();
        camera.transform.GetComponent<Camera>().fieldOfView = 30f;

        AudioManager audioManager = AudioManager.instance;
        Scene scene = SceneManager.GetActiveScene();
        audioManager.StopMusic(scene.name);
        audioManager.PlayMusic("Dialogue");

        dialogueIndex = 0;
        dialogueLines = new List<string>();

        foreach (string line in lines)
        {
            dialogueLines.Add(line);
        }

        this.npcName = npcName;
        CreateDialogue();
        FindObjectOfType<PlayerMovement>().canMove = false;
    }

    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialogueBox.SetActive(true);
    }

    public void ContinueDialogue()
    {
        if (dialogueIndex < dialogueLines.Count -1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else
        {
            ThirdPersonCamera camera = FindObjectOfType<ThirdPersonCamera>();
            camera.transform.GetComponent<Camera>().fieldOfView = 60f;

            AudioManager audioManager = AudioManager.instance;
            Scene scene = SceneManager.GetActiveScene();
            audioManager.StopMusic("Dialogue");
            audioManager.PlayMusic(scene.name);

            dialogueBox.SetActive(false);
            FindObjectOfType<PlayerMovement>().canMove = true;
        }
    }

}
