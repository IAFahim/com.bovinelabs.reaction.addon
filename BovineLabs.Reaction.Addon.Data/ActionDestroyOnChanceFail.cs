using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    [InternalBufferCapacity(0)]
    public struct ActionDestroyOnChanceFail : IBufferElementData
    {
        public Target Target;
    }
}