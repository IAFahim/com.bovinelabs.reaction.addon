// BovineLabs.Reaction.Data/Actions/ActionCreateOnDeactivate.cs
namespace BovineLabs.Reaction.Data.Actions
{
    using BovineLabs.Core.ObjectManagement;
    using BovineLabs.Reaction.Data.Core;
    using Unity.Entities;

    [InternalBufferCapacity(1)]
    public struct ActionCreateOnDeactivate : IBufferElementData
    {
        public ObjectId Id;
        public Target Target;
    }
}