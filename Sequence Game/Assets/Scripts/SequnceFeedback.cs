using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SequnceFeedback : MonoBehaviour
{
    [SerializeField] private LightButtonHandler lightBtnHandler;
    [SerializeField] private SequenceCreator sequenceCreator;
    [SerializeField] private Light2D buttonLight;
    [Space(10)]
    [SerializeField] private float lightIntensity = 15f;
    [SerializeField] private float minSequenceInterval = 0.6f;
    [SerializeField] private float maxSequenceInterval = 1.6f;
    [SerializeField] private float intervalSubtractor = 0.08f;
    [SerializeField] private float currentSequenceInterval = 1.5f;
    private Coroutine lightCoroutine;
    private WaitForSeconds sequenceInterval;
    private Coroutine sequenceFeedbackCoroutine;

    private void Awake()
    {
        sequenceInterval = new WaitForSeconds(maxSequenceInterval);
    }

    internal void InitializeSequenceFeedback()
    {
        buttonLight.intensity = 0;
        GameManager.Instance.SwitchState(GameState.CreateSequence);
    }

    internal void PlaySequenceFeedback()
    {
        sequenceFeedbackCoroutine = StartCoroutine(SequenceFeedbackCoroutine());
    }

    internal void ResetGamePressed()
    {
        TurnOffLight();

        if (sequenceFeedbackCoroutine != null)
        {
            StopCoroutine(sequenceFeedbackCoroutine);
        }

        
        ResetInterval();
    }

    internal void ResetInterval()
    {
        currentSequenceInterval = maxSequenceInterval;
        sequenceInterval = new WaitForSeconds(currentSequenceInterval);
    }

    internal void DecreaseSequenceInterval()
    {
        currentSequenceInterval -= intervalSubtractor;

        if(currentSequenceInterval < minSequenceInterval)
        {
            currentSequenceInterval = minSequenceInterval;
        }

        sequenceInterval = new WaitForSeconds(currentSequenceInterval);
    }

    internal void IncreaseSequenceInterval()
    {
        currentSequenceInterval += intervalSubtractor;

        if (currentSequenceInterval > maxSequenceInterval)
        {
            currentSequenceInterval = maxSequenceInterval;
        }

        sequenceInterval = new WaitForSeconds(currentSequenceInterval);
    }

    internal void TurnOffLight()
    {
        buttonLight.intensity = 0;
    }

    internal void TurnOnLight()
    {
        if(lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }

        lightCoroutine = StartCoroutine(LightBeepCoroutine());
        //buttonLight.intensity = lightIntensity;
    }

    private IEnumerator LightBeepCoroutine()
    {
        TurnOffLight();

        yield return null;

        while (buttonLight.intensity < lightIntensity)
        {
            buttonLight.intensity += 0.6f;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        while (buttonLight.intensity > 0)
        {
            buttonLight.intensity -= 0.6f;
            yield return null;
        }

        buttonLight.intensity = 0;
    }

    internal void UdateLightPosition(int index)
    {
        buttonLight.transform.position = lightBtnHandler.GetLightBtnAtIndex(index)
                                            .transform.position;
    }

    // Change this to use async
    private IEnumerator SequenceFeedbackCoroutine()
    {
        List<int> sequence = sequenceCreator.GetSequenceList();

        yield return null;

        for (int i = 0; i < sequence.Count; i++) {
            buttonLight.transform.position = lightBtnHandler.GetLightBtnAtIndex(sequence[i])
                                            .transform.position;
            lightCoroutine = StartCoroutine(LightBeepCoroutine());

            AudioHandler.Instance.PlaySfx(lightBtnHandler.GetLightBtnAtIndex(sequence[i]).GetOnSound());

            yield return sequenceInterval;

            TurnOffLight();

            yield return null;
        }

        yield return null;

        GameManager.Instance.SwitchState(GameState.PlayerTurn);
    }
}
