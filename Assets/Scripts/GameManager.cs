using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string MAIN_SCENE = "Main";

    public delegate void GameStateHandler();
    public delegate void GameSpeedHandler(float newSpeed);
    public event GameSpeedHandler GameSpeedChanged;
    public event GameStateHandler GameStartedEvent;
    public event GameStateHandler GameEndedEvent;

    [field: Header("References")]
    [field: SerializeField] public PlayerHealthController PlayerHealth { get; private set; }
    [field: SerializeField] public ObstacleSpawner ObstacleSpawner { get; private set; }
    [field: SerializeField] public LaneManager LaneManager { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public ColorContainer ColorContainer { get; private set; }


    [field: Header("Settings")]
    [field: SerializeField] public int ProjectileLayerIndex { get; private set; } = 6;
    [field: SerializeField] public float LoadGameOverSceneDelay { get; private set; } = 3f;
    [field: SerializeField] public float DecreasedSimulationSpeed { get; private set; } = .75f;
    [field: SerializeField] public float DecreasedSpeedDuration { get; private set; } = 2.5f;
    [field: SerializeField] public bool EnableMusic { get; private set; } = false;
    [field: SerializeField] public bool EnableSFX { get; private set; } = false;

    [field: SerializeField] public float ScreenFlashAlphaIntensity { get; private set; } = 200f;
    [field: SerializeField] public float ScreenFlashFadeInDuration { get; private set; } = .1f;
    [SerializeField] public float SimulationSpeed { get; private set; } = 0f;


    public bool IsGameActive { get; private set; } = false;

    private Coroutine decreasedSpeedRoutine = null;

    private int spawnerBudgetsEmptied = 0;

    private void OnEnable()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent += OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerGameOverEvent += OnPlayerDeathEvent;
        }

        if (ObstacleSpawner != null)
        {
            ObstacleSpawner.OnObjectBudgetEmpty += OnObstacleSpawnerEmptyEvent;
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.PlayerLoseLifeEvent -= OnPlayerLoseLifeEvent;
            PlayerHealth.PlayerGameOverEvent -= OnPlayerDeathEvent;
        }

        ObstacleSpawner.OnObjectBudgetEmpty -= OnObstacleSpawnerEmptyEvent;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
    private void Start()
    {
        if (MainCamera == null)
            MainCamera = Camera.main;

        AudioManager.Instance?.PlayGameMusic();
    }

    private void OnPlayerDeathEvent()
    {
        AudioManager.Instance?.PlayGameOverClip();
        SimulationSpeed = 0f;
        IsGameActive = false;
        GameEndedEvent?.Invoke();
    }

    private void OnPlayerLoseLifeEvent()
    {
        if (decreasedSpeedRoutine == null)
            StartCoroutine(DecreaseGameSpeedCoroutine());
    }

    private void OnObstacleSpawnerEmptyEvent()
    {
        spawnerBudgetsEmptied++;
        SimulationSpeed += 0.05f;
        GameSpeedChanged?.Invoke(SimulationSpeed);

        if (spawnerBudgetsEmptied == 1)
        {
            ObstacleSpawner.SetObjectBudgets(5, 4, 3, 2, 1);
        }
        else if (spawnerBudgetsEmptied == 2)
        {
            ObstacleSpawner.SetObjectBudgets(7, 6, 5, 3, 2);
        }
        else if (spawnerBudgetsEmptied == 3)
        {
            ObstacleSpawner.SetObjectBudgets(8, 6, 6, 4, 2);

        }
        else if (spawnerBudgetsEmptied >= 4)
        {

            ObstacleSpawner.SetObjectBudgets(8, 7, 6, 5, 3);
        }

        float obstaclesPerSpawn = ObstacleSpawner.ObjectsPerSpawnTick;
        if (ObstacleSpawner.ObjectsPerSpawnTick < 3f)
            obstaclesPerSpawn += .5f;
            
        ObstacleSpawner.SetSpawnSettings(ObstacleSpawner.SpawnDelay - 0.10f, obstaclesPerSpawn);
        ObstacleSpawner.ResetObstacleBudget();
    }

    private IEnumerator DecreaseGameSpeedCoroutine()
    {
        float normalSpeed = 1;

        SimulationSpeed = DecreasedSimulationSpeed;
        GameSpeedChanged?.Invoke(SimulationSpeed);

        yield return new WaitForSeconds(DecreasedSpeedDuration);

        SimulationSpeed = normalSpeed;
        GameSpeedChanged?.Invoke(SimulationSpeed);

        decreasedSpeedRoutine = null;
    }

    public Vector2 GetScreenBounds()
    {
        Vector2 screenBottomLeft = MainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenTopRight = MainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        return new Vector2(screenTopRight.x, screenTopRight.y);
    }
    #region GameStates

    public void StartGame()
    {
        SimulationSpeed = 1f;
        IsGameActive = true;
        GameStartedEvent?.Invoke();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
