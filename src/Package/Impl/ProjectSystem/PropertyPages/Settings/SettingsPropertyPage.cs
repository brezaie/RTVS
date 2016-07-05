﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.R.Package.ProjectSystem.PropertyPages.Settings {
    [Guid("EE42AA31-44FF-4A83-B098-45C4F98FE9C3")]
    internal class SettingsPropertyPage : PropertyPage {
        internal static readonly string PageName = Resources.ProjectProperties_SettingsPageTitle;
        private readonly SettingsPageControl _control;

        public SettingsPropertyPage() {
            _control = new SettingsPageControl(ConfiguredProperties);
            _control.DirtyStateChanged += OnDirtyStateChanged;
            this.Load += OnLoad;
        }

        private void OnDirtyStateChanged(object sender, EventArgs e) {
            IsDirty = _control.IsDirty;
        }

        private void OnLoad(object sender, EventArgs e) {
            this.Controls.Add(_control);
            this.AutoScroll = true;
        }

        protected override string PropertyPageName => PageName;

        protected override Task OnDeactivate() {
            return Task.CompletedTask;
        }

        protected override async Task<int> OnApply() {
            return await _control.SaveAsync() ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        protected override Task OnSetObjects(bool isClosing) {
            return Task.CompletedTask;
        }
    }
}
