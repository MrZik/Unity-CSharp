using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SequenceCreator : MonoBehaviour
{
    [SerializeField] private AudioClip correctSoundClip;
    [SerializeField] private AudioClip wrongSoundClip;
    [SerializeField] private int currentSequenceCount = 1;
    [Space]
    [SerializeField] private List<int> sequenceList = new List<int>();
    private const int MIN_LIGHT_COUNT = 0;
    private const int MAX_LIGHT_COUNT = 4;
    private WaitForSeconds playSequenceDelay;
    private WaitForSeconds initializationDelay;

    internal int TotalSequenceCount => currentSequenceCount;

    private void Awake()
    {
        playSequenceDelay = new WaitForSeconds(0.55f);
        initializationDelay = new WaitForSeconds(1.25f);
    }

    internal void InitializeSequenceCreator()
    {
        sequenceList.Clear();
    }

    internal void CreateSequnce()
    {
        System.Random randNum = new System.Random();
        sequenceList = Enumerable.Repeat(0, currentSequenceCount)
                                 .Select(i => randNum.Next(MIN_LIGHT_COUNT, MAX_LIGHT_COUNT))
                                 .ToList();

        StartCoroutine(DelayNextState());
        
    }

    private IEnumerator DelayNextState()
    {
        yield return playSequenceDelay;
        GameManager.Instance.SwitchState(GameState.PlaySequence);
    }

    internal void IncreaseSequenceCount()
    {
        currentSequenceCount++;

        StartCoroutine(DelayNextInitialiation());
    }

    private IEnumerator DelayNextInitialiation()
    {
        yield return initializationDelay;

        GameManager.Instance.SwitchState(GameState.Initialize);
    }

    internal void ResetSequenceCount()
    {
        currentSequenceCount = 1;
    }

    internal void DecreaseSequenceCount()
    {
        currentSequenceCount--;

        if(currentSequenceCount < 1 ) {
            currentSequenceCount = 1;
        }

        StartCoroutine(DelayNextInitialiation());
    }


    internal int GetSequnceNumberAtIndex(int index)
    {
        return sequenceList[index];
    }

    internal bool SequnceListHasIndex(int index)
    {
        return sequenceList.Contains(index);
    }

    public List<int> GetSequenceList()
    {
        return sequenceList;
    }
}
