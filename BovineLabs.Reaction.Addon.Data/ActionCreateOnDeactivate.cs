// BovineLabs.Reaction.Data/Actions/ActionCreateOnDeactivate.cs

using BovineLabs.Core.ObjectManagement;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Data.Actions
{
    /// <summary>
    ///     Spawns an object from the object registry when the action deactivates.
    /// </summary>
    [InternalBufferCapacity(1)]
    public struct ActionCreateOnDeactivate : IBufferElementData
    {
        public ObjectId Id;

        public Target Target;
    }
}