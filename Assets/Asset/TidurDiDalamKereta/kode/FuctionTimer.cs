using System;
using UnityEngine;

public class FunctionTimer : MonoBehaviour
{
    private static GameObject _init;

    public static void Create(Action action, float timer)
    {
        if (_init == null)
        {
            _init = new GameObject("FunctionTimer_Init");
        }

        FunctionTimerObject fto = _init.AddComponent<FunctionTimerObject>();
        fto.Setup(action, timer);
    }

    public class FunctionTimerObject : MonoBehaviour
    {
        private Action action;
        private float timer;

        public void Setup(Action action, float timer)
        {
            this.action = action;
            this.timer = timer;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                action();
                Destroy(this);
            }
        }
    }
}
