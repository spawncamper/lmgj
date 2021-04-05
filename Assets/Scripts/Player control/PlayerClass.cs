using System.Collections;
using UnityEngine;

public class PlayerClass : MonoBehaviour, IKillable // IKillable = ICharacter
{
    [SerializeField] float playerDeathEventDelay;

    public void SelfDestruct()
    {
        print("[PlayerClass] SelfDestruct()");

        StartCoroutine(SelfDestructCoroutine());
    }

    IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(playerDeathEventDelay);

        DestroyPlayerObject();
//        Destroy(gameObject);
    }

    void DestroyPlayerObject()
    {
        var playerInstance = FindObjectOfType<PlayerClass>();

        if (playerInstance != null)
        {
            Destroy(playerInstance);

            if (playerInstance != null)
            {
                Debug.LogError("[PlayerClass] DestroyPlayerObject() playerInstance was not destroyed");
            }
        }
        else
        {
            Debug.LogError("[PlayerClass] DestroyPlayerObject() playerInstance == null");
        }
    }
}