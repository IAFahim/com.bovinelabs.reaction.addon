
using BovineLabs.Reaction.Data.Actions;
using BovineLabs.Reaction.Data.Core;
using UnityEngine;

namespace BovineLabs.Reaction.Addon.Data
{
using Unity.Entities;
    public enum ExecutionPhase : byte
    {
        OnActivate,     // Triggers instantly (e.g. Cooldown start)
        OnDeactivate    // Triggers when going down (e.g. Duration ends)
    }

    [InternalBufferCapacity(0)]
    public struct ActionDestroy : IBufferElementData, IActionWithTarget
    {
        public Target Target;
        public ExecutionPhase Phase;

        Target IActionWithTarget.Target => this.Target;
    }
}