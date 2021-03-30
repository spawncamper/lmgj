using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform playerInitialPosition;
    [SerializeField] float endWaitTime = 0.5f;
    [SerializeField] ClickToMove movePlayer;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Text messageText;
    private bool gameWon = false;
    private bool roundEnded = false;
    Timer timer;
    
    public delegate void RoundStarted();
    public static event RoundStarted RoundStartedEvent;

    public delegate void GameWon();
    public static event GameWon GameWonEvent;

    private void OnEnable()
    {
        timer = GetComponent<Timer>();
    }

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

        player.transform.position = playerInitialPosition.position;
        timer.ResetTimer();
        roundEnded = false;

        yield return new WaitForSeconds(endWaitTime);
    }

    IEnumerator RoundPlaying()
    {
//        scorer = FindObjectOfType<Scorer>();

        while (roundEnded == false)
        {
            movePlayer.MovePlayer();

            timer.UpdateTimer();

            yield return null;
        }
    }

    IEnumerator RoundEnding()
    {
        timer.ResetTimer();

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