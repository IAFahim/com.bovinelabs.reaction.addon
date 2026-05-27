using BovineLabs.Core.ObjectManagement;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    /// <summary>
    ///     Spawns an object from the object registry when the action deactivates.
    /// </summary>
    [InternalBufferCapacity(0)]
    public struct ActionCreateOnDeactivate : IBufferElementData
    {
        public ObjectId Id;

        public Target Target;
    }
}