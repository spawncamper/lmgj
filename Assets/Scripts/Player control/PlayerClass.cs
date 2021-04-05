using System.Collections;
using UnityEngine;

public class PlayerClass : MonoBehaviour, IKillable // IKillable = ICharacter
{
    [SerializeField] float playerDeathEventDelay;

    public void SelfDestruct()
    {
        print("[PlayerClass] SelfDestruct");

        StartCoroutine(SelfDestructCoroutine());
    }

    IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(playerDeathEventDelay);

        Destroy(gameObject);
    }
}