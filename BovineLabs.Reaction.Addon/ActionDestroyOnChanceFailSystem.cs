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
        public void OnUpdate(ref SystemState state)
        {
            new DestroyJob
            {
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntity>()
            }.Schedule();
        }

        [BurstCompile]
        [WithAll(typeof(ConditionChance))]
        [WithDisabled(typeof(ConditionAllActive))]
        [WithDisabled(typeof(Active))]
        [WithDisabled(typeof(ActivePrevious))]
        [WithDisabled(typeof(DestroyEntity))]
        private partial struct DestroyJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public ComponentLookup<DestroyEntity> DestroyLookup;

            private void Execute(Entity entity, in ConditionActive conditionActive,
                in DynamicBuffer<ActionDestroyOnChanceFail> actions, in Targets targets)
            {
                if (!conditionActive.Value.AllTrue)
                    return;

                for (var i = 0; i < actions.Length; i++)
                    ActionResolver.EnableDestroy(actions[i].Target, entity, targets, ref DestroyLookup);
            }
        }
    }
}