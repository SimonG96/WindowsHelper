using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WindowsHelper.Events;
using WindowsHelper.ViewModels;

namespace WindowsHelper.Inputs
{
    public static class InputHelper
    {
        private static readonly DropdownItemViewModel NO_RESULT_ITEM = new DropdownItemViewModel("No matching Result");

        public static List<DropdownItemViewModel> CheckInput(string input)
        {
            List<DropdownItemViewModel> items = new List<DropdownItemViewModel>();

            foreach (var name in Enum.GetNames(typeof(MenuInputs)))
            {
                if (name.StartsWith(input, StringComparison.InvariantCultureIgnoreCase) ||
                    name.Equals(input, StringComparison.InvariantCultureIgnoreCase))
                {
                    items.Add(new DropdownItemViewModel(name));
                }
            }

            if (!items.Any())
            {
                items.Add(NO_RESULT_ITEM);
            }

            return items;
        }

        public static ICommand GetCommandForInput(MenuInputs input, object sender)
        {
            switch (input)
            {
                case MenuInputs.Settings:
                {
                    return ShowSettingsWindowEvent.GetShowSettingsWindowCommand(sender);
                }
                case MenuInputs.Spotify:
                {
                    return ShowSpotifyWindowEvent.GetShowSpotifyWindowCommand(sender);
                }
                case MenuInputs.Exit:
                {
                    return CloseRequestedEvent.GetCloseRequestedCommand(sender);
                }
                default:
                {
                    throw new InvalidOperationException($"Can't get Command for Input {input} from Sender {sender}");
                }
            }
        }
    }
}