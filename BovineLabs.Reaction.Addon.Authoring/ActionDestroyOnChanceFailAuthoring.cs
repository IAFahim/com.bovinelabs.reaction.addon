using BovineLabs.Reaction.Addon.Data;
using BovineLabs.Reaction.Authoring;
using BovineLabs.Reaction.Authoring.Core;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;
using UnityEngine;

namespace BovineLabs.Reaction.Addon.Authoring
{
    [ReactionAuthoring]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReactionAuthoring))]
    public class ActionDestroyOnChanceFailAuthoring : MonoBehaviour
    {
        [Tooltip("Set to Self to destroy the Reaction entity itself.")]
        public Target Target = Target.Self;

        private class Baker : Baker<ActionDestroyOnChanceFailAuthoring>
        {
            public override void Bake(ActionDestroyOnChanceFailAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<ActionDestroyOnChanceFail>(entity, new ActionDestroyOnChanceFail()
                {
                    Target = authoring.Target
                });
            }
        }
    }
}
