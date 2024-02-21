using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSPatternScanner
{
    public class SigPattern
    {
        public SigPattern(string pattern)
        {
            // "BA 03 00 ?? 11"
            Pattern = pattern;

            ParsePattern(pattern);
        }

        private void ParsePattern(string pattern)
        {
            // Make sure this is a valid byte length
            List<byte> outBytes = new List<byte>();
            List<bool> outMask = new List<bool>();

            List<string> byteString = new List<string>();

            pattern = pattern.Replace(" ", "");

            try
            {
                for (int i = 0; i < pattern.Length; i = i + 2)
                {
                    byteString.Add(pattern.Substring(i, 2));
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Malformed pattern found. Make sure your pattern is an even number of digits.");
            }

            try
            {
                foreach (var s in byteString)
                {
                    if (s == "??")
                    {
                        // Any byte is valid in this place
                        outBytes.Add(0);
                        outMask.Add(false);
                    }
                    else
                    {
                        outBytes.Add(Convert.ToByte(s, 16));
                        outMask.Add(true);
                    }
                }
            }

            catch (FormatException)
            {
                throw new Exception("Invalid hex byte found in pattern. Ensure all pattern bytes are 0-9 a-f.");
            }

            Bytes = outBytes.ToArray();
            Mask = outMask.ToArray();
        }

        public bool Match(byte b, int pos)
        {
            var pattern_byte = Bytes[pos];
            var byteMustMatch = Mask[pos];

            return (b == pattern_byte || !byteMustMatch);
        }

        public string Pattern;
        public byte[] Bytes;
        public bool[] Mask;
    }
}
