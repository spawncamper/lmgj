using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject corePrefab;

    static bool corePrefabsSpawned = false;    // hasSpawned
    public static SceneLoader Instance;

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

        SceneLoader SceneLoader = FindObjectOfType<SceneLoader>();

        if (SceneLoader == null)
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
}
