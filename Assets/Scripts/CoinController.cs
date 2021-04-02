using UnityEngine;

public class CoinController : MonoBehaviour
{
    GameObject metal;
    [SerializeField] int initialCoins = 3;
    [SerializeField] GameObject coin;
    int currentCoins;

//    public delegate void CoinSpawned();
//    public static event CoinSpawned CoinSpawnedEvent;

    public delegate void ScoreChanged();
    public static event ScoreChanged ScoreChangedEvent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (currentCoins >= 1)
            {
                Vector3 CoinPosition = new Vector3(transform.position.x, 0.65f, transform.position.z);
                Instantiate(coin, CoinPosition, Quaternion.identity);

                if (ScoreChangedEvent != null)
                    ScoreChangedEvent();

                currentCoins--;
            }
            else
            {
                Debug.LogWarning("[CoinController] currentCoins = 0");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCoins = initialCoins;

        if (ScoreChangedEvent != null)
            ScoreChangedEvent();
    }

    public void AddCoins(int number)
    {
        currentCoins = currentCoins + number;

        if (ScoreChangedEvent != null)
            ScoreChangedEvent();
    }

    public int ReturnCurrentCoins()
    {
        return currentCoins;
    }

    void ResetCurrentCoins()
    {
        currentCoins = 0;

        if (ScoreChangedEvent != null)
            ScoreChangedEvent();
    }
}
