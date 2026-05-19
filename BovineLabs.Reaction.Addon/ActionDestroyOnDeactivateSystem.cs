// BovineLabs.Reaction.Addon/ActionDestroyOnDeactivateSystem.cs
namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Core.LifeCycle;
    using BovineLabs.Reaction.Addon.Data;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Reaction.Groups;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;

    /// <summary>
    /// Processes destroy actions when an action deactivates.
    /// </summary>
    [UpdateInGroup(typeof(ActiveDisabledSystemGroup))]
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
        private partial struct DestroyJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in DynamicBuffer<ActionDestroyOnDeactivate> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    ActionResolver.EnableDestroy(actions[i].Target, entity, targets, ref DestroyLookup);
                }
            }
        }
    }
}