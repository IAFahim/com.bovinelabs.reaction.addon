// BovineLabs.Reaction.Addon.Data/ActionDestroyOnDeactivate.cs

using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    /// <summary>
    ///     Enables the DestroyEntity component on the target when the action deactivates.
    /// </summary>
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnDeactivate : IBufferElementData
    {
        public Target Target;
    }
}