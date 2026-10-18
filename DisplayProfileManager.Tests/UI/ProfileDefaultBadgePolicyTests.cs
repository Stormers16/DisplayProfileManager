using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DisplayProfileManager.Profiles.Models;
using DisplayProfileManager.UI.Presentation;
using DisplayProfileManager.UI.ViewModels;

namespace DisplayProfileManager.Tests.UI
{
    [TestClass]
    public class ProfileDefaultBadgePolicyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Synchronize_DefaultSwitchAndClear_UpdateOnlyBoundDefaultProperties()
        {
            var first = new ProfileViewModel(new Profile("First")) { IsDefault = true, IsActive = true };
            var second = new ProfileViewModel(new Profile("Second"));
            var viewModels = new List<ProfileViewModel> { first, second };
            var notifications = new List<string>();

            first.PropertyChanged += (_, e) => notifications.Add("first:" + e.PropertyName);
            second.PropertyChanged += (_, e) => notifications.Add("second:" + e.PropertyName);

            ProfileDefaultBadgePolicy.Synchronize(viewModels, second.Id);

            Assert.IsFalse(first.IsDefault);
            Assert.IsTrue(second.IsDefault);
            Assert.IsTrue(first.IsActive);
            Assert.IsFalse(second.IsActive);
            CollectionAssert.AreEqual(
                new[] { "first:IsDefault", "second:IsDefault" }, notifications);

            notifications.Clear();
            ProfileDefaultBadgePolicy.Synchronize(viewModels, null);

            Assert.IsFalse(first.IsDefault);
            Assert.IsFalse(second.IsDefault);
            Assert.IsTrue(first.IsActive);
            CollectionAssert.AreEqual(new[] { "second:IsDefault" }, notifications);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Synchronize_UnchangedDefaultAndEmptyList_AreNonDisruptive()
        {
            var profile = new ProfileViewModel(new Profile("Default")) { IsDefault = true };
            int notifications = 0;
            profile.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(ProfileViewModel.IsDefault))
                    notifications++;
            };

            ProfileDefaultBadgePolicy.Synchronize(new[] { profile }, profile.Id);
            ProfileDefaultBadgePolicy.Synchronize(null, profile.Id);
            ProfileDefaultBadgePolicy.Synchronize(Array.Empty<ProfileViewModel>(), profile.Id);

            Assert.IsTrue(profile.IsDefault);
            Assert.AreEqual(0, notifications);
        }
    }
}
