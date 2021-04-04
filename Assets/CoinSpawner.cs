using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    // Location: on the PlayerPrefab
    CoinController coinController;
    [SerializeField] GameObject coinPrefab;

    public delegate void CoinSpawned();
    public static event CoinSpawned CoinSpawnedEvent;

    // Start is called before the first frame update
    void Start()
    {
        coinController = GetComponent<CoinController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (coinController.ReturnCurrentCoins() >= 1)
            {
                if (CoinSpawnedEvent != null)
                    CoinSpawnedEvent();

                Vector3 CoinPosition = new Vector3(transform.position.x, 0.65f, transform.position.z);

                GameObject cointInstance = Instantiate(coinPrefab, CoinPosition, Quaternion.identity) as GameObject;
                cointInstance.transform.parent = coinController.transform;
            }
            else
            {
                Debug.LogWarning("[CoinController] currentCoins = 0");
            }
        }
    }
}
