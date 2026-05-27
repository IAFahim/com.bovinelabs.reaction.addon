using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    public struct ActionDestroyOnChanceFail : IComponentData
    {
        public Target Target;
    }
}