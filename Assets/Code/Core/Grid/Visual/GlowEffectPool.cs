using System.Collections.Generic;
using UnityEngine;

namespace Core.Grid.Visual
{
    public class GlowEffectPool : MonoBehaviour
    {
        [SerializeField]
        private GlowEffectView glowPrefab;

        [SerializeField]
        private int preCreateCount = 4;

        private readonly Stack<GlowEffectView> _pool = new Stack<GlowEffectView>(4);

        public void WarmUp()
        {
            for (var i = preCreateCount; i > 0; --i)
            {
                CreateInstance();
            }
        }

        public GlowEffectView Get()
        {
            if (_pool.Count == 0)
                CreateInstance();
            return _pool.Pop();
        }

        public void Return(GlowEffectView view)
        {
            view.Reset();
            _pool.Push(view);
        }

        private void CreateInstance()
        {
            var instance = Instantiate(glowPrefab, transform);
            instance.Reset();
            _pool.Push(instance);
        }
    }
}