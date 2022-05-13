using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace Global.Leaderboard.Services
{
    public class ScoresService
    {
        public void LoadScores(int count, Action<List<ScoreData>> callback)
        {
            Debug.Log("[Scores] Load scores...");
            PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_main,
                LeaderboardStart.PlayerCentered,
                20,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                data =>
                {
                    Debug.Log("[Scores] Scores load status = " + data.Status);
                    if (data.Status != ResponseStatus.Success)
                        return;
                    Debug.Log("[Scores] Load users... ");
                    Debug.Log("[Scores] Scores loaded. Count " + data.Scores.Length);
                    Debug.Log("[Scores] Data: Id = " + data.Id + "; title = " + data.Title
                    + "; user scores = " + data.PlayerScore?.value);
                    foreach (var dataScore in data.Scores)
                    {
                        Debug.Log("[Scores] Raw score: " + dataScore.userID + " --- " + dataScore.date +
                                  " --- " + dataScore.value + " --- " + dataScore.leaderboardID);

                    }
                    LoadUsers(data.Scores.Select(s => s.userID).ToArray(),
                        users =>
                        {
                            Debug.Log("[Scores] Users loaded. Count " + users.Count);

                            var res = new List<ScoreData>(count);
                            foreach (var s in data.Scores)
                            {
                                if (!users.ContainsKey(s.userID))
                                {
                                    Debug.LogError("Can't find user with id " + s.userID);
                                    continue;
                                }
                                var scoreData = new ScoreData(s.value, users[s.userID])
                                {
                                    place = s.rank
                                };
                                res.Add(scoreData);
                            }
                            Debug.Log("[Scores] Invoke callback. Callback is null = " + (callback == null));
                            callback?.Invoke(res);
                        });
                });
        }

        private void LoadUsers(string[] userIds, Action<Dictionary<string, string>> callback)
        {
            // load the profiles and display (or in this case, log)
            PlayGamesPlatform.Instance.LoadUsers(userIds,
                (users) =>
                {
                    Debug.Log("[Scores] Load users. Users loaded.");
                    if (users == null)
                    {
                        Debug.Log("[Scores] Load users. Users not loaded!");
                        callback?.Invoke(new Dictionary<string, string>());
                        return;
                    }
                    var res = users.ToDictionary(profile => profile.id, profile => profile.userName);
                    Debug.Log("[Scores] Load users. call callback");

                    callback?.Invoke(res);
                });
        }
    }
}