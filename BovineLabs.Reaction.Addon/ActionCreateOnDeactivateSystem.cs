using BovineLabs.Core;

namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Core.ObjectManagement;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Reaction.Groups;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;

    [UpdateInGroup(typeof(ActiveDisabledSystemGroup))]
    public partial struct ActionCreateOnDeactivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBufferSystem = SystemAPI.GetSingleton<InstantiateCommandBufferSystem.Singleton>();
            
            new DeactivatedJob
            {
                CommandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ObjectDefinitions = SystemAPI.GetSingleton<ObjectDefinitionRegistry>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        private partial struct DeactivatedJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ObjectDefinitionRegistry ObjectDefinitions;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in DynamicBuffer<ActionCreateOnDeactivate> actions, in Targets targets)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    var create = actions[i];
                    var prefab = this.ObjectDefinitions[create.Id];
                    var createdEntity = this.CommandBuffer.Instantiate(chunkIndex, prefab);

                    var target = targets.Get(create.Target, entity);
                    var instTargets = targets.Copy(entity, target);

                    this.CommandBuffer.SetComponent(chunkIndex, createdEntity, instTargets);
                }
            }
        }
    }
}