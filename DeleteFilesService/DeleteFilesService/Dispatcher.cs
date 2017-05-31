using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeleteFilesService
{
    /// <summary>
    /// Program dispatcher.
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public Dispatcher(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                Settings.SettingsFilePath = args[0];
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                if (Settings.Init())
                {
                    FilesManager.Delete();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLogs(ex);
            }

        }
    }
}
