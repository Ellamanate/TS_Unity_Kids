using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public static class TransformExtensions
    {
        public static bool IsFullyInside(this RectTransform inner, RectTransform outer)
        {
            var innerCorners = new Vector3[4];
            var outerCorners = new Vector3[4];

            inner.GetWorldCorners(innerCorners);
            outer.GetWorldCorners(outerCorners);

            var outerMin = outerCorners[0];
            var outerMax = outerCorners[2];

            for (int i = 0; i < 4; i++)
            {
                var corner = innerCorners[i];

                if (corner.x < outerMin.x || corner.x > outerMax.x ||
                    corner.y < outerMin.y || corner.y > outerMax.y)
                {
                    return false;
                }
            }

            return true;
        }
    }
}