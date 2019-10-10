using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAudioListener : MonoBehaviour
{
    public void FootStep()
    {
        SendMessageUpwards("PlayFootStep");
    }
}
