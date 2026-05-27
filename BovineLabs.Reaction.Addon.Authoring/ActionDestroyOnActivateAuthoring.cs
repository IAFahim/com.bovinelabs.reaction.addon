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
    public class ActionDestroyOnActivateAuthoring : MonoBehaviour
    {
        [Tooltip("Set to Self to destroy the Reaction entity itself.")]
        public Target Target = Target.Self;

        private class Baker : Baker<ActionDestroyOnActivateAuthoring>
        {
            public override void Bake(ActionDestroyOnActivateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<ActionDestroyOnActivate>(entity);
                buffer.Add(new ActionDestroyOnActivate { Target = authoring.Target });
            }
        }
    }
}