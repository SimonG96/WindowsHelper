using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WindowsHelper.Events;
using WindowsHelper.Interfaces;
using WindowsHelper.Settings;
using Lib.Tools;

namespace WindowsHelper.ClipboardManager
{
    public class ClipboardManager : IPlugin
    {
        public const int OPEN_CLIPBOARD_TRIES = 5;

        private const string PINNED_ENTRY_KEY = "PinnedEntry.";
        private const string SETTING_KEY = "Setting.";

        public ClipboardManager()
        {
            Settings = new ClipboardManagerSettings(this);

            ClipboardObjects = new ObservableCollection<ClipboardObjectViewModel>();
            ClipboardObjects.EnableCollectionSynchronization();

            SavedClipboardObjects = new ObservableCollection<ClipboardObjectViewModel>();
            SavedClipboardObjects.EnableCollectionSynchronization();

            ClipboardUpdatedEvent.ClipboardUpdated += OnClipboardUpdate;
            ClipboardObjectViewModel.PinStateChanged += OnClipboardObjectPinStateChanged;
            ClipboardObjectViewModel.ObjectDeleted += OnClipboardObjectDeleted;
        }


        public string Name => nameof(ClipboardManager);
        public ISettings Settings { get; }
        public ObservableCollection<ClipboardObjectViewModel> ClipboardObjects { get; set; }
        public ObservableCollection<ClipboardObjectViewModel> SavedClipboardObjects { get; set; }


        #region Methods
        

        public bool Init()
        {
            LoadPinnedItems();
            LoadSettings();
            return true;
        }

        public void DeInit()
        {
            //TODO: Maybe save entries?

            SaveSettings();
        }

        private void OnClipboardUpdate(object sender, EventArgs args) //TODO: Either remove entries with the same data or don't add on paste
        {
            object data = null;
            ClipboardObjectType type = ClipboardObjectType.NoData;

            if (ClipboardHelper.ContainsText(OPEN_CLIPBOARD_TRIES))
            {
                data = ClipboardHelper.GetText(OPEN_CLIPBOARD_TRIES);
                type = ClipboardObjectType.String;
            }
            else if (ClipboardHelper.ContainsImage(OPEN_CLIPBOARD_TRIES))
            {
                data = ClipboardHelper.GetImage(OPEN_CLIPBOARD_TRIES);
                type = ClipboardObjectType.Image;
            }

            if (data == null)
                return;

            if (SavedClipboardObjects.Any(i => i.Data.Equals(data)))
                return;

            var sameObjects = ClipboardObjects.Where(i => i.Data.Equals(data)).ToList();
            foreach (var sameObject in sameObjects)
            {
                ClipboardObjects.Remove(sameObject);
            }

            ClipboardObjectViewModel noDataItem = ClipboardObjects.FirstOrDefault(i => i.ClipboardObjectType == ClipboardObjectType.NoData);
            if (noDataItem != null)
                ClipboardObjects.Remove(noDataItem);

            ClipboardObjects.Insert(0, new ClipboardObjectViewModel(data, type));

            if (ClipboardObjects.Count <= ((ClipboardManagerSettings) Settings).MaxEntries)
                return;

            ClipboardObjectViewModel lastObject = ClipboardObjects.LastOrDefault();
            if (lastObject == null)
                return;

            ClipboardObjects.Remove(lastObject);
        }


        private void OnClipboardObjectPinStateChanged(object sender, bool isPinned)
        {
            if (isPinned)
            {
                ClipboardObjectViewModel clipboardObject = ClipboardObjects.FirstOrDefault(i => i.Equals(sender));
                ClipboardObjects.Remove(clipboardObject);
                SavedClipboardObjects.Add(clipboardObject);

                SavePinnedItem(clipboardObject);
            }
            else
            {
                ClipboardObjectViewModel savedClipboardObject = SavedClipboardObjects.FirstOrDefault(i => i.Equals(sender));
                int index = SavedClipboardObjects.IndexOf(savedClipboardObject);
                SavedClipboardObjects.Remove(savedClipboardObject);
                ClipboardObjects.Add(savedClipboardObject);

                DeletePinnedItem(index);
            }
        }

        private void SavePinnedItem(ClipboardObjectViewModel clipboardObject)
        {
            RegistryHelper.Instance.SubKey(Name).Set($"{PINNED_ENTRY_KEY}{SavedClipboardObjects.IndexOf(clipboardObject)}", (string) clipboardObject.Data);
        }

        private void DeletePinnedItem(int index)
        {
            RegistryHelper.Instance.SubKey(Name).Remove($"{PINNED_ENTRY_KEY}{index}");

            if (SavedClipboardObjects.Count <= index)
                return;

            for (int i = index + 1; i <= SavedClipboardObjects.Count; i++)
            {
                string key = $"{PINNED_ENTRY_KEY}{i}";
                if (!RegistryHelper.Instance.SubKey(Name).Exists(key))
                    continue;

                string value = RegistryHelper.Instance.SubKey(Name).GetString(key, "");
                RegistryHelper.Instance.SubKey(Name).Remove(key);

                if (value.IsNullOrEmpty())
                    continue;

                RegistryHelper.Instance.SubKey(Name).Set($"{PINNED_ENTRY_KEY}{i - 1}", value);
            }
        }

        private void LoadPinnedItems()
        {
            int numberOfValues = RegistryHelper.Instance.SubKey(Name).GetNumberOfValues();
            for (int i = 0; i < numberOfValues; i++)
            {
                string key = $"{PINNED_ENTRY_KEY}{i}";

                if (!RegistryHelper.Instance.SubKey(Name).Exists(key))
                    return; //Return if one key doesn't exist -> there may be other entries here (not only pinned entries) and there are no spaces with pinned entry keys

                string value = RegistryHelper.Instance.SubKey(Name).GetString(key, "");
                if (value.IsNullOrEmpty())
                    continue;

                SavedClipboardObjects.Insert(i, new ClipboardObjectViewModel(value, ClipboardObjectType.String) { IsPinned = true });
            }
        }

        private void OnClipboardObjectDeleted(object sender, EventArgs args)
        {
            if (!(sender is ClipboardObjectViewModel clipboardObject))
                return;

            if (!ClipboardObjects.Remove(clipboardObject))
                SavedClipboardObjects.Remove(clipboardObject);
        }

        private void SaveSettings()
        {
            var properties = Settings.GetType().GetProperties().Where(p => p.IsDefined(typeof(SettingsPropertyAttribute), false));
            foreach (var property in properties)
            {
                SettingsPropertyAttribute attribute = (SettingsPropertyAttribute)property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(SettingsPropertyAttribute));
                if (attribute == null)
                    continue;

                if (!attribute.Save)
                    continue;

                RegistryHelper.Instance.SubKey(Name).Set($"{SETTING_KEY}{property.Name}", property.GetValue(Settings));
            }
        }

        private void LoadSettings()
        {
            var properties = Settings.GetType().GetProperties().Where(p => p.IsDefined(typeof(SettingsPropertyAttribute), false));
            foreach (var property in properties)
            {
                SettingsPropertyAttribute attribute = (SettingsPropertyAttribute)property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(SettingsPropertyAttribute));
                if (attribute == null)
                    continue;

                if (!attribute.Save)
                    continue;

                string key = $"{SETTING_KEY}{property.Name}";
                if (!RegistryHelper.Instance.SubKey(Name).Exists(key))
                    continue;

                var value = RegistryHelper.Instance.SubKey(Name).GetObject(key, property.GetValue(Settings));
                property.SetValue(Settings, value);
            }
        }

        public void Dispose()
        {

        }

        #endregion Methods

    }
}