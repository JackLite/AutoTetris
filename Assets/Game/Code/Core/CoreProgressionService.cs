using System.Linq;
using Global.Settings.Core;

namespace Core
{
    public class CoreProgressionService
    {
        private readonly CoreSettings _coreSettings;
        private CoreSpeedProgression[] _speedProgression;
        
        public CoreProgressionService(CoreSettings coreSettings)
        {
            _coreSettings = coreSettings;
            _speedProgression = _coreSettings.fallSpeedProgression.OrderByDescending(p => p.scores).ToArray();
        }

        public float GetSpeed(long scores)
        {
            foreach (var progression in _speedProgression)
            {
                if (progression.scores > scores)
                    continue;

                return progression.speed;
            }
            return _speedProgression.Last().speed;
        }

        public int GetLevel(long scores)
        {
            for (var i = 0; i < _speedProgression.Length; ++i)
            {
                var progression = _speedProgression[i];
                if (progression.scores > scores)
                    continue;
                return _speedProgression.Length - i;
            }
            return 1;
        }
    }
}