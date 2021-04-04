using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // on the BOOT object in the boot scene
    
    [SerializeField] GameObject corePrefab;
    [SerializeField] string mainMenu;
    [SerializeField] string GameScene01;
    [SerializeField] float delay = 1f;

    static bool corePrefabsSpawned = false;    // hasSpawned
    public static SceneLoader Instance;

    private string currentLevelName = string.Empty;

    List<AsyncOperation> loadOperations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("[SceneLoader] Second instance of GameManager detected and deleted");
        }

//        SceneLoader SceneLoader = FindObjectOfType<SceneLoader>();

        DontDestroyOnLoad(this);

        if (corePrefabsSpawned == true)
        {
            return;
        }
        else if (corePrefabsSpawned == false)
        {
            corePrefabsSpawned = true;
            SpawnCorePrefabs();
        }

        loadOperations = new List<AsyncOperation>();

        StartCoroutine(OpenMainMenu());
    }

    IEnumerator OpenMainMenu()
    {
        LoadLevelAsync(mainMenu);
        Debug.Log("[SceneLoader] Open main menu");
        yield return new WaitForSeconds(delay);
    }

    public void LoadLevelAsync(string levelName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        if (asyncOp == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);

            return;
        }

        asyncOp.completed += OnLoadOperationComplete;
        loadOperations.Add(asyncOp);
        currentLevelName = levelName;
    }

    public void UnloadLevelAsync(string levelName)
    {
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(levelName);

        if (asyncOp == null)
        {
            Debug.LogError("[SceneLoader] Unable to unload scene " + levelName);

            return;
        }

        asyncOp.completed += OnUnloadOperationComplete;
    }

    void OnLoadOperationComplete(AsyncOperation asyncOp)
    {
        if (loadOperations.Contains(asyncOp))
        {
            loadOperations.Remove(asyncOp);

            if (loadOperations.Count == 0)
            {
                //               UpdateState(GameState.RUNNING);
            }
        }

        Debug.Log("Load complete");
    }

    void OnUnloadOperationComplete(AsyncOperation asyncOp)
    {
        Debug.Log("Unload complete");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SpawnCorePrefabs()
    {
        GameObject persistentObjects = Instantiate(corePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        DontDestroyOnLoad(persistentObjects);
    }

    public Scene[] GetOpenScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        return loadedScenes;
    }
}