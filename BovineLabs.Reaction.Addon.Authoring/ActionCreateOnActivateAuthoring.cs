// BovineLabs.Reaction.Authoring/Actions/ActionCreateOnActivateAuthoring.cs
namespace BovineLabs.Reaction.Authoring.Actions
{
    using System;
    using BovineLabs.Core.Authoring.ObjectManagement;
    using BovineLabs.Reaction.Authoring.Core;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Spawns an object when the action activates.
    /// </summary>
    [ReactionAuthoring]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReactionAuthoring))]
    public class ActionCreateOnActivateAuthoring : MonoBehaviour
    {
        [Tooltip("Objects to spawn when this action activates.")]
        public Spawn[] Spawns = Array.Empty<Spawn>();

        [Serializable]
        public class Spawn
        {
            public ObjectDefinition Definition;
            public Target Target = Target.Target;
        }

        private class Baker : Baker<ActionCreateOnActivateAuthoring>
        {
            public override void Bake(ActionCreateOnActivateAuthoring authoring)
            {
                var entity = this.GetEntity(TransformUsageFlags.None);
                var buffer = this.AddBuffer<ActionCreateOnActivate>(entity);

                foreach (var spawn in authoring.Spawns)
                {
                    if (spawn.Definition == null)
                    {
                        throw new InvalidOperationException(
                            $"[{nameof(ActionCreateOnActivateAuthoring)}] {authoring.name}: ObjectDefinition is null.");
                    }

                    this.DependsOn(spawn.Definition);
                    buffer.Add(new ActionCreateOnActivate { Id = spawn.Definition, Target = spawn.Target });
                }
            }
        }
    }
}