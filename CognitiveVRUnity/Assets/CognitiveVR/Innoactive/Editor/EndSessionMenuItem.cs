#if CVR_INNOACTIVE
using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    /// <inheritdoc />
    public class EndSessionMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "C3D/End Session";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new EndSessionBehavior("End Session",BehaviorExecutionStages.Activation);
        }
    }
}
#endif