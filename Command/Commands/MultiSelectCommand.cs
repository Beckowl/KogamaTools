using KogamaTools.Helpers;
using KogamaTools.Patches;
namespace KogamaTools.Command.Commands
{
    internal class MultiSelectCommand : BaseCommand
    {
        public MultiSelectCommand() : base("/multiselect", "Forces multi selection to not de-select objects unless it is disabled.")
        {
            AddVariant(args => Toggle());
        }

        private void Toggle()
        {
            MultiSelect.ForceSelection = !MultiSelect.ForceSelection;
            NotificationHelper.NotifySuccess($"Multi selection forced {(MultiSelect.ForceSelection ? "on" : "off")}.");
        }
    }
}
