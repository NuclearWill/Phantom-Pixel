using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakertext;
    public TextMeshProUGUI dialoguetext;
    public Dialogue LevelDialogue;
    public Dialogue EndLevelDialogue;
    public float textTimer = 2.0f;
    public GameObject dialogueUI;
    
    private Queue<string> sentences;

    public static bool isDialogueActive { private set; get; } = false;

    /// <summary>
    /// Need to animate Dialogue
    /// </summary>

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();

        StartCoroutine(Dialogue(LevelDialogue));

    }
    
    public IEnumerator Dialogue(Dialogue dialogue)
    {
        StartDialogue(dialogue);

        // dialogue output code
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        while (sentences.Count > 0)
        {
            DisplayNextSentence();
            yield return new WaitForSeconds(textTimer);
        }

        EndDialogue();
    }
    
/*
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting Message with " + dialogue.speaker);

        speakertext.text = dialogue.speaker;
        
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    
    public void EndDialogue(Dialogue dialogue)
    {
        Debug.Log("Level Complete " + dialogue.speaker);

        speakertext.text = dialogue.speaker;
        
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    
*/

    //This has to be auto continue
    // this is probably best handled through a co-routine 
    // in this coroutine the program will first 
    // wait actually i think i just need to make start dialogue the coroutine
    // Start dialogue will fire off at each scene start 
    // then pause the timer until sentences is empty 

    // outputs the sentence to the dialogue box
    private void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        dialoguetext.text = sentence;
        Debug.Log(sentence);
    }

    private void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        dialogueUI.SetActive(true);
        speakertext.text = dialogue.speaker;
        sentences.Clear();
    }
    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        Debug.Log("End of Conversation");
        isDialogueActive = false;
    }
}
