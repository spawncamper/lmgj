using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
 //   [SerializeField] Transform playerInitialPosition;
    [SerializeField] float endWaitTime = 0.5f;
    [SerializeField] ClickToMove movePlayer;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Text messageText;
    private bool gameWon = false;
    private bool roundEnded = false;
    
    public delegate void RoundStarted();
    public static event RoundStarted RoundStartedEvent;

    public delegate void Tick();
    public static event Tick TickEvent;

    public delegate void GameWon();
    public static event GameWon GameWonEvent;

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        if (RoundStartedEvent != null)
            RoundStartedEvent();

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
//        PlayBackgroundMusic();

 //       player.transform.position = playerInitialPosition.position;
 
        roundEnded = false;

        yield return new WaitForSeconds(endWaitTime);
    }

    IEnumerator RoundPlaying()
    {
//        scorer = FindObjectOfType<Scorer>();

        while (roundEnded == false)
        {
            movePlayer.MovePlayer();

            yield return null;
        }
    }

    IEnumerator RoundEnding()
    {

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
}