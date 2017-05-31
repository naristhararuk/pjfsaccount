using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ServiceInstaller;

namespace SCG.eAccounting.Resender
{
    [RunInstaller(true)]
    public partial class ResendingInstaller : DynamicInstaller
    {
        public ResendingInstaller()
        {
            InitializeComponent();
        }

    }
}
