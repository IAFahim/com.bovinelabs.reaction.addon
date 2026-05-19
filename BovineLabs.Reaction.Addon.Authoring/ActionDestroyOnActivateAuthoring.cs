// BovineLabs.Reaction.Addon.Authoring/ActionDestroyOnActivateAuthoring.cs
namespace BovineLabs.Reaction.Addon.Authoring
{
    using BovineLabs.Reaction.Addon.Data;
    using BovineLabs.Reaction.Authoring;
    using BovineLabs.Reaction.Authoring.Core;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Destroys the target entity when the action activates.
    /// </summary>
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
                var entity = this.GetEntity(TransformUsageFlags.None);
                var buffer = this.AddBuffer<ActionDestroyOnActivate>(entity);
                buffer.Add(new ActionDestroyOnActivate { Target = authoring.Target });
            }
        }
    }
}