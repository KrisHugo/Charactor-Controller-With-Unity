using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
[CreateAssetMenu(menuName = "Audio Event Reactor/General")]
public class AudioEventReactor : AudioEvent
{
    public AudioClip[] clips;

    [MinMaxRange(0, 1)]
    public RangedFloat volume;

    [MinMaxRange(0, 2)]
    public RangedFloat pitch;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0)
            return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.Play();
    }

}
