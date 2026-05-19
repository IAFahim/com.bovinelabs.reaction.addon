// BovineLabs.Reaction.Authoring/Actions/ActionCreateOnDeactivateAuthoring.cs
namespace BovineLabs.Reaction.Authoring.Actions
{
    using System;
    using BovineLabs.Core.Authoring.ObjectManagement;
    using BovineLabs.Reaction.Authoring.Core;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;
    using UnityEngine;

    [ReactionAuthoring]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReactionAuthoring))]
    public class ActionCreateOnDeactivateAuthoring : MonoBehaviour
    {
        public Data[] Create = Array.Empty<Data>();

        [Serializable]
        public class Data
        {
            public ObjectDefinition Definition;
            public Target Target = Target.Target;
        }

        public class Baker : Baker<ActionCreateOnDeactivateAuthoring>
        {
            public override void Bake(ActionCreateOnDeactivateAuthoring authoring)
            {
                var buffer = this.AddBuffer<ActionCreateOnDeactivate>(this.GetEntity(TransformUsageFlags.None));

                foreach (var e in authoring.Create)
                {
                    if (e.Definition == null) continue;
                    this.DependsOn(e.Definition);

                    buffer.Add(new ActionCreateOnDeactivate { Id = e.Definition, Target = e.Target });
                }
            }
        }
    }
}