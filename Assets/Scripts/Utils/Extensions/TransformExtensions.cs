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

        public static bool IsInsideOval(this RectTransform target, RectTransform oval, Canvas canvas)
        {
            float a = oval.rect.size.x * canvas.scaleFactor / 2f;
            float b = oval.rect.size.y * canvas.scaleFactor / 2f;

            float dx = target.position.x - oval.position.x;
            float dy = target.position.y - oval.position.y;

            float value = (dx * dx) / (a * a) + (dy * dy) / (b * b);
            return value <= 1f;
        }
    }
}