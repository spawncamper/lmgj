using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Location: in each game scene, on the parent Canvas object
    
    [SerializeField] string Level01;
    [SerializeField] string mainMenu;
    [SerializeField] string bootScene;
    [SerializeField] float delay = 0.5f;
    [SerializeField] TMP_Text scoreText;
   

    SceneLoader sceneLoader;
    CoinController coinController;
    AudioManager audioManager;

    bool isMusicOn = true;
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
        audioManager = FindObjectOfType<AudioManager>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        coinController = FindObjectOfType<CoinController>();
        scoreText = GetComponentInChildren<TMP_Text>();

        //       activeScene = SceneManager.GetActiveScene().name;

        //       print("[UIManager] activeScene " + activeScene);
        //        buildIndex = activeScene.buildIndex;
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

    void UpdateScore()
    {
        if (coinController == null)
            Debug.LogError("[UIManager] coinController is null");

        currentCoins = coinController.ReturnCurrentCoins();

        if (scoreText == null)
        {
            Debug.LogError("[UIManager] scoreText is null");
        }

        scoreText.text = currentCoins.ToString();
    }

    void FindObjectOfTypeCoinController()
    {
        coinController = FindObjectOfType<CoinController>();

        if (coinController == null)
        {
            Debug.LogError("[UIManager] coinController == null");
        }
    }


    public void ToggleMusic()
    {
        if (isMusicOn == true)
        {
            print("click + isMusicOn = " + isMusicOn);
            audioManager.StopMusic();
            isMusicOn = false;
        }
        else
        {
            print("click + isMusicOn = " + isMusicOn);
            audioManager.PlayBackgroundMusic();
            isMusicOn = true;
        }
    }

}