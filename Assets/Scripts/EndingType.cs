using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EndingType : MonoBehaviour
{
    [SerializeField] private string[] dialogueSentence;

    private TextMeshProUGUI textDisplayer;

    private int playSoundAfterTimes;

    private AudioSource audioSrc;

    [SerializeField] private AudioClip typeSound;

    [SerializeField] private float volume;

    [SerializeField] private float typeDelay;

    public bool typing;

    private float delayToNextDialogue;

    private int index;

    [SerializeField] private GameObject nextDialogue;

    [SerializeField] private Image speakerPortraitReference;

    [SerializeField] private Sprite speakerSprite;

    [SerializeField] private float timeToSkipDialogue;

    [SerializeField] private string sceneToLoadWhenDone; 


    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        textDisplayer = FindObjectOfType<TextMeshProUGUI>();
        index = 0;
        CleanDialogue();
        speakerPortraitReference.sprite = speakerSprite;
        StartTyping();
    }


    private void StartTyping()
    {
        StartCoroutine(TypeEffect());
    }


    private void CleanDialogue()
    {
        textDisplayer.text = "";
    }

    private void NextDialogue()
    {
        CleanDialogue();
        index++;
        if (index < dialogueSentence.Length)
        {
            StartCoroutine(TypeEffect());
            return;
        }
   
        index = 0;
        if (nextDialogue != null)
        {
            nextDialogue.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Entered");
            StartCoroutine(Gamemanager.instance.LoadScene(sceneToLoadWhenDone));
        }
        
    }

    private void Update()
    {
        if (!typing)
        {
            delayToNextDialogue += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && delayToNextDialogue >= timeToSkipDialogue)
            {
                NextDialogue();
                delayToNextDialogue = 0;
            }
        }
    }

    private IEnumerator TypeEffect()
    {

        typing = true;
        foreach (char letter in dialogueSentence[index].ToCharArray())
        {

            textDisplayer.text += letter;

            if (playSoundAfterTimes > 0)
            {
                audioSrc.PlayOneShot(typeSound, volume);
                playSoundAfterTimes = 0;
            }

            else playSoundAfterTimes++;


            yield return new WaitForSeconds(typeDelay);
        }

        typing = false;
    }
}
