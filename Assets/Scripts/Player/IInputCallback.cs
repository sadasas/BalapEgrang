using System;
using UnityEngine;

namespace Player
{
    public interface IInputCallback
    {
        public event Action OnTap;
        public event Action OnHold;
        public event Action<Vector3> OnSwipe;
    }
}

