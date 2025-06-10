using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {
        public static Vector3 ChangeZ(this Vector3 vector3, float z)
        {
            return new Vector3(vector3.x, vector3.y, z);
        }
    }
}