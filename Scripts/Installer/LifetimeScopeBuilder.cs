using UnityEngine;
using VContainer;

namespace Unity1week202403.Installer
{
    public abstract class LifetimeScopeBuilder : MonoBehaviour
    {
        public abstract void Configure(IContainerBuilder builder);
    }
}