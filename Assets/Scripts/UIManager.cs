using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] string Level01;
    [SerializeField] string mainMenu;
    [SerializeField] string bootScene;
    [SerializeField] float delay = 0.5f;
    SceneLoader sceneLoader;

    string activeScene;
    int buildIndex;
    Scene[] loadedScenes;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        
 //       activeScene = SceneManager.GetActiveScene().name;

 //       print("[UIManager] activeScene " + activeScene);
//        buildIndex = activeScene.buildIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("press Esc");
            
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

                StartCoroutine(GoToMainMenu());

                break;
            }
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

    IEnumerator GoToMainMenu()
    {
        sceneLoader.LoadLevelAsync(mainMenu);
        yield return new WaitForSeconds(delay);
        sceneLoader.UnloadLevelAsync(loadedScenes[1].name);
        yield return new WaitForSeconds(delay);
    }
}
