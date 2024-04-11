using Unity1week202403.Extensions;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class UnscaledShaderTime : Presenter, ITickable
    {
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

        public void Tick()
        {
            Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
        }
    }
}