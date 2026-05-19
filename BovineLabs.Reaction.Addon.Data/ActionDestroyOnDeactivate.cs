// BovineLabs.Reaction.Addon.Data/ActionDestroyOnDeactivate.cs
namespace BovineLabs.Reaction.Addon.Data
{
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;

    /// <summary>
    /// Enables the DestroyEntity component on the target when the action deactivates.
    /// </summary>
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnDeactivate : IBufferElementData
    {
        public Target Target;
    }
}