using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
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

    void SelfDestruct()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
