// BovineLabs.Reaction.Authoring/Actions/ActionCreateOnDeactivateAuthoring.cs

using System;
using BovineLabs.Core.Authoring.ObjectManagement;
using BovineLabs.Reaction.Authoring.Core;
using BovineLabs.Reaction.Data.Actions;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;
using UnityEngine;

namespace BovineLabs.Reaction.Authoring.Actions
{
    /// <summary>
    ///     Spawns an object when the action deactivates.
    /// </summary>
    [ReactionAuthoring]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReactionAuthoring))]
    public class ActionCreateOnDeactivateAuthoring : MonoBehaviour
    {
        [Tooltip("Objects to spawn when this action deactivates.")]
        public Spawn[] Spawns = Array.Empty<Spawn>();

        [Serializable]
        public class Spawn
        {
            public ObjectDefinition Definition;
            public Target Target = Target.Target;
        }

        private class Baker : Baker<ActionCreateOnDeactivateAuthoring>
        {
            public override void Bake(ActionCreateOnDeactivateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<ActionCreateOnDeactivate>(entity);

                foreach (var spawn in authoring.Spawns)
                {
                    if (spawn.Definition == null)
                    {
                        UnityEngine.Debug.LogError($"[{nameof(ActionCreateOnDeactivateAuthoring)}] {authoring.name}: ObjectDefinition is null.", authoring);
                        continue;
                    }

                    DependsOn(spawn.Definition);
                    buffer.Add(new ActionCreateOnDeactivate { Id = spawn.Definition, Target = spawn.Target });
                }
            }
        }
    }
}