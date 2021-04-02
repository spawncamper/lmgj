using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
//    [SerializeField] GameObject player;
 //   [SerializeField] Transform playerInitialPosition;
    [SerializeField] float endWaitTime = 0.5f;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Text messageText;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Transform playerSpawnPoint;


    ClickToMove playerMove;
    private bool gameWon = false;
    private bool roundEnded = false;

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
        messageText.text = "YOU WON";
        //        audioManager.PlayGameWonMusic();

        roundEnded = true;
        gameWon = false;

        yield return new WaitForSeconds(endWaitTime * 2);

        StartCoroutine(GameLoop());
    }


    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            GameObject playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
            playerInstance.transform.parent = gameObject.transform;

            if (PlayerSpawnedEvent != null)
                PlayerSpawnedEvent();
        }
    }
}