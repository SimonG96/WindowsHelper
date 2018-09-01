using System;

namespace WindowsHelper.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingsPropertyAttribute : Attribute //TODO: Is there any more information needed here? (e.g. name or something similar)
    {
        public SettingsPropertyAttribute(bool save = false, bool setManual = true)
        {
            Save = save;
            SetManual = setManual;
        }


        /// <summary>
        /// Decides whether this Entry is saved to Registry or not
        /// </summary>
        public bool Save { get; set; }

        /// <summary>
        /// Decides whether this Entry is shown in it's settings tab or not
        /// </summary>
        public bool SetManual { get; set; }
    }
}