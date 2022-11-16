using UnityEngine;

namespace Utalitys
{
    public static class UtilityExtention
    {
        public static void SetAlpha(this Material material, float alpha)
        {
            Color _color = material.color;
            _color.a = alpha;
            material.color = _color;
        }
    }
}