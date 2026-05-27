using BovineLabs.Reaction.Data.Core;
using Unity.Entities;

namespace BovineLabs.Reaction.Addon.Data
{
    public struct ActionDestroyOnDeactivate : IComponentData
    {
        public Target Target;
    }
}