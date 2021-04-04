using UnityEngine;

public class CoinController : MonoBehaviour
{
    // Location: on the same object as GameManager in the Scene
    GameObject metal;
    [SerializeField] int initialCoins = 3;

    GameManager gameManager;

    int currentCoins;

    public delegate void ScoreChanged();
    public static event ScoreChanged ScoreChangedEvent;

    private void OnEnable()
    {
        CoinSpawner.CoinSpawnedEvent += SpawnCoin;
    }

    private void OnDisable()
    {
        CoinSpawner.CoinSpawnedEvent -= SpawnCoin;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCoins = initialCoins;

        if (ScoreChangedEvent != null)
            ScoreChangedEvent();

        gameManager = FindObjectOfType<GameManager>();
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

    void SpawnCoin()
    {
        AddCoins(1);
    }
}
