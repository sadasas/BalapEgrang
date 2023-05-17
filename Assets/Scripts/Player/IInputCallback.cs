using System;
using UnityEngine;

namespace Player
{
    public interface IInputCallback
    {
        public event Action OnTap;
        public event Action OnHold;
        public event Action OnRelease;
        public event Action<Vector2> OnSwipe;
    }
}

