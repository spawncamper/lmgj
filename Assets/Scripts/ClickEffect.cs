using UnityEngine;

public class ClickEffec : MonoBehaviour
{
    [SerializeField] ParticleSystem onClickEffect;
    [SerializeField] float destructionDelay = 1f;

    public void PlayClickEffect(Vector3 ClickPoint)
    {
        ParticleSystem clickEffect;
        clickEffect = Instantiate(onClickEffect, ClickPoint, Quaternion.identity);

        Destroy(clickEffect, destructionDelay);
    }
}
