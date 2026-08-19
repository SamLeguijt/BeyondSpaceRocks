using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovementController : MonoBehaviour
{
    public delegate void PlayerMoveLanesHandler(Lane targetLane);

    public event PlayerMoveLanesHandler StartLaneSwapEvent;
    public event PlayerMoveLanesHandler StopLaneSwapEvent;

    public bool AllowedToMove { get; private set; } = false;
    public Lane CurrentLane { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerHealthController playerHealthController = null;
    [SerializeField] private PlayerColorController playerColorController = null;
    [SerializeField] private LaneManager laneManager = null;
    [SerializeField] private ParticleSystem moveParticles = null;

    [Header("Movement settings")]
    [SerializeField] private float laneSwapSpeed = 3f;

    private Coroutine swapLanesCoroutine = null;
    private bool isSwappingLanes = false;

    private void OnEnable()
    {
        if (inputController == null)
        {
            Debug.LogWarning($"InputController reference is missing on {this}, please assign and re run to use movement logic.");
            return;
        }

        inputController.MoveLeftInput += OnLeftMoveInputReceivedEvent;
        inputController.MoveRightInput += OnRightMoveInputReceivedEvent;

        playerHealthController.PlayerGameOverEvent += OnPlayerDeathEvent;

        playerColorController.OnColorChanged += OnColorChangedEvent;
    }

    private void OnDisable()
    {
        inputController.MoveLeftInput -= OnLeftMoveInputReceivedEvent;
        inputController.MoveRightInput-= OnRightMoveInputReceivedEvent;

        playerHealthController.PlayerGameOverEvent -= OnPlayerDeathEvent;
    
        playerColorController.OnColorChanged -= OnColorChangedEvent;
    }


    private void Start()
    {
        player.position = new Vector3(CurrentLane.Position.x, player.position.y, player.position.z);
        CurrentLane = laneManager.MiddleLane;

        GameManager.Instance.GameStartedEvent += OnGameStartEvent;
        GameManager.Instance.GameEndedEvent += OnGameEndedEvent;
    }

    private void OnGameStartEvent()
    {
        AllowedToMove = true;
    }

    private void OnGameEndedEvent()
    {
        AllowedToMove = false;
    }

    private void OnLeftMoveInputReceivedEvent()
    {
        if (!AllowedToMove)
            return;

        if (isSwappingLanes || CurrentLane.LaneType == LaneEnum.Left)
            return;

        Lane targetLane = CurrentLane;

        switch (CurrentLane.LaneType)
        {
            case LaneEnum.Middle:
                targetLane = laneManager.LeftLane;
                break;
            case LaneEnum.Right:
                targetLane = laneManager.MiddleLane;
                break;
            default:
                break;
        }

        if (targetLane.LaneType != CurrentLane.LaneType)
            MoveToLane(targetLane, true);
    }

    private void OnRightMoveInputReceivedEvent()
    {
        if (!AllowedToMove)
            return;

        if (isSwappingLanes || CurrentLane.LaneType == LaneEnum.Right)
            return;

        Lane targetLane = CurrentLane;

        switch (CurrentLane.LaneType)
        {
            case LaneEnum.Left:
                targetLane = laneManager.MiddleLane;
                break;
            case LaneEnum.Middle:
                targetLane = laneManager.RightLane;
                break;
            default:
                break;
        }

        if (targetLane.LaneType != CurrentLane.LaneType)
            MoveToLane(targetLane, false);
    }

    private void MoveToLane(Lane targetLane, bool movesLeft = false)
    {
        if (!AllowedToMove)
            return;

        if (swapLanesCoroutine != null)
            StopCoroutine(swapLanesCoroutine);

        swapLanesCoroutine = StartCoroutine(MoveLaneCoroutine(targetLane));
        PlayMoveParticles(movesLeft);
    }

    private IEnumerator MoveLaneCoroutine(Lane targetLane)
    {
        float elapsedTime = 0;
        float maxTime = 10;

        StartLaneSwapEvent?.Invoke(targetLane);
        isSwappingLanes = true;
        AudioManager.Instance?.PlayMovementSFX();

        while (elapsedTime < maxTime)
        {
            // Keep the players' Y pos to 
            Vector2 targetPos = new Vector3(targetLane.Position.x, player.position.y);

            player.position = Vector2.MoveTowards(player.position, targetPos, laneSwapSpeed * GameManager.Instance.SimulationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            if (player.position.x == targetLane.Position.x)
            {
                break;
            }

            yield return null;
        }

        CurrentLane = targetLane;
        StopLaneSwapEvent?.Invoke(targetLane);
        isSwappingLanes = false;
    }

    private void PlayMoveParticles(bool toLeft)
    {
        float particleRotation = 0;

        if (toLeft)
            particleRotation = 0;
        else
            particleRotation = 180f;

        moveParticles.transform.rotation = Quaternion.Euler(transform.rotation.x, particleRotation, transform.rotation.z);
        moveParticles.Play();
    }

    private void OnColorChangedEvent(ColorData colorData)
    {
        var particlesMain = moveParticles.main;
        particlesMain.startColor = playerColorController.CurrentColor.Color;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[moveParticles.particleCount];
        int numParticlesAlive = moveParticles.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].startColor = playerColorController.CurrentColor.Color;
        }

        moveParticles.SetParticles(particles, numParticlesAlive);
    }


    private void OnPlayerDeathEvent()
    {
        AllowedToMove = false;
    }
}

