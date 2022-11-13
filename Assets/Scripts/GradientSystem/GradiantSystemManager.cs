using System.Collections.Generic;
using UnityEngine;

namespace GradientSystem
{
    /// <summary>
    /// all copy right belong to furkan tural , you free feel to use or modify but if class help you 
    /// in a way feel free to send me a mail, all possitive contributions are open
    /// </summary>
    public static class GradiantSystemManager
    {
        private static List<Gradient> _gradients = new List<Gradient>();
        private static List<GradientColorKey[]> _colorKey = new List<GradientColorKey[]>();
        private static List<GradientAlphaKey[]> _alphaKey = new List<GradientAlphaKey[]>();
        private static ColorData _colorData;

        static GradiantSystemManager()
        {
            GetColorData();
            GenarateColorKeysAndAlphas(_colorData.Colors, false);
            GenarateGradiants();
        }

        private static void GetColorData() => _colorData = Resources.Load<ColorData>("Data/ColorData");

        private static void GenarateColorKeysAndAlphas(List<ColorValueHolder> colors , bool isTransprant = true)
        {
            float persentage = 0f;
            float ratio;
            for (int i = 0; i < colors.Count; i++)
            {
                int colorNumber = colors[i].value.Count;
                GradientColorKey[] colorKey = new GradientColorKey[colorNumber];
                GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

                ratio = (colors[i].value.Count % 2 == 0) ? 1f : 0.5f;

                alphaKey[0].alpha = 1.0f;
                alphaKey[0].time = 0.0f;
                alphaKey[1].alpha = (isTransprant) ? 0.0f : 1.0f;
                alphaKey[1].time = 1.0f;
                
                for (int j = 0; j < colorNumber; j++)
                {
                    colorKey[j].color = colors[i].value[j];
                    colorKey[j].time = 0.0f + persentage;
                    persentage += ratio;
                }

                persentage = 0f;

                _colorKey.Add(colorKey);
                _alphaKey.Add(alphaKey);
            }

        }



        private static void GenarateGradiants()
        {

            for (int i = 0; i < _colorKey.Count; i++)
            {
                Gradient gradient = new Gradient();              
                gradient.SetKeys(_colorKey[i], _alphaKey[i]);
                _gradients.Add(gradient);
            }
        }

        public static Color GetGradiantColorByPersentage(float min, float max, GradianType type)
        {
            return _gradients[(int)type].Evaluate(min / max);
        }
    }



    public enum GradianType
    {
        Sneak,
        Block
    }
}
