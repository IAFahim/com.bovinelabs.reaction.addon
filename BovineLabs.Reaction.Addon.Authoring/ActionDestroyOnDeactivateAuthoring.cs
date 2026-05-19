// BovineLabs.Reaction.Addon.Authoring/ActionDestroyOnDeactivateAuthoring.cs
namespace BovineLabs.Reaction.Addon.Authoring
{
    using BovineLabs.Reaction.Addon.Data;
    using BovineLabs.Reaction.Authoring;
    using BovineLabs.Reaction.Authoring.Core;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Destroys the target entity when the action deactivates.
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
                var entity = this.GetEntity(TransformUsageFlags.None);
                var buffer = this.AddBuffer<ActionDestroyOnDeactivate>(entity);
                buffer.Add(new ActionDestroyOnDeactivate { Target = authoring.Target });
            }
        }
    }
}