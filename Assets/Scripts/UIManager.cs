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
    Score score;
    SceneLoader sceneLoader;

    string activeScene;
    int buildIndex;
    int currentScore;
    Scene[] loadedScenes;

    void OnEnable()
    {
        Score.ScoreChangedEvent += UpdateScore;
    }

    void OnDisable()
    {
        Score.ScoreChangedEvent -= UpdateScore;
    }

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        score = FindObjectOfType<Score>();
        
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
        currentScore = score.GetScore();

        scoreText.text = currentScore.ToString();
    }
}
