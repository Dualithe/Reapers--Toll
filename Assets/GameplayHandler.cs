using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayHandler : MonoBehaviour
{
    public static GameplayHandler Instance { get; private set; }

    [SerializeField]
    private Camera cameraReference;
    public Camera Camera => cameraReference;
    public InputHandler inputHandler;
    public UIHandler uiHandler;
    public ItemSpawner itemSpawner;
    public EnemySpawner enemySpawner;
    public Transform player;

    public GameObject menuPrefab;
    public GameObject endGamePrefab;
    public GameObject wonGamePrefab;
    public GameObject menu;
    public GameObject endGame;
    public GameObject wonGame;
    public GameObject[] gameplayObjects;
    public AudioSource sound;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // FIX 1: persist across scenes

        SceneManager.sceneLoaded += OnSceneLoaded; // FIX 2: refresh references every load
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ShowMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignObjects(); // runs after every scene load!
    }

    private void AssignObjects()
    {
        cameraReference = Camera.main;
        uiHandler = FindFirstObjectByType<UIHandler>();
        itemSpawner = FindFirstObjectByType<ItemSpawner>();
        inputHandler.controller = FindFirstObjectByType<ScytheController>();
        var pHandler = FindFirstObjectByType<PlayerHandler>();
        player = pHandler ? pHandler.transform : null;
        gameplayObjects = GameObject.FindGameObjectsWithTag("gameplayObject"); // Add unique tag to all gameplay objects
        itemSpawner.allowSpawning = true;
    }


    public void ShowMenu()
    {
        foreach (var go in gameplayObjects)
        {
            go.SetActive(false);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Destroy(g);
        }

        if (!menu)
            menu = Instantiate(menuPrefab);
        
        if (!endGame)
            endGame = Instantiate(endGamePrefab);
        
        if (!wonGame)
            wonGame = Instantiate(wonGamePrefab);
        
        endGame.SetActive(false);
        wonGame.SetActive(false);
        menu.SetActive(true);
        inputHandler.inputAllowed = false;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(0);
        
        if (!menu)
            menu = Instantiate(menuPrefab);
        
        if (!endGame)
            endGame = Instantiate(endGamePrefab);
        
        if (!wonGame)
            wonGame = Instantiate(wonGamePrefab);
        
        endGame.SetActive(false);
        menu.SetActive(false);
        wonGame.SetActive(false);
        
        foreach (var go in gameplayObjects)
        {
            go.SetActive(true);
        }

        inputHandler.inputAllowed = true;

        AssignObjects();

        uiHandler.UpdateUpgradeButtonTexts();
        
        if (itemSpawner != null)
            itemSpawner.allowSpawning = true;
        else
            Debug.LogWarning("ItemSpawner is not assigned.");

        enemySpawner.LoadWaveData();
    }
    
    public void GameOver()
    {
        if (!menu)
            menu = Instantiate(menuPrefab);
        
        if (!endGame)
            endGame = Instantiate(endGamePrefab);
        
        if (!wonGame)
            wonGame = Instantiate(wonGamePrefab);
        
        inputHandler.inputAllowed = false;
        endGame.SetActive(true);
        wonGame.SetActive(false);
        menu.SetActive(false);
        foreach (var go in gameplayObjects)
        {
            go.SetActive(false);
        }
    }
    
    public void GameWon()
    {
        if (!menu)
            menu = Instantiate(menuPrefab);
        
        if (!endGame)
            endGame = Instantiate(endGamePrefab);
        
        if (!wonGame)
            wonGame = Instantiate(wonGamePrefab);
        
        inputHandler.inputAllowed = false;
        endGame.SetActive(false);
        wonGame.SetActive(true);
        menu.SetActive(false);
        foreach (var go in gameplayObjects)
        {
            go.SetActive(false);
        }
    }
}