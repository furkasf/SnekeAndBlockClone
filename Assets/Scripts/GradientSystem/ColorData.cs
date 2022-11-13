using System;
using System.Collections.Generic;
using UnityEngine;

namespace GradientSystem
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData")]
    
    public class ColorData : ScriptableObject
    {
        //public List<List<Color>> Colors;
        public List<ColorValueHolder> Colors;
    }
    [Serializable]
    public class ColorValueHolder
    {
        public List<Color> value;
    }
}