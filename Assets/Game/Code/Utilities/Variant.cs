using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities
{
    public class Variant
    {
        private readonly int? _int;
        private readonly float? _float;
        [CanBeNull]
        private readonly string _string;
        private readonly Dictionary<string, string> _dictionary;

        public Variant(int val)
        {
            _int = val;
        }

        public Variant(float val)
        {
            _float = val;
        }

        public Variant(string val)
        {
            _string = val;
        }

        public Variant(Dictionary<string, string> val)
        {
            _dictionary = val;
        }

        public Variant(Dictionary<string, object> val)
        {
            _dictionary = val.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
        }

        public int GetInt()
        {
            if (_int.HasValue)
                return _int.Value;

            throw new ArgumentException(CreateThrowMsg(typeof(int)));
        }

        public float GetFloat()
        {
            if (_float.HasValue)
                return _float.Value;

            throw new ArgumentException(CreateThrowMsg(typeof(float)));
        }

        public string GetString()
        {
            if (_string != null)
                return _string;

            throw new ArgumentException(CreateThrowMsg(typeof(string)));
        }

        public IReadOnlyDictionary<string, string> GetMap()
        {
            if (_dictionary != null)
                return _dictionary;

            throw new ArgumentException(CreateThrowMsg(typeof(Dictionary<string, string>)));
        }

        public bool HasInt()
        {
            return _int.HasValue;
        }

        public bool HasFloat()
        {
            return _float.HasValue;
        }

        public bool HasString()
        {
            return !string.IsNullOrEmpty(_string);
        }

        public bool HasMap()
        {
            return _dictionary != null;
        }

        private string CreateThrowMsg(Type t)
        {
            var additionalMsg = "";
            if (_int.HasValue)
                additionalMsg = " but contain int value == " + _int.Value.ToString(CultureInfo.InvariantCulture);
            else if (_float.HasValue)
                additionalMsg = " but contain float value == " + _float.Value.ToString(CultureInfo.InvariantCulture);
            else if (_string != null)
                additionalMsg = " but contain float value == " + _string;

            return "Variant does not contains " + t.ToString() + " val" + additionalMsg;
        }
    }
}