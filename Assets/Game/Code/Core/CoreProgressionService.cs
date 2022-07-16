using System.Linq;
using Global.Settings.Core;

namespace Core
{
    public class CoreProgressionService
    {
        private readonly CoreSpeedProgression[] _speedProgression;
        
        public CoreProgressionService(CoreSettings coreSettings)
        {
            _speedProgression = coreSettings.fallSpeedProgression.OrderByDescending(p => p.scores).ToArray();
        }

        public CoreSpeedProgression GetDifficult(long scores)
        {
            foreach (var progression in _speedProgression)
            {
                if (progression.scores > scores)
                    continue;

                return progression;
            }
            return _speedProgression.Last();
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