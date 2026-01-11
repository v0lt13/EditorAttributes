using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(WrapAttribute))]
    public class WrapDrawer : MinMaxAxisDrawer
    {
        protected override void MinMaxAxis((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, ref Vector4 vector)
        {
            float x = Wrap(vector.x, minMaxX.Item1, minMaxX.Item2);
            float y = Wrap(vector.y, minMaxY.Item1, minMaxY.Item2);
            float z = Wrap(vector.z, minMaxZ.Item1, minMaxZ.Item2);
            float w = Wrap(vector.w, minMaxW.Item1, minMaxW.Item2);

            vector = new Vector4(x, y, z, w);
        }

        private float Wrap(float value, float min, float max)
        {
            if (value > max)
                value = min;

            if (value < min)
                value = max;

            return value;
        }
    }
}
