using System;
using BovineLabs.Core.Authoring.ObjectManagement;
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
                var entity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<ActionCreateOnActivate>(entity);

                foreach (var spawn in authoring.Spawns)
                {
                    if (spawn.Definition == null)
                        throw new InvalidOperationException(
                            $"[{nameof(ActionCreateOnActivateAuthoring)}] {authoring.name}: ObjectDefinition is null.");

                    DependsOn(spawn.Definition);
                    buffer.Add(new ActionCreateOnActivate { Id = spawn.Definition, Target = spawn.Target });
                }
            }
        }
    }
}