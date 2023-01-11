using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    internal class HexHelper
    {
        public static Vector3 AxialToCube(float q, float r)
        {
            var qCube = q;
            var rCube = r;
            var sCube = -q - r;

            return new Vector3(qCube, rCube, sCube);
        }

        public static Position Rounding(Vector2 axial, Vector3 cube) //rounds off coords
        {
            Vector3 cubeRounded;
            if (axial != null && cube == Vector3.zero)
                cubeRounded = AxialToCube(axial.x, axial.y);
            else
                cubeRounded = cube;

            var q = Mathf.Round(cubeRounded.x);
            var r = Mathf.Round(cubeRounded.y);
            var s = Mathf.Round(cubeRounded.z);


            var qDiff = Mathf.Abs(q - cubeRounded.x);
            var rDiff = Mathf.Abs(r - cubeRounded.y);
            var sDiff = Mathf.Abs(s - cubeRounded.z);

            if (qDiff > rDiff && qDiff > sDiff)
                q = -r - s;
            else if (rDiff > sDiff)
                r = -q - s;
            else
                s = -q - r;


            return new Position((int)q, (int)r, (int)s);
        }

        public static int CubeDistance(Position fromPos, Position toPos)
        {
            return (int)(MathF.Abs(fromPos.Q - toPos.Q) + MathF.Abs(fromPos.R - toPos.R) + MathF.Abs(fromPos.S - toPos.S)) / 2;
        }
    }

