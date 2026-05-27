using BovineLabs.Core.ObjectManagement;
using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    [InternalBufferCapacity(0)]
    public struct ActionCreateOnActivate : IBufferElementData
    {
        public ObjectId Id;

        public Target Target;
    }
}