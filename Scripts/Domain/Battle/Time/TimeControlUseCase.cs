using R3;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public enum TimeScaleType
    {
        Slow = 0,
        Normal = 1,
        Fast = 2,
    }

    public class TimeControlUseCase
    {
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;
        public ReadOnlyReactiveProperty<TimeScaleType> TimeScale => _timeScale;

        private readonly ReactiveProperty<bool> _isPlaying = new(true);
        private readonly ReactiveProperty<TimeScaleType> _timeScale = new(TimeScaleType.Normal);

        private const float StopTimeScale = 0.0f;

        public void Play()
        {
            _isPlaying.Value = true;
            Time.timeScale = ActualTimeScale(_isPlaying.Value, _timeScale.Value);
        }

        public void Stop()
        {
            _isPlaying.Value = false;
            Time.timeScale = StopTimeScale;
        }

        public void SwitchPlayAndStop()
        {
            if (_isPlaying.Value)
                Stop();
            else
                Play();
        }

        public void SetTimeScale(TimeScaleType timeScaleType)
        {
            _timeScale.Value = timeScaleType;
            Time.timeScale = ActualTimeScale(_isPlaying.Value, timeScaleType);
        }

        public void SetDefaultTimeScale()
        {
            _timeScale.Value = TimeScaleType.Normal;
            Time.timeScale = ActualTimeScale(_isPlaying.Value, TimeScaleType.Normal);
        }

        private static float ActualTimeScale(bool isPlaying, TimeScaleType timeScaleType)
        {
            return isPlaying ? ToTimeScaleValue(timeScaleType) : StopTimeScale;
        }

        private static float ToTimeScaleValue(TimeScaleType timeScaleType) => timeScaleType switch
        {
            TimeScaleType.Slow => 0.5f,
            TimeScaleType.Normal => 1.0f,
            TimeScaleType.Fast => 2.0f,
            _ => Time.timeScale
        };
    }
}