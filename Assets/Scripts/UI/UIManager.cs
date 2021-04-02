using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] string Level01;
    [SerializeField] string mainMenu;
    [SerializeField] string bootScene;
    [SerializeField] float delay = 0.5f;
    [SerializeField] TMP_Text scoreText;

    SceneLoader sceneLoader;
    CoinController coinController;

    string activeScene;
    int currentCoins;
    Scene[] loadedScenes;

    void OnEnable()
    {
        GameManager.PlayerSpawnedEvent += FindObjectOfTypeCoinController;
        CoinController.ScoreChangedEvent += UpdateScore;
    }

    void OnDisable()
    {
        GameManager.PlayerSpawnedEvent -= FindObjectOfTypeCoinController;
        CoinController.ScoreChangedEvent -= UpdateScore;
    }

    void Start()
    {
        
        sceneLoader = FindObjectOfType<SceneLoader>();

        coinController = FindObjectOfType<CoinController>();

        //       activeScene = SceneManager.GetActiveScene().name;

        //       print("[UIManager] activeScene " + activeScene);
        //        buildIndex = activeScene.buildIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("press Esc");

            GoToMainMenu();
        }
    }

    public void StartGameButton()
    {
        sceneLoader.LoadLevelAsync(Level01);
        sceneLoader.UnloadLevelAsync(mainMenu);
    }

    public void QuitGameButton()
    {
        sceneLoader.QuitGame();
    }

    void GoToMainMenu()
    {
        loadedScenes = sceneLoader.GetOpenScenes();

        foreach (Scene scene in loadedScenes)
        {
            activeScene = scene.name;

            print(activeScene);

            if (activeScene == bootScene)
            {
                continue;
            }

            if (activeScene == mainMenu)
            {
                continue;
            }

            Debug.Log("[UIManager] Update loop, foreach Scene scene in loadedScenes");

            StartCoroutine(GoToMainMenuCoroutine());

            break;
        }
    }

    IEnumerator GoToMainMenuCoroutine()
    {
        sceneLoader.LoadLevelAsync(mainMenu);
        yield return new WaitForSeconds(delay);
        sceneLoader.UnloadLevelAsync(loadedScenes[1].name);
        yield return new WaitForSeconds(delay);
    }

    void UpdateScore()
    {
        currentCoins = coinController.ReturnCurrentCoins();

        scoreText.text = currentCoins.ToString();
    }

    public void WriteToTextmeshPro(string inputString)
    {
        scoreText.text = inputString.ToString();
    }

    void FindObjectOfTypeCoinController()
    {
        coinController = FindObjectOfType<CoinController>();

        if (coinController == null)
        {
            Debug.LogError("[UIManager] coinController == null");
        }
    }
}
