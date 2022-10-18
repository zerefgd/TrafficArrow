using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    #region START

    private bool hasGameFinished;

    public static GameplayManager Instance;

    private void Awake()
    {
        Instance = this;

        hasGameFinished = false;
        GameManager.Instance.IsInitialized = true;

        score = 0;
        currentLevel = 0;
        _scoreText.text = "0";

        scoreSpeed = _levelSpeed[currentLevel];

        for (int i = 0; i < 8; i++)
        {
            SpawnObstacle();
        }
    }

    #endregion

    #region  SCORE

    private float score;
    private float scoreSpeed;
    private int currentLevel;
    [SerializeField] private List<int> _levelSpeed, _levelMax;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private float _spawnX, _spawnY;

    private void Update()
    {
        if (hasGameFinished) return;

        score += scoreSpeed * Time.deltaTime;

        _scoreText.text = ((int)score).ToString();

        if (score > _levelMax[Mathf.Clamp(currentLevel, 0, _levelMax.Count - 1)])
        {
            SpawnObstacle();
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, _levelMax.Count - 1);
            scoreSpeed = _levelSpeed[currentLevel];
        }
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnX,_spawnX), Random.Range(-_spawnY, _spawnY),0f);
        RaycastHit2D hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
        bool canSpawn = hit;

        while(canSpawn)
        {
            spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0f);
            hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
            canSpawn = hit;
        }

        Instantiate(_obstaclePrefab, spawnPos, Quaternion.identity);
    }

    #endregion

    #region GAME_OVER

    [SerializeField] private AudioClip _loseClip;

    public void GameEnded()
    {
        SoundManager.Instance.PlaySound(_loseClip);
        hasGameFinished = true;
        GameManager.Instance.CurrentScore = (int)score;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }

    #endregion
}
