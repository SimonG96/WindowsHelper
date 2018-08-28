using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WindowsHelper.Events;
using WindowsHelper.Interfaces;
using Lib.Tools;

namespace WindowsHelper.ClipboardManager
{
    public class ClipboardManager : IPlugin
    {
        private const string PINNED_ENTRY_KEY = "PinnedEntry.";

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
            return true;
        }

        public void DeInit()
        {
            //TODO: Maybe save entries?
        }

        private void OnClipboardUpdate(object sender, EventArgs args) //TODO: Either remove entries with the same data or don't add on paste
        {
            object data = null;
            ClipboardObjectType type = ClipboardObjectType.NoData;

            if (Clipboard.ContainsText())
            {
                //data = Clipboard.GetDataObject()?.GetData(DataFormats.Text);
                data = Clipboard.GetText();
                type = ClipboardObjectType.String;
            }
            else if (Clipboard.ContainsImage())
            {
                data = Clipboard.GetImage();
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


        public void Dispose()
        {

        }

        #endregion Methods

    }
}