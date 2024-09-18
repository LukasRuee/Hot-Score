using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootLockerManager : MonoBehaviour
{
    static public LootLockerManager Instance;
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private PlayerController _playerController;
    [field: SerializeField] public LootLockerLeaderBoard LeaderBoard {get; private set; }
    public string PlayerName { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(SetUpRoutine());
    }
    #region LogIn
    private IEnumerator SetUpRoutine()
    {
        yield return LogInRoutine();
        yield return LeaderBoard.FetchTopHighScoreRoutine();
    }
    private IEnumerator LogInRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("PLayer was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    #endregion
    #region Submission
    public void SubmitScore()
    {
        StartCoroutine(SubmitRoutine());
    }
    private IEnumerator SubmitRoutine()
    {
        yield return SubmitName();
        yield return LeaderBoard.SubmitScoreRoutine(_playerController.Score);
        yield return LeaderBoard.FetchTopHighScoreRoutine();
        GameController.Instance.CreateLeaderboard();
    }
    private IEnumerator SubmitName()
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(PlayerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Playername set");
                done = true;
            }
            else
            {
                Debug.Log("Could not set playername set: " + response.errorData);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    #endregion
    public void SetPlayerName()
    {
        PlayerName = _playerNameInput.text;
    }
}
