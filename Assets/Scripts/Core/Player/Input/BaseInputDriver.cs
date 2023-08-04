using UnityEngine;

namespace Input
{
    public abstract class BaseInputDriver : MonoBehaviour
    {
        public Vector2 Movement { get; protected set; }
        public Vector2 LookPosition { get; protected set; }
        public bool Jump { get; protected set; }
        public bool HoldingJump { get; protected set; }
        public bool ReleaseJump { get; protected set; }
        public bool HoldingAbility { get; protected set; }
        public bool ReleaseAbility { get; protected set; }       
        public bool Rerorll { get; protected set; }       
        public abstract void UpdateInput(float timeStep);
    }
}
