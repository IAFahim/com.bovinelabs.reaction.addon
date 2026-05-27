using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnActivate : IComponentData
    {
        public Target Target;
    }
}