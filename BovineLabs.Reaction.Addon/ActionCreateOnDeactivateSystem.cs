using BovineLabs.Core;
using BovineLabs.Core.ObjectManagement;
using BovineLabs.Reaction.Addon.Data;
using BovineLabs.Reaction.Data.Active;
using BovineLabs.Reaction.Data.Core;
using BovineLabs.Reaction.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon
{
    [UpdateInGroup(typeof(ActiveDisabledSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ClientSimulation |
                       WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ActionCreateOnDeactivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBufferSystem = SystemAPI.GetSingleton<InstantiateCommandBufferSystem.Singleton>();

            new DeactivateJob
            {
                CommandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ObjectDefinitions = SystemAPI.GetSingleton<ObjectDefinitionRegistry>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        private partial struct DeactivateJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ObjectDefinitionRegistry ObjectDefinitions;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity,
                in DynamicBuffer<ActionCreateOnDeactivate> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    var create = actions[i];
                    var prefab = ObjectDefinitions[create.Id];
                    if (prefab == Entity.Null) continue;

                    var target = targets.Get(create.Target, entity);
                    var instTargets = targets.Copy(entity, target);

                    var createdEntity = CommandBuffer.Instantiate(chunkIndex, prefab);
                    CommandBuffer.SetComponent(chunkIndex, createdEntity, instTargets);
                }

                CommandBuffer.SetComponentEnabled<ActivePrevious>(chunkIndex, entity, false);
            }
        }
    }
}