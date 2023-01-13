using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public struct Position
    {
        private readonly int _q;
        private readonly int _r;


        public int Q => _q;
        public int R => _r;

        public Position(int q, int r)
        {
            _q = q;
            _r = r;
        }
        public override string ToString()
        {
            return $"Position(Q: {_q}, R: {_r}";
        }
    }
