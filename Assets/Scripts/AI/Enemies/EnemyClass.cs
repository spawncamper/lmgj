using System.Collections;
using UnityEngine;

public class EnemyClass : MonoBehaviour, IKillable
{
    [SerializeField] float enemyDeathEventDelay;

    private void OnEnable()
    {
        EnemyController.EnemyDeathEvent += SelfDestruct;
    }

    private void OnDisable()
    {
        EnemyController.EnemyDeathEvent -= SelfDestruct;
    }

    public void SelfDestruct()
    {
        print("[EnemyClass] SelfDestruct " + gameObject.name);

        StartCoroutine(SelfDestructCoroutine());
    }

    IEnumerator SelfDestructCoroutine()
    {
        yield return new WaitForSeconds(enemyDeathEventDelay);

        Destroy(gameObject);
    }
}