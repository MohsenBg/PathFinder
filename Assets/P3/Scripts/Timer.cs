using UnityEngine;

public class Timer {
    private float _time = 60;
    private float _HelperTime = 0;

    public float GetTimerTime() {
        return _time;
    }

    public void SetTimerTime(float time) {
        if (time < 0) {
            Debug.LogWarning("Timer time can not be negative!");
            return;
        }
        _time = time;
        _HelperTime = time;
    }

    public void RestartTimer() {
        _time = _HelperTime;
    }

    public Timer(float time) {
        SetTimerTime(time);
    }

    public bool IsTimerZero() {
        if (_time == 0)
            return true;
        return false;
    }

    public void UpdateTimer(float deltaTime) {
        if (_time <= 0) {
            _time = 0;
            return;
        }
        _time -= Time.deltaTime;
    }
}
