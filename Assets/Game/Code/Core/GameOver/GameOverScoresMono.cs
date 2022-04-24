using UnityEngine;

namespace Core.GameOver
{
    public class GameOverScoresMono : MonoBehaviour
    {
        [SerializeField]
        private GameObject newMaxGO;

        public void SetNewMaxState()
        {
            newMaxGO.SetActive(true);
        }
    }
}