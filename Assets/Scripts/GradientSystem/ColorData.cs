using System;
using System.Collections.Generic;
using UnityEngine;

namespace GradientSystem
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData")]
    
    public class ColorData : ScriptableObject
    {
        public List<ColorValueHolder> Colors;
        public List<Gradient> gradientReflaction;//to see gradiant generation in editor
    }
    [Serializable]
    public class ColorValueHolder
    {
        public List<Color> value;
    }
}