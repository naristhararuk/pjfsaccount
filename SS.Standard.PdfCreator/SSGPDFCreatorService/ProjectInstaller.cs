using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace SSG.PDF.Service
{
    /// <summary>
    /// The class ProjectInstaller used for installing windows service.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// The constructor of project installer.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
