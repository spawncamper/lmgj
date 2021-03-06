using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Location: Game Manager object in the Scene hierarchy
    //
    //    [SerializeField] GameObject player;
    //   [SerializeField] Transform playerInitialPosition;
    [SerializeField] float endWaitTime = 0.5f;
    [SerializeField] float messageTextTimerDelay = 2f;
    [SerializeField] float gameOverDelay = 5f;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] TMP_Text tutorialText;

    [SerializeField] bool playIntro = true;

    AudioManager audioManager;
    ClickToMove playerMove;
    SceneLoader sceneLoader;

    private bool gameWon = false;
    public bool roundEnded = false;
    bool isMusicOn = true;

    public static bool isPlayerDead = true;

    public delegate void RoundStarted();
    public static event RoundStarted RoundStartedEvent;

    public delegate void PlayerSpawned();
    public static event PlayerSpawned PlayerSpawnedEvent;

    public delegate void GameWon();
    public static event GameWon GameWonEvent;

    public delegate void GameOver();
    public static event GameOver GameOverEvent;

    void OnEnable()
    {
        EnemyController.PlayerDeathEvent += PlayerDeathEvent;
    }

    void OnDisable()
    {
        EnemyController.PlayerDeathEvent -= PlayerDeathEvent;
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        yield return StartCoroutine(RoundPlaying());

        yield return StartCoroutine(RoundEnding());

        if (gameWon == false)
        {
            if (GameOverEvent != null)
                GameOverEvent();

            print("[GameManager] Round LOST -- GAME OVER");

            sceneLoader = FindObjectOfType<SceneLoader>();

            if (sceneLoader == null)
            {
                Debug.LogError("[GameManager] GameLoop() sceneLoader == null");
            }
            
            sceneLoader.GoToMainMenu();

            yield return new WaitForSeconds(gameOverDelay * 5);

            Debug.LogError("[GameManager] " + gameObject.name + "gameOverDelay exceeded");

            //            StartCoroutine(GameLoop());
        }
        else
        {
            Debug.LogWarning("[GameManager] GameLoop() gameWon != false");

            if (GameWonEvent != null)
                GameWonEvent();

            StartCoroutine(GameWonCoroutine());
        }
    }

    IEnumerator RoundStarting()
    {        
        print("Round starting");

        audioManager = FindObjectOfType<AudioManager>();

        if (playIntro)
        {
            yield return StartCoroutine(TutorialCoroutine());
        }

        SpawnPlayer();

        isPlayerDead = false;

        playerMove = FindObjectOfType<ClickToMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove is Null");

            isPlayerDead = true;
        }

        //        PlayBackgroundMusic();

        //       player.transform.position = playerInitialPosition.position;

        roundEnded = false;

        yield return new WaitForSeconds(endWaitTime);

        if (RoundStartedEvent != null)
            RoundStartedEvent();
    }

    IEnumerator RoundPlaying()
    {

        print("Round playing Started");
        //        scorer = FindObjectOfType<Scorer>();

        isPlayerDead = false;

        while (roundEnded == false)
        {
           // print(roundEnded);
            
            if (playerMove != null)
            {
                playerMove.MovePlayer();
            }

            yield return null;
        }

        print("Round playing Ended");
    }

    IEnumerator RoundEnding()
    {        
        print("Round Ending Started");

        //        isPlayerDead = false;

//        DestroyPlayerObject();

        yield return endWaitTime;
    }

    IEnumerator GameWonCoroutine()
    {
        // messageText.text = "YOU WON";
        //        audioManager.PlayGameWonMusic();

 //       isPlayerDead = true;

        roundEnded = true;
        gameWon = false;

        yield return new WaitForSeconds(endWaitTime * 2);

        StartCoroutine(GameLoop());
    }


    void SpawnPlayer()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            if (FindObjectOfType<PlayerClass>() == null)
            {
                GameObject playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
                playerInstance.transform.parent = gameObject.transform;

                if (PlayerSpawnedEvent != null)
                    PlayerSpawnedEvent();

                isPlayerDead = false;
            }
            else
            {
                Debug.LogError("[GameManager] SpawnPlayer() PlayerClass is not Null ");
            }
        }
        else if (playerPrefab == null)
        {
            Debug.LogError("[GameManager] SpawnPlayer() playerPrefab is null");
        }
        else if (playerSpawnPoint == null)
        {
            Debug.LogError("[GameManager] SpawnPlayer() playerSpawnPoint is null");
        }
    }

    void PlayerDeathEvent()
    {
        // Game over text
        print("[GameManager] PlayerDeath");
        isPlayerDead = true;
        roundEnded = true;
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

    IEnumerator TutorialCoroutine()
    {
        print("[GameManager] Tutorial Coroutine has started");
        
        tutorialText.text = "HELLO ADVENTURER!";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.75f);
        tutorialText.text = "WELCOME TO RAGE OF RICHES";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.75f);
        tutorialText.text = "AVOID THE ROBBERS";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.75f);
        tutorialText.text = "DROP COINS AS BAIT";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.50f);
        tutorialText.text = "GOOD LUCK";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.25f);
        tutorialText.text = "...";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.25f);
        tutorialText.text = "HAVE FUN";
        yield return new WaitForSeconds(messageTextTimerDelay * 0.25f);
        tutorialText.text = string.Empty; 
    }
}