#if CVR_INNOACTIVE
using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    /// <inheritdoc />
    public class CustomEventMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "C3D/Custom Event";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new PlayCustomEventBehavior("Record Event", "New Event","",BehaviorExecutionStages.Deactivation);
        }
    }
}
#endif