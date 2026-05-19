// BovineLabs.Reaction.Addon/ActionDestroyOnActivateSystem.cs

using BovineLabs.Core.LifeCycle;
using BovineLabs.Reaction.Addon.Data;
using BovineLabs.Reaction.Data.Active;
using BovineLabs.Reaction.Data.Core;
using BovineLabs.Reaction.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace BovineLabs.Reaction.Actions
{
    /// <summary>
    ///     Processes destroy actions when an action activates.
    /// </summary>
    [UpdateInGroup(typeof(ActiveEnabledSystemGroup))]
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

            private void Execute(Entity entity, in DynamicBuffer<ActionDestroyOnActivate> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                    ActionResolver.EnableDestroy(actions[i].Target, entity, targets, ref DestroyLookup);
            }
        }
    }
}