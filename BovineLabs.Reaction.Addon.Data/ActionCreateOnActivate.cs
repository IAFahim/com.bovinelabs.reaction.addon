// BovineLabs.Reaction.Data/Actions/ActionCreateOnActivate.cs

using BovineLabs.Core.ObjectManagement;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Data.Actions
{
    /// <summary>
    ///     Spawns an object from the object registry when the action activates.
    /// </summary>
    [InternalBufferCapacity(1)]
    public struct ActionCreateOnActivate : IBufferElementData
    {
        public ObjectId Id;

        public Target Target;
    }
}