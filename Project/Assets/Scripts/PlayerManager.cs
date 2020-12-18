using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Singleton { get; private set; }

    [Header("Player Stats")]
    [SerializeField]
    private int _maxPoints = 1000;
    public int MaxPoints {
        get { return _maxPoints; }
        private set
        {
        }
    }

    [SerializeField]
    private int _currentPoints;

    [SerializeField]
    private int _currentLives;

    [SerializeField]
    [Tooltip("Amount of points needed to get a bonus life")]
    private int _bonusLifeGoal;

    private int _pointsForBonusLife;

    [Header("User Interface")]
    [SerializeField]
    private TextMeshProUGUI _livesText;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private Image _nextBlockPreview;

    private Sprite _nextBlockPreviewSprite;
    
    public Sprite NextBlockPreviewSprite
    {
        get
        {
            return _nextBlockPreviewSprite;
        }
        set
        {
            _nextBlockPreviewSprite = value;
            RefreshUi();
        }
    }

    private void Start()
    {
        RefreshUi();
    }

    public int CurrentLives
    {
        get { return _currentLives; }
        set
        {
            if (value == 0)
                Invoke(nameof(LoseGame), 1);

            if (value < 0)
                _currentLives = 0;
            else
                _currentLives = value;

            RefreshUi();
            Debug.Log($"Current lives are {_currentLives}");
        }
    }

    public int CurrentPoints
    {
        get { return _currentPoints; }
        set
        {
            if (value >= _maxPoints)
                Invoke(nameof(WinGame), 1);

            int addedPoints = value - _currentPoints;
            _currentPoints = value;
            PointsForBonusLife += addedPoints;

            RefreshUi();

            Debug.Log($"Current points are {_currentPoints}");
        }
    }

    private void RefreshUi()
    {
        _livesText.text = $"Remaining lives: <b>{_currentLives}</b>";
        _scoreText.text = $"Score: <b>{_currentPoints}</b> / <b>{_maxPoints}";
        _nextBlockPreview.sprite = _nextBlockPreviewSprite;
    }

    public int PointsForBonusLife
    {
        get { return _pointsForBonusLife; }
        set
        {
            _pointsForBonusLife = value;

            if (_pointsForBonusLife >= _bonusLifeGoal)
            {
                _pointsForBonusLife -= _bonusLifeGoal;
                CurrentLives++;
            }
        }
    }


    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoseGame()
    {
        Debug.Log($"Game lost!");
        RestartScene();
    }

    private void WinGame()
    {
        Debug.Log($"Game won!");
        RestartScene();
    }
}
