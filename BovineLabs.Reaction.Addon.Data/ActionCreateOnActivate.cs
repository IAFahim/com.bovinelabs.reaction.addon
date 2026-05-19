// BovineLabs.Reaction.Data/Actions/ActionCreateOnActivate.cs
namespace BovineLabs.Reaction.Data.Actions
{
    using BovineLabs.Core.ObjectManagement;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;

    /// <summary>
    /// Spawns an object from the object registry when the action activates.
    /// </summary>
    [InternalBufferCapacity(1)]
    public struct ActionCreateOnActivate : IBufferElementData
    {
        public ObjectId Id;

        public Target Target;
    }
}