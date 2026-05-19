// BovineLabs.Reaction.Addon.Authoring/ActionDestroyOnDeactivateAuthoring.cs

using BovineLabs.Reaction.Addon.Data;
using BovineLabs.Reaction.Authoring;
using BovineLabs.Reaction.Authoring.Core;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;
using UnityEngine;

namespace BovineLabs.Reaction.Addon.Authoring
{
    /// <summary>
    ///     Destroys the target entity when the action deactivates.
    /// </summary>
    [ReactionAuthoring]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReactionAuthoring))]
    public class ActionDestroyOnDeactivateAuthoring : MonoBehaviour
    {
        [Tooltip("Set to Self to destroy the Reaction entity itself.")]
        public Target Target = Target.Self;

        private class Baker : Baker<ActionDestroyOnDeactivateAuthoring>
        {
            public override void Bake(ActionDestroyOnDeactivateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<ActionDestroyOnDeactivate>(entity);
                buffer.Add(new ActionDestroyOnDeactivate { Target = authoring.Target });
            }
        }
    }
}