using BovineLabs.Core;
using BovineLabs.Core.LifeCycle;
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
    public partial struct ActionDestroyOnDeactivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
            var commandBufferSystem = SystemAPI.GetSingleton<InstantiateCommandBufferSystem.Singleton>();
            new DestroyJob
            {
                CommandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntity>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        [WithDisabled(typeof(DestroyEntity))] 
        private partial struct DestroyJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in DynamicBuffer<ActionDestroyOnDeactivate> actions, in Targets targets)
            {
                
                for (var i = 0; i < actions.Length; i++)
                    ActionResolver.EnableDestroy(actions[i].Target, entity, targets, ref DestroyLookup);
                
                CommandBuffer.SetComponentEnabled<ActivePrevious>(chunkIndex, entity, false);
            }
        }
    }
}