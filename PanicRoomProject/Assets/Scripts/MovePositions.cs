using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositions : MonoBehaviour
{
    public List<AudioClip> RandomSounds;

    public AudioClip GetRandomSound()
    {
        int rand = Random.Range(0, RandomSounds.Count);
        AudioClip sound = RandomSounds[rand];
        return sound;
    }
}
