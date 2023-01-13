using System;
using UnityEngine;

namespace Core
{
    public static class MathUtils
    {
        public static float Area(int x1, int y1, int x2,
            int y2, int x3, int y3)
        {
            return Math.Abs((x1 * (y2 - y3) +
                             x2 * (y3 - y1) +
                             x3 * (y1 - y2)) / 2.0f);
        }

        public static float Area(Vector2 a, Vector2 b, Vector2 c)
        {
            //[ x1(y2 – y3) + x2(y3 – y1) + x3(y1-y2)]/2 
            return Math.Abs(
                a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)
            ) / 2;
        }
    }
}