using UnityEngine;
using System.Collections;

public class ScenarioItem : MonoBehaviour
{

    public float duration = 5.0f;
    public bool EnabledInScenario = true;
    public float timer = 0;

    public float progression
    {
        get
        {
            return timer / duration;
        }
    }

    public float progressionQuad
    {
        get
        {
            return Interpolate.EaseInOutQuad(0f, 1f, timer, duration);
        }
    }


    protected CharacterControllerBasedOnAxis controller;
    protected Transform self;

    void Awake()
    {
        controller = MonoBehaviour.FindObjectOfType<CharacterControllerBasedOnAxis>() as CharacterControllerBasedOnAxis;

        if (controller)
        {
            self = controller.transform;
        }
    }

    public virtual void OnEnable()
    {
        timer = 0f;
    }

    public virtual void OnDisable()
    {
    }

    // Use this for initialization
    public virtual void Start()
    {
        AudioInstructionManager am = FindObjectOfType(typeof(AudioInstructionManager)) as AudioInstructionManager;
        if (am == null)
            return;

        am.launchAudio();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer > duration)
        {
            controller.ActivateNextScenarioItem();
        }

    }

    public void ActivateNextScenarioItem()
    {
        controller.ActivateNextScenarioItem();
    }

    public void AdjustDuration(float newDuration)
    {
        duration = newDuration;
    }

    public void ActiveInScenario(bool activeInScenario)
    {
        EnabledInScenario = activeInScenario;
    }
}
