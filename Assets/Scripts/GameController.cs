using LootLocker.Requests;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static public GameController Instance;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _leaderBoardUI;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _nameInput;
    [SerializeField] private TMP_Text _scoreUI;
    private string _playerName;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if(_scoreUI != null)
        {
            _scoreUI.text = _playerController.Score.ToString();
        }
    }
    public void CreateLeaderboard()
    {
        StartCoroutine(LootLockerManager.Instance.LeaderBoard.FetchTopHighScoreRoutine());
    }
    public void EndGame()
    {
        _menuUI.SetActive(false);
        _nameInput.SetActive(true);
    }
    public void CloseApp()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void ReturnToMenu()
    {
        StartCoroutine(EndGameRoutine());
    }
    IEnumerator EndGameRoutine()
    {
        _menuUI.SetActive(false);
        _leaderBoardUI.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        yield return LootLockerManager.Instance.LeaderBoard.SubmitScoreRoutine(_playerController.Score);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
