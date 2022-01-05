using Core.Moving;
using Core.Pause;
using Core.Scores;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class MainScreenMono : MonoBehaviour
    {
        public RectTransform grid;

        public MovingData movingData;
        
        [field:SerializeField]
        public ScoreView ScoreView { get; private set; }

        [field:SerializeField]
        public Button PauseButton { get; private set; }

        [field:SerializeField]
        public Button UnPauseButton { get; private set; }

        [field:SerializeField]
        public GameObject PauseScreen { get; private set; }

        [field:SerializeField]
        public NextFigureUI NextFigure { get; private set; }
    }
}