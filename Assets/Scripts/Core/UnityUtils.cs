using UnityEngine;

public static class UnityUtils
{
    public static void PlayProperly(this ParticleSystem particles, bool withChildren = true)
    {
        if (particles.isPlaying)
            particles.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);

        particles.Play(withChildren);
    }
}