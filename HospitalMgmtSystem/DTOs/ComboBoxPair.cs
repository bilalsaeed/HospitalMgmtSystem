﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.DTOs
{
    public class ComboBoxPair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ComboBoxPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
