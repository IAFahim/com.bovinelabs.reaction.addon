using BovineLabs.Reaction.Addon.Data;
using Unity.Collections;

namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Core.LifeCycle;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Reaction.Groups;
    using Unity.Burst;
    using Unity.Entities;

    [UpdateInGroup(typeof(ActiveEnabledSystemGroup))]
    public partial struct ActionDestroyOnActivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new ActivateJob
            {
                CommandBuffer = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                TargetsCustoms = SystemAPI.GetComponentLookup<TargetsCustom>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        private partial struct ActivateJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ComponentLookup<TargetsCustom> TargetsCustoms;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in DynamicBuffer<ActionDestroy> actions, in Targets targets)
            {
                // Fix: Using a standard for-loop avoids Burst Enumerator hashing errors
                for (int i = 0; i < actions.Length; i++)
                {
                    var action = actions[i];
                    if (action.Phase == ExecutionPhase.OnActivate)
                    {
                        var target = targets.Get(action.Target, entity, TargetsCustoms);
                        if (target != Entity.Null)
                        {
                            CommandBuffer.AddComponent<DestroyEntity>(chunkIndex, target);
                            CommandBuffer.SetComponentEnabled<DestroyEntity>(chunkIndex, target, true);
                        }
                    }
                }
            }
        }
    }

    [UpdateInGroup(typeof(ActiveDisabledSystemGroup))]
    public partial struct ActionDestroyOnDeactivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new DeactivateJob
            {
                CommandBuffer = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                TargetsCustoms   = SystemAPI.GetComponentLookup<TargetsCustom>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        private partial struct DeactivateJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ComponentLookup<TargetsCustom> TargetsCustoms;
            
            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in DynamicBuffer<ActionDestroy> actions, in Targets targets)
            {
                // Fix: Using a standard for-loop avoids Burst Enumerator hashing errors
                for (int i = 0; i < actions.Length; i++)
                {
                    var action = actions[i];
                    if (action.Phase == ExecutionPhase.OnDeactivate)
                    {
                        var target = targets.Get(action.Target, entity, TargetsCustoms);
                        if (target != Entity.Null)
                        {
                            CommandBuffer.AddComponent<DestroyEntity>(chunkIndex, target);
                            CommandBuffer.SetComponentEnabled<DestroyEntity>(chunkIndex, target, true);
                        }
                    }
                }
            }
        }
    }
}