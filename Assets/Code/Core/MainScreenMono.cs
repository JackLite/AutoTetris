using Core.Input;
using Core.Scores;
using UnityEngine;

namespace Core
{
    public class MainScreenMono : MonoBehaviour
    {
        public RectTransform grid;

        [field:SerializeField]
        public ScoreView ScoreView { get; private set; }
    }
}