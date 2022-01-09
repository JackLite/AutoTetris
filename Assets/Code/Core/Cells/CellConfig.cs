using UnityEngine;

namespace Core.Cells
{
    [CreateAssetMenu(fileName = "CellConfig", menuName = "Cells/Cell Config", order = 0)]
    public class CellConfig : ScriptableObject
    {
        [field:SerializeField]
        public float MoveSpeed { get; private set; }
        
        [field:SerializeField]
        public float Distance { get; private set; }
    }
}