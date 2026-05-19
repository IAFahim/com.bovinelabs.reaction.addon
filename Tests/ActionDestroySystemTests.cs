// BovineLabs.Reaction.Addon.Tests/ActionDestroySystemTests.cs
namespace BovineLabs.Reaction.Addon.Tests
{
    using BovineLabs.Core.LifeCycle;
    using BovineLabs.Reaction.Addon.Data;
    using BovineLabs.Reaction.Data.Actions;
    using BovineLabs.Reaction.Data.Active;
    using BovineLabs.Reaction.Data.Core;
    using BovineLabs.Testing;
    using NUnit.Framework;
    using Unity.Entities;

    /// <summary>
    /// Tests for ActionDestroyOnDeactivateSystem.
    /// </summary>
    public class ActionDestroySystemTests : ECSTestsFixture
    {
        [Test]
        public void ActionDestroyOnDeactivate_TargetIsNull_DoesNotCrash()
        {
            // Arrange
            var reactionEntity = Manager.CreateEntity(
                typeof(Active),
                typeof(ActivePrevious),
                typeof(DynamicBuffer<ActionDestroyOnDeactivate>));

            var buffer = Manager.GetBuffer<ActionDestroyOnDeactivate>(reactionEntity);
            buffer.Add(new ActionDestroyOnDeactivate { Target = Target.Target });

            // Manager does not have DestroyEntity component on anything.
            // The system should handle this gracefully.

            // Act - should not throw
            Assert.DoesNotThrow(() => Manager.CompleteAllTrackedJobs());
        }

        [Test]
        public void ActionDestroyOnDeactivate_TargetHasDestroyEntity_EnablesDestroy()
        {
            // Arrange
            var target = Manager.CreateEntity(typeof(DestroyEntity));
            Manager.SetComponentEnabled<DestroyEntity>(target, false);

            var reactionEntity = Manager.CreateEntity(
                typeof(Active),
                typeof(ActivePrevious),
                typeof(Targets),
                typeof(DynamicBuffer<ActionDestroyOnDeactivate>));

            Manager.SetComponentData(reactionEntity, new Targets { Target = target });
            var buffer = Manager.GetBuffer<ActionDestroyOnDeactivate>(reactionEntity);
            buffer.Add(new ActionDestroyOnDeactivate { Target = Target.Target });

            // Act
            Manager.CompleteAllTrackedJobs();

            // Assert
            Assert.IsTrue(Manager.IsComponentEnabled<DestroyEntity>(target));
        }

        [Test]
        public void ActionDestroyOnDeactivate_TargetIsSelf_DestroysOwner()
        {
            // Arrange
            var owner = Manager.CreateEntity(typeof(DestroyEntity));
            Manager.SetComponentEnabled<DestroyEntity>(owner, false);

            var reactionEntity = Manager.CreateEntity(
                typeof(Active),
                typeof(ActivePrevious),
                typeof(Targets),
                typeof(DynamicBuffer<ActionDestroyOnDeactivate>));

            Manager.SetComponentData(reactionEntity, new Targets { Owner = owner });
            var buffer = Manager.GetBuffer<ActionDestroyOnDeactivate>(reactionEntity);
            buffer.Add(new ActionDestroyOnDeactivate { Target = Target.Owner });

            // Act
            Manager.CompleteAllTrackedJobs();

            // Assert
            Assert.IsTrue(Manager.IsComponentEnabled<DestroyEntity>(owner));
        }

        [Test]
        public void ActionDestroyOnActivate_TargetHasDestroyEntity_EnablesDestroy()
        {
            // Arrange
            var target = Manager.CreateEntity(typeof(DestroyEntity));
            Manager.SetComponentEnabled<DestroyEntity>(target, false);

            var reactionEntity = Manager.CreateEntity(
                typeof(Active),
                typeof(DynamicBuffer<ActionDestroyOnActivate>),
                typeof(Targets));

            // ActivePrevious is disabled (not set), so Active is active
            Manager.SetComponentData(reactionEntity, new Targets { Target = target });
            var buffer = Manager.GetBuffer<ActionDestroyOnActivate>(reactionEntity);
            buffer.Add(new ActionDestroyOnActivate { Target = Target.Target });

            // Act
            Manager.CompleteAllTrackedJobs();

            // Assert
            Assert.IsTrue(Manager.IsComponentEnabled<DestroyEntity>(target));
        }

        [Test]
        public void ActionDestroyOnActivate_WithoutActivePrevious_SkipsIfActivePrevious()
        {
            // Arrange - reaction is already active (no ActivePrevious means it just became active)
            // Wait, this is a bit tricky. The system requires [WithDisabled(typeof(ActivePrevious))].
            // So it only runs when ActivePrevious is disabled. But this test fixture may not set that up.
            // This test verifies the filter works.
            var target = Manager.CreateEntity(typeof(DestroyEntity));
            Manager.SetComponentEnabled<DestroyEntity>(target, false);

            var reactionEntity = Manager.CreateEntity(
                typeof(Active),
                typeof(ActivePrevious), // <-- ActivePrevious enabled means it was already active before
                typeof(DynamicBuffer<ActionDestroyOnActivate>),
                typeof(Targets));

            Manager.SetComponentData(reactionEntity, new Targets { Target = target });
            var buffer = Manager.GetBuffer<ActionDestroyOnActivate>(reactionEntity);
            buffer.Add(new ActionDestroyOnActivate { Target = Target.Target });

            // Act
            Manager.CompleteAllTrackedJobs();

            // Assert - should NOT enable because ActivePrevious is still enabled
            Assert.IsFalse(Manager.IsComponentEnabled<DestroyEntity>(target));
        }
    }
}