using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PeNet;

namespace CSPatternScanner
{
    public class PatternScanner
    {
        public PatternScanner(string filename)
        {
            _textBytes = GetTextSectionBytes(filename);
        }

        public int? Scan(Signature signature)
        {
            // Make sure the signature name doesn't contain a space
            if (signature.Name.Contains(" "))
            {
                throw new Exception("Signature names cannot contain spaces.");
            }

            Console.WriteLine($"Scanning for '{signature.Name}' with pattern '{signature.Pattern}'");

            SigPattern pattern = new SigPattern(signature.Pattern);

            for (int i = 0; i < _textBytes.Length; i++)
            {
                for (int j = 0; j < pattern.Bytes.Length; j++)
                {
                    byte file_byte = _textBytes[i+j];

                    if (!pattern.Match(file_byte, j))
                    {
                        break;
                    }

                    if (j == pattern.Bytes.Length - 1)
                    {
                        // We found the pattern
                        Console.WriteLine("Found");
                        return i + _virtualAddress;
                    }
                }
            }

            // Couldn't find pattern
            Console.WriteLine("Not found");
            return null;
        }

        private byte[] GetTextSectionBytes(string filename)
        {
            var fileBytes = File.ReadAllBytes(filename);

            var peHeader = new PeFile(filename);

            var textSection = peHeader.ImageSectionHeaders.First(s => s.Name == ".text");

            var startOfTextSection = Convert.ToInt32(textSection.PointerToRawData);
            var sizeOfTextSection = Convert.ToInt32(textSection.SizeOfRawData);

            _virtualAddress = Convert.ToInt32(textSection.VirtualAddress);

            var textSectionBytes = fileBytes.Skip(startOfTextSection).Take(sizeOfTextSection);

            return textSectionBytes.ToArray();
        }

        private byte[] _textBytes;
        private int _virtualAddress;
    }
}
