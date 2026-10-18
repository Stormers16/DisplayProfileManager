using System;
using System.Collections.Generic;

using DisplayProfileManager.UI.ViewModels;

namespace DisplayProfileManager.UI.Presentation
{
    internal static class ProfileDefaultBadgePolicy
    {
        internal static void Synchronize(IEnumerable<ProfileViewModel> viewModels, string defaultProfileId)
        {
            if (viewModels == null)
                return;

            foreach (var viewModel in viewModels)
            {
                if (viewModel != null)
                    viewModel.IsDefault = string.Equals(viewModel.Id, defaultProfileId, StringComparison.Ordinal);
            }
        }
    }
}
