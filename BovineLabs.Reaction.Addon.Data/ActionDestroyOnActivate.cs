// BovineLabs.Reaction.Addon.Data/ActionDestroyOnActivate.cs
namespace BovineLabs.Reaction.Addon.Data
{
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;

    /// <summary>
    /// Enables the DestroyEntity component on the target when the action activates.
    /// </summary>
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnActivate : IBufferElementData
    {
        public Target Target;
    }
}