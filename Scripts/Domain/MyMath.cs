using UnityEngine;

namespace Unity1week202403.Domain
{
    public static class MyMath
    {
        public static float EaseOutCubic(float x){
            return 1 - Mathf.Pow(1 - x, 3);
        }

        public static float EaseOutExpo(float x)
        {
            return x >= 1f ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }

        public static float EaseInBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;

            return c3 * x * x * x - c1 * x * x;
        }

        public static float EaseOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }

    }
}