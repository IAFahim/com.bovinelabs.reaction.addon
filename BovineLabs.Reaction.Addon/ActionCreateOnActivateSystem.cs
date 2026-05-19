// BovineLabs.Reaction.Addon/ActionCreateOnActivateSystem.cs
namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Core;
    using BovineLabs.Core.ObjectManagement;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Reaction.Groups;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;

    [UpdateInGroup(typeof(ActiveEnabledSystemGroup))]
    public partial struct ActionCreateOnActivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBufferSystem = SystemAPI.GetSingleton<InstantiateCommandBufferSystem.Singleton>();

            new ActivateJob
            {
                CommandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ObjectDefinitions = SystemAPI.GetSingleton<ObjectDefinitionRegistry>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        private partial struct ActivateJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ObjectDefinitionRegistry ObjectDefinitions;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in DynamicBuffer<ActionCreateOnActivate> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    var create = actions[i];
                    var prefab = this.ObjectDefinitions[create.Id];
                    if (prefab == Entity.Null) continue;

                    var target = targets.Get(create.Target, entity);
                    var instTargets = targets.Copy(entity, target);

                    var createdEntity = this.CommandBuffer.Instantiate(chunkIndex, prefab);
                    this.CommandBuffer.SetComponent(chunkIndex, createdEntity, instTargets);
                }
            }
        }
    }
}