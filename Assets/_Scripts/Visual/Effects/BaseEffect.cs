using UnityEngine;

namespace Visual.Effects
{
    public abstract class BaseEffect : MonoBehaviour
    {
        public abstract void Play();
        public abstract void Stop();
    }
}
