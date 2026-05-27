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
    [UpdateInGroup(typeof(ActiveEnabledSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ClientSimulation |
                       WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ActionDestroyOnActivateSystem : ISystem
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
        [WithAll(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        private partial struct DestroyJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in ActionDestroyOnActivate actions, in Targets targets)
            {
                    ActionResolver.EnableDestroy(actions.Target, entity, targets, ref DestroyLookup);
            }
        }
    }
}