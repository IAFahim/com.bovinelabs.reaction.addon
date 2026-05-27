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
            new DestroyJob
            {
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntity>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        [WithDisabled(typeof(DestroyEntity))]
        private partial struct DestroyJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in ActionDestroyOnDeactivate actions, in Targets targets)
            {
                ActionResolver.EnableDestroy(actions.Target, entity, targets, ref DestroyLookup);
            }
        }
    }
}