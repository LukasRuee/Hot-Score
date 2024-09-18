using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerLeaderBoard : MonoBehaviour
{
    private string _leaderBoardKey = "LeaderBoard";
    [SerializeField] private TMP_Text playerNamesField;
    [SerializeField] private TMP_Text playerScoresField;
    private string playerNames;
    private string playerScores;
    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(LootLockerManager.Instance.PlayerName, scoreToUpload, _leaderBoardKey.ToString(), (response) =>
        {
            Debug.Log(LootLockerManager.Instance.PlayerName);
            if (response.success)
            {
                Debug.Log("Uploaded Score");
                done = true;
            }
            else
            {
                Debug.LogError("Failed "+ response.errorData);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public IEnumerator FetchTopHighScoreRoutine()
    {
        bool done = false;

        LootLockerSDKManager.GetScoreList(_leaderBoardKey.ToString(), 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Name\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                if(members != null)
                {
                    for (int i = 0; i < members.Length; i++)
                    {
                        tempPlayerNames += members[i].rank + ". ";
                        if (members[i].player.name != "")
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            tempPlayerNames += members[i].player.id;
                        }
                        tempPlayerScores += members[i].score + "\n";
                        tempPlayerNames += "\n";
                    }
                    playerNames = tempPlayerNames;
                    playerScores = tempPlayerScores;
                }
                else
                {
                    playerNames = "No Players yet";
                    playerScores = "No Scores yet";
                }
                done = true;
            }
            else
            {
                Debug.LogError("Failed" + response.errorData);
                done = true;
            }
        });

        playerNamesField.text = playerNames;
        playerScoresField.text = playerScores;

        yield return new WaitWhile(() => done == false);
    }
}
