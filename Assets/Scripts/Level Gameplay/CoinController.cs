using UnityEngine;

public class CoinController : MonoBehaviour
{
    // Location: on the same object as GameManager in the Scene

    [SerializeField] int initialCoins = 3;
    [SerializeField] int oneCoin = 1;
    [SerializeField] int enemyDeathReward = 10;

    int currentCoins;

    public delegate void ScoreChanged();
    public static event ScoreChanged ScoreChangedEvent;

    private void OnEnable()
    {
        CoinSpawner.CoinSpawnedEvent += CoinSpawnEvent;
        EnemyController.EnemyDeathEvent += EnemyDeath;
    }

    private void OnDisable()
    {
        CoinSpawner.CoinSpawnedEvent -= CoinSpawnEvent;
        EnemyController.EnemyDeathEvent -= EnemyDeath;
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

    void CoinSpawnEvent()
    {
        AddCoins(oneCoin * (-1));
    }

    void EnemyDeath()
    {
        AddCoins(enemyDeathReward);
    }
}
