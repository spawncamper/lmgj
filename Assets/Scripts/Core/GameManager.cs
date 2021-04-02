using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
//    [SerializeField] GameObject player;
 //   [SerializeField] Transform playerInitialPosition;
    [SerializeField] float endWaitTime = 0.5f;
    [SerializeField] float messageTextTimerDelay = 2f;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] bool playIntro = true;

    AudioManager audioManager;
    ClickToMove playerMove;
    private bool gameWon = false;
    private bool roundEnded = false;
    bool isMusicOn = true;

    public delegate void RoundStarted();
    public static event RoundStarted RoundStartedEvent;

    public delegate void RoundEnded();
    public static event RoundEnded RoundEndedEvent;

    public delegate void PlayerSpawned();
    public static event PlayerSpawned PlayerSpawnedEvent;

    public delegate void GameWon();
    public static event GameWon GameWonEvent;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        yield return StartCoroutine(RoundPlaying());

        yield return StartCoroutine(RoundEnding());

        if (gameWon == false)
        {
            StartCoroutine(GameLoop());
        }
        else
        {
            if (GameWonEvent != null)
                GameWonEvent();

            StartCoroutine(GameWonCoroutine());
        }
    }

    IEnumerator RoundStarting()
    {        
        print("Round starting");

        if (playIntro)
        {
            yield return StartCoroutine(TutorialCoroutine());
        }

        SpawnPlayer();

        playerMove = FindObjectOfType<ClickToMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove is Null");
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

        print("Round playing");
        //        scorer = FindObjectOfType<Scorer>();

        while (roundEnded == false)
        {
            if (playerMove != null)
            {
                playerMove.MovePlayer();
            }

            yield return null;
        }
    }

    IEnumerator RoundEnding()
    {
        print("Round Ending");

        if (RoundEndedEvent != null)
            RoundEndedEvent();

        var playerInstance = FindObjectOfType<PlayerClass>();

        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        yield return endWaitTime;
    }

    IEnumerator GameWonCoroutine()
    {
        // messageText.text = "YOU WON";
        //        audioManager.PlayGameWonMusic();

        roundEnded = true;
        gameWon = false;

        yield return new WaitForSeconds(endWaitTime * 2);

        StartCoroutine(GameLoop());
    }


    void SpawnPlayer()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
            playerInstance.transform.parent = gameObject.transform;

            if (PlayerSpawnedEvent != null)
                PlayerSpawnedEvent();
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

    IEnumerator TutorialCoroutine()
    {
        messageText.text = "Welcome to RAGE OF RICHES";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = "AVOID THE ROBBERS";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = "DROP COINS AS BAIT";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = "GOOD LUCK";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = "...";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = "HAVE FUN";
        yield return new WaitForSeconds(messageTextTimerDelay);
        messageText.text = string.Empty;
    }
}