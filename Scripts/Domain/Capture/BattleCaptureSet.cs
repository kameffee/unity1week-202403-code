using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain.Capture
{
    public readonly struct BattleCaptureSet
    {
        public StageId StageId { get; }
        public Texture2D Texture { get; }
        
        public BattleCaptureSet(StageId stageId, Texture2D texture)
        {
            StageId = stageId;
            Texture = texture;
        }
    }
}