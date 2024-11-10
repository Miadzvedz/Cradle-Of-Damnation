using UnityEngine;

namespace Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool IsMatch(this LayerMask layerMask, int layerIndex) =>
            (layerMask.value & (1 << layerIndex)) > 0;
    }
}
