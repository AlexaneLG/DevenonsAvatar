using UnityEngine;
using System.Collections;

public class AudioInstructionManager : MonoBehaviour
{

    public AudioClip[] audioInstructions;

    private float startDelay;

    void Start()
    {
        AudioSource Audio = GetComponent<AudioSource>();
    }

    public void launchAudio()
    {
        switch (CharacterControllerBasedOnAxis.currentScenarioItem)
        {
            case -1:
                break;
            case 0:
                //StartCoroutine(PlayScenarioInstruction(1.0f, 0));
                //StartCoroutine(PlayScenarioInstruction(6.0f, 1));
                //StartCoroutine(PlayScenarioInstruction(8.0f, 2));
                break;
            case 1:
                //StartCoroutine(PlayScenarioInstruction(0.0f, 3));
                StartCoroutine(PlayScenarioInstruction(6.0f, 4));
                StartCoroutine(PlayScenarioInstruction(10.0f, 5));
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                StartCoroutine(PlayScenarioInstruction(1.0f, 6));
                StartCoroutine(PlayScenarioInstruction(5.0f, 7));
                StartCoroutine(PlayScenarioInstruction(16.0f, 8));
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            default:
                break;
        }
    }

    public IEnumerator PlayScenarioInstruction(float delay, int instructionRef)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<AudioSource>().clip = audioInstructions[instructionRef];
        GetComponent<AudioSource>().Play();
    }
    
}
