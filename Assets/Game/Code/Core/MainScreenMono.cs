using Core.Cells.Visual;
using Core.CoreDebug;
using Core.Grid.Visual;
using Core.Input;
using Core.Pause;
using Core.Scores;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class MainScreenMono : MonoBehaviour
    {
        public RectTransform grid;

        [field:SerializeField]
        public ScoreView ScoreView { get; private set; }

        [field:SerializeField]
        public Button PauseButton { get; private set; }

        [field:SerializeField]
        public Button UnPauseButton { get; private set; }

        [field:SerializeField]
        public PauseScreenView PauseScreen { get; private set; }

        [field:SerializeField]
        public NextFigureUI NextFigure { get; private set; }

        [field:SerializeField]
        public SwipeMono SwipeMono { get; private set; }

        [field:SerializeField]
        public GlowEffectPool GlowEffectPool { get; private set; }

        [field:SerializeField]
        public GridView GridView { get; private set; }

        [field:SerializeField]
        public ShadowCellsController ShadowCellsController { get; private set; }

        [field:SerializeField]
        public TextMeshProUGUI GeneticText { get; private set; }

        [field:Header("Debug")]
        [field:SerializeField]
        public GameObject DebugInPause { get; private set; }

        [field:SerializeField]
        public DebugMono DebugMono { get; private set; }
    }
}