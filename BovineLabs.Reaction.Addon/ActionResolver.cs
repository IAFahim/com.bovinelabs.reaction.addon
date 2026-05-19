// BovineLabs.Reaction.Addon/ActionResolver.cs

using BovineLabs.Core.LifeCycle;

namespace BovineLabs.Reaction.Actions
{
    using BovineLabs.Reaction.Data.Core;
    using Unity.Collections;
    using Unity.Entities;

    /// <summary>
    /// Helper for resolving action targets and applying effects.
    /// </summary>
    internal static class ActionResolver
    {
        /// <summary>
        /// Enables the DestroyEntity component on the resolved target.
        /// </summary>
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