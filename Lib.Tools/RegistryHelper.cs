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

        public bool Set(string key, int value)
        {
            if (_rootKey == null)
                return false;

            _rootKey.SetValue(key, value);
            return true;
        }

        public int GetInt(string key, int defaultValue)
        {
            if (_rootKey == null)
                return defaultValue;

            object value = _rootKey.GetValue(key, defaultValue);
            return Convert.ToInt32(value);
        }

        public bool Set(string key, double value)
        {
            if (_rootKey == null)
                return false;

            _rootKey.SetValue(key, value);
            return true;
        }

        public double GetDouble(string key, double defaultValue)
        {
            if (_rootKey == null)
                return defaultValue;

            object value = _rootKey.GetValue(key, defaultValue);
            return Convert.ToDouble(value);
        }

        public bool Set(string key, object value)
        {
            if (_rootKey == null)
                return false;

            switch (value)
            {
                case string stringValue:
                    return Set(key, stringValue);
                case bool boolValue:
                    return Set(key, boolValue);
                case int intValue:
                    return Set(key, intValue);
                case double doubleValue:
                    return Set(key, doubleValue);
                default:
                    throw new InvalidOperationException($"No implementation for {value.GetType()}");
            }
        }

        //public T GetObject<T>(string key, T defaultValue)
        //{
        //    if (_rootKey == null)
        //        return defaultValue;

        //    if (typeof(T) == typeof(string))
        //        return (T) Convert.ChangeType(GetString(key, Convert.ToString(defaultValue)), typeof(T));
        //    else if (typeof(T) == typeof(bool))
        //        return (T) Convert.ChangeType(GetBool(key, Convert.ToBoolean(defaultValue)), typeof(T));
        //    else if (typeof(T) == typeof(int))
        //        return (T) Convert.ChangeType(GetInt(key, Convert.ToInt32(defaultValue)), typeof(T));
        //    else if (typeof(T) == typeof(double))
        //        return (T) Convert.ChangeType(GetDouble(key, Convert.ToDouble(defaultValue)), typeof(T));
        //    else
        //        throw new InvalidOperationException($"No implementation for {typeof(T)}");
        //}

        public object GetObject(string key, object defaultValue)
        {
            if (_rootKey == null)
                return defaultValue;

            switch (defaultValue)
            {
                case string defaultStringValue:
                    return GetString(key, defaultStringValue);
                case bool defaultBoolValue:
                    return GetBool(key, defaultBoolValue);
                case int defaultIntValue:
                    return GetInt(key, defaultIntValue);
                case double defaultDoubleValue:
                    return GetDouble(key, defaultDoubleValue);
                default:
                    throw new InvalidOperationException($"No implementation for {defaultValue.GetType()}");
            }
        }

        public int GetNumberOfValues()
        {
            return _rootKey.ValueCount;
        }

        #endregion Methods
    }
}