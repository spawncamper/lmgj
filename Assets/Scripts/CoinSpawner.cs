using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;

    public delegate void CoinSpawned();
    public static event CoinSpawned CoinSpawnedEvent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Vector3 CoinPosition = new Vector3(transform.position.x, 0.65f, transform.position.z);
            Instantiate(coin, CoinPosition, Quaternion.identity);

            if (CoinSpawnedEvent != null)
                CoinSpawnedEvent();
        }
    }
}
