using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of all spawnable blocks")]
    private List<GameObject> _blocks;

    [SerializeField]
    private GameObject _audioManagerObject;

    [SerializeField]
    private Camera _mainCamera;

    private GameObject _blockToSpawn;

    private Vector2 _currentHighestPosition;

    private Vector2 _currentPositionOffset;

    private Vector2 _currentSpawnPosition;

    private void Awake()
    {
        _currentHighestPosition = new Vector2(0, -4.6f);
        _currentSpawnPosition = transform.position;
    }

    void Start()
    {
        _blockToSpawn = NextBlock();
        HandleNext();
    }

    public void HandleNext(Transform landedBlock = null)
    {
        int currentLives = PlayerManager.Singleton.CurrentLives;
        int currentPoints = PlayerManager.Singleton.CurrentPoints;
        int maxPoints = PlayerManager.Singleton.MaxPoints;
        if (currentLives > 0 && currentPoints < maxPoints)
        {
            SpawnBlock();
            _blockToSpawn = NextBlock();
            SetNextBlockSprite();
        }

        if (landedBlock != null && landedBlock.position.y > _currentHighestPosition.y)
        {
            UpdatePositions(landedBlock);
        }

    }

    private void UpdatePositions(Transform landedBlock)
    {
        _currentPositionOffset.y = landedBlock.position.y - _currentHighestPosition.y;
        _currentSpawnPosition.y += _currentPositionOffset.y;
        _currentHighestPosition.y = landedBlock.position.y;
        Vector3 cameraPosition = _mainCamera.transform.position;
        cameraPosition.y += _currentPositionOffset.y;
        _mainCamera.transform.position = cameraPosition;
    }

    private void SetNextBlockSprite()
    {
        SpriteRenderer renderer = _blockToSpawn.GetComponent<SpriteRenderer>();
        PlayerManager.Singleton.NextBlockPreviewSprite = renderer.sprite;
    }

    private void SpawnBlock()
    {
        GameObject newBlock = Instantiate(_blockToSpawn, _currentSpawnPosition, transform.rotation);
        SetUpBlock(newBlock);
    }

    private GameObject NextBlock()
    {
        int randomIndex = Random.Range(0, _blocks.Count);
        return _blocks[randomIndex];
    }

    private void SetUpBlock(GameObject block)
    {
        BlockBehaviour blockBehaviour = block.GetComponent<BlockBehaviour>();
        blockBehaviour.Manager = this;
        AudioManager audioManager = _audioManagerObject.GetComponent<AudioManager>();
        blockBehaviour.AudioManager = audioManager;
        ScoreSpeedIncrease(blockBehaviour);
    }

    private void ScoreSpeedIncrease(BlockBehaviour block)
    {
        float speedIncrease = (float)PlayerManager.Singleton.CurrentPoints / PlayerManager.Singleton.MaxPoints;
        block.ScoreSpeedModifier += speedIncrease;
    }
}