using System;
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
    public class ActionDestroyAuthoring : MonoBehaviour
    {
        public Data[] Destroys = Array.Empty<Data>();

        [Serializable]
        public class Data
        {
            [Tooltip("Set to Self to destroy the Reaction entity itself.")]
            public Target Target = Target.Self;
            public ExecutionPhase Phase = ExecutionPhase.OnDeactivate;
        }

        private class Baker : Baker<ActionDestroyAuthoring>
        {
            public override void Bake(ActionDestroyAuthoring authoring)
            {
                var entity = this.GetEntity(TransformUsageFlags.None);
                var buffer = this.AddBuffer<ActionDestroy>(entity);

                foreach (var d in authoring.Destroys)
                {
                    buffer.Add(new ActionDestroy { Target = d.Target, Phase = d.Phase });
                }
            }
        }
    }
}