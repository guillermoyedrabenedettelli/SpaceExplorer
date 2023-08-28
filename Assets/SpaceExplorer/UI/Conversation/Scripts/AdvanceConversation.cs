using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class AdvanceConversation : MonoBehaviour
{
    Image[] sentences;
    [SerializeField] float timeTostartNextSentence = 2f;
    int currentSentence = 0;
    // Start is called before the first frame update
    void Awake()
    {
        sentences = GetComponentsInChildren<Image>();
        currentSentence = 0;
        foreach(Image i in sentences)
        {
            i.gameObject.SetActive(false);
        }

    }
    private void Start()
    {
        sentences[currentSentence].gameObject.SetActive(true);
        StartCoroutine(NextConversation());
    }


    IEnumerator NextConversation()
    {
        yield return new WaitForSecondsRealtime(timeTostartNextSentence);
        if (currentSentence < sentences.Length-1)
        {
            sentences[currentSentence].gameObject.SetActive(false);
            currentSentence++;
            sentences[currentSentence].gameObject.SetActive(true);
            StartCoroutine(NextConversation());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
