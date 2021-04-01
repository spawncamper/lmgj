using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem onClickEffect;


    public void PlayClickEffect(Vector3 ClickPoint)
    {
        ParticleSystem clickEffect;
        clickEffect = Instantiate(onClickEffect, ClickPoint, Quaternion.identity);

        Destroy(clickEffect, 1f);
    }
}
