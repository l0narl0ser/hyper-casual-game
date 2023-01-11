using UnityEngine;

namespace Core
{
    public class SnapshotManager
    {
        private const string PlayerScoreKey = "playerScoreKey";
        private int _playerScore;


        public void Reset()
        {
            _playerScore = 0;
            PlayerPrefs.DeleteAll();
        }

        public int GetScore()
        {
            return _playerScore;
        }

        public void SetScore(int score)
        {
            _playerScore = score;
        }
        public void Save()
        {
            PlayerPrefs.SetInt(PlayerScoreKey, _playerScore);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(PlayerScoreKey))
            {
                _playerScore = PlayerPrefs.GetInt(PlayerScoreKey);
            }
            else
            {
                _playerScore = 0;
            }
        }
    }
}