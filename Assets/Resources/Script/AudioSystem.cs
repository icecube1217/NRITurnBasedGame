using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip attackPlayerSound;
    public AudioClip defendPlayerSound;
    public AudioClip skillPlayerSound;
    public AudioClip attackGoblinSound;
    public AudioClip defendGoblinSound;
    public AudioClip skillGoblinSound;

    public AudioClip buffSound;
    public AudioClip debuffSound;
    public AudioClip healSound;

    public void PlayHeal() { 
        audioSource.PlayOneShot(healSound);
    }

    public void PlayAttackGoblinSound()
    {
        audioSource.PlayOneShot(attackGoblinSound);
    }

    public void PlaySkillGoblinSound()
    {
        audioSource.PlayOneShot(skillGoblinSound);
    }

    public void PlayDefendGoblinSound()
    {
        audioSource.PlayOneShot(defendGoblinSound);
    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackPlayerSound);
    }

    public void PlayDefendSound()
    {
        audioSource.PlayOneShot(defendPlayerSound);
    }

    public void PlaySkillSound()
    {
        audioSource.PlayOneShot(skillPlayerSound);
    }

    public void PlayBuffSound()
    {
        audioSource.PlayOneShot(buffSound);
    }

    public void PlayDebuffSound()
    {
        audioSource.PlayOneShot(debuffSound);
    }
}
