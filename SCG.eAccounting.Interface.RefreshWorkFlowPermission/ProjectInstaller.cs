using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using ServiceInstaller;


namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : DynamicInstaller
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        
    }
}
