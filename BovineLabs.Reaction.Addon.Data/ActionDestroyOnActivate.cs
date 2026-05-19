// BovineLabs.Reaction.Addon.Data/ActionDestroyOnActivate.cs

using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    /// <summary>
    ///     Enables the DestroyEntity component on the target when the action activates.
    /// </summary>
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnActivate : IBufferElementData
    {
        public Target Target;
    }
}