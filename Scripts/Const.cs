using UnityEngine;

namespace Unity1week202403
{
    public static class Const
    {
        public const int LayerIdMonsterCollider = 6;
        public static LayerMask LayerMaskMonsterCollider = 1 << LayerIdMonsterCollider;

        public static class Scene
        {
            public const string Title = "Title";
            public const string InGame = "InGame";
            public const string Ending = "Ending";
        }
    }
}