using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Lib.Tools
{
    public class RegistryHelper
    {
        #region Singleton

        public static RegistryHelper Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Registry Helper has not been initialized!");

                return _instance;
            }
        }

        private static RegistryHelper _instance;

        public static void Initialize(string rootKeyName)
        {
            _instance = new RegistryHelper(rootKeyName);
        }

        #endregion Singleton



        private RegistryKey _rootKey;

        private RegistryHelper(string rootKeyName)
        {
            _rootKey = Registry.CurrentUser.CreateSubKey($"Software\\{rootKeyName}");
        }

        private RegistryHelper(RegistryKey parentKey, string subKeyName)
        {
            _rootKey = parentKey.CreateSubKey(subKeyName);
        }

        public RegistryHelper SubKey(string subKeyName)
        {
            return new RegistryHelper(_rootKey, subKeyName);
        }

        
        
        #region Methods

        public bool Exists(string key)
        {
            return _rootKey.GetValue(key) != null;
        }

        public void Remove(string key)
        {
            _rootKey.DeleteValue(key, false);
        }


        public bool Set(string key, string value)
        {
            if (_rootKey == null)
                return false;

            _rootKey.SetValue(key, value);
            return true;
        }

        public string GetString(string key, string defaultValue)
        {
            if (_rootKey == null)
                return defaultValue;

            return (string) _rootKey.GetValue(key, defaultValue);
        }

        public bool Set(string key, bool value)
        {
            if (_rootKey == null)
                return false;
            
            _rootKey.SetValue(key, value);
            return true;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            if (_rootKey == null)
                return defaultValue;

            object value = _rootKey.GetValue(key, defaultValue);
            return Convert.ToBoolean(value);
        }

        public int GetNumberOfValues()
        {
            return _rootKey.ValueCount;
        }

        #endregion Methods
    }
}