using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Visual.Effects
{
    public enum EffectType
    {
        Click,
        Found,
        All
    }

    // Responsible for managing and playing visual effects
    public class EffectController : MonoBehaviour
    {
        [Serializable]
        public struct EffectGroup
        {
            public EffectType Type;
            public List<BaseEffect> Effects;
        }

        [SerializeField] private List<EffectGroup> _effectGroups;

        public void PlayEffect(EffectType type)
        {
            if (_effectGroups == null || _effectGroups.Count == 0)
            {
                Debug.LogWarning($"[EffectController] No effect groups configured on {name}");
                return;
            }

            var group = _effectGroups.FirstOrDefault(g => g.Type == type);
            
            if (group.Effects == null || group.Effects.Count == 0)
            {
                Debug.LogWarning($"[EffectController] No effects found for type {type} on {name}");
                return;
            }

            foreach (var effect in group.Effects)
            {
                if (effect != null)
                {
                    Debug.Log($"[EffectController] Playing {effect.GetType().Name} for type {type} on {name}");
                    effect.Play();
                }
                else
                {
                    Debug.LogWarning($"[EffectController] Null effect in group {type} on {name}");
                }
            }
        }

        public void StopAllEffects()
        {
            foreach (var group in _effectGroups)
            {
                if (group.Effects == null) continue;
                
                foreach (var effect in group.Effects)
                    if (effect != null) effect.Stop();
            }
        }
        
        private void OnValidate()
        {
            if (_effectGroups == null) return;
            foreach(var group in _effectGroups)
            {
                if(group.Effects != null && group.Effects.Contains(null))
                    Debug.LogWarning($"EffectController on {name} has empty effect slots!");
            }
        }
    }
}
