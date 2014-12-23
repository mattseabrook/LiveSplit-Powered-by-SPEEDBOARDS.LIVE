﻿using LiveSplit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpdateManager;

namespace LiveSplit.Updates
{
    public static class UpdateHelper
    {
        public static String GitHash { get { return GitInfo.hash; } }
        public static String GitBranch { get { return GitInfo.branch; } }
        public static readonly Version Version = Version.Parse("1.4.5");

        public static List<Type> AlreadyChecked = new List<Type>();

        public static void Update(Form form, Action closeAction, params IUpdateable[] updateables)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var actualUpdateables = updateables.Where(x => !AlreadyChecked.Contains(x.GetType()));
                    if (Updater.CheckForAnyUpdate(actualUpdateables))
                    {
                        String dialogText = actualUpdateables.Where(x => x.CheckForUpdate()).Select(x =>
                                x.UpdateName + " (" + x.GetNewVersion() + ")\r\n" +
                                x.GetChangeLog().Select(y => " - " + y + "\r\n")
                                        .Aggregate("", (y, z) => y + z) + "\r\n")
                                        .Aggregate((x, y) => x + y) + "Do you want to update?";
                        DialogResult result = (new ScrollableMessageBox()).Show(dialogText, "New updates are available", MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            try
                            {
                                Updater.UpdateAll(actualUpdateables, "http://livesplit.org/update/UpdateManager.exe");
                                closeAction();
                            }
                            catch (Exception e)
                            {
                                Log.Error(e);
                            }
                        }
                    }
                    AlreadyChecked.AddRange(actualUpdateables.Select(x => x.GetType()));
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            });
        }
    }
}
