namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Core.LifeCycle;
    using BovineLabs.Reaction.Addon.Data;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Reaction.Groups;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;

    [UpdateInGroup(typeof(ActiveEnabledSystemGroup))]
    public partial struct ActionDestroyOnActivateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ActivateJob
            {
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntity>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        private partial struct ActivateJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in DynamicBuffer<ActionDestroy> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    var action = actions[i];
                    if (action.Phase != ExecutionPhase.OnActivate) continue;
                    DestroyResolver.EnableDestroy(action.Target, entity, targets, ref DestroyLookup);
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
            new DeactivateJob
            {
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntity>()
            }.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(ActivePrevious))]
        [WithDisabled(typeof(Active))]
        private partial struct DeactivateJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in DynamicBuffer<ActionDestroy> actions, in Targets targets)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    var action = actions[i];
                    if (action.Phase != ExecutionPhase.OnDeactivate) continue;
                    DestroyResolver.EnableDestroy(action.Target, entity, targets, ref DestroyLookup);
                }
            }
        }
    }

    internal static class DestroyResolver
    {
        public static void EnableDestroy(
            Target requested,
            Entity self,
            in Targets targets,
            ref ComponentLookup<DestroyEntity> destroyLookup)
        {
            var target = targets.Get(requested, self);
            if (target == Entity.Null) return;
            if (!destroyLookup.HasComponent(target)) return;
            destroyLookup.SetComponentEnabled(target, true);
        }
    }
}