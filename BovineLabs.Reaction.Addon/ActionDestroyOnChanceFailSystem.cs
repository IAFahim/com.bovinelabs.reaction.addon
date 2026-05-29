using BovineLabs.Core.LifeCycle;
using BovineLabs.Reaction.Addon.Data;
using BovineLabs.Reaction.Data.Active;
using BovineLabs.Reaction.Data.Conditions;
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
    public partial struct ActionDestroyOnChanceFailSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            new DestroyJob
            {
                Ecb = ecb.AsParallelWriter(),
                
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ConditionChance))]
        [WithDisabled(typeof(ConditionAllActive))]
        [WithDisabled(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        [WithDisabled(typeof(DestroyEntity))]
        private partial struct DestroyJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in ConditionActive conditionActive,
                in ActionDestroyOnChanceFail actions, in Targets targets)
            {
                if (!conditionActive.Value.AllTrue) return;

                ActionResolver.EnableDestroy(chunkIndex, actions.Target, entity, targets, ref Ecb);
            }
        }
    }
}