using System;
using CommandLine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CSPatternScanner
{
    internal class Program
    {
        public class Options
        {
            [Option('s', "signatures", Required = true, HelpText = "Signatures json file path.")]
            public string SignatureFile { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file path.")]
            public string OutputFile { get; set; }

            [Option('f', "file", Required = true, HelpText = "File to scan for signatures.")]
            public string InputFile { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (!File.Exists(o.InputFile))
                {
                    Console.WriteLine($"Couldn't find input file: {o.InputFile}");
                    Environment.Exit(1);
                }

                if (!File.Exists(o.SignatureFile))
                {
                    Console.WriteLine($"Couldn't find signatures file: {o.SignatureFile}");
                    Environment.Exit(1);
                }

                // Deserialize signatures json file
                List<Signature> signatures = JsonConvert.DeserializeObject<List<Signature>>(File.ReadAllText(o.SignatureFile));

                PatternScanner scanner = new PatternScanner(o.InputFile);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("// This file was generated automatically by CSPatternScanner.exe");
                sb.AppendLine();

                foreach (var signature in signatures)
                {
                    // Find this pattern in our file bytes
                    int? address = scanner.Scan(signature);

                    if (address != null)
                    {
                        if (signature.Comment != string.Empty) {
                            sb.AppendLine($"#define {signature.Name} 0x{address:X}; // {signature.Comment}");
                        }
                        else
                        {
                            sb.AppendLine($"#define {signature.Name} 0x{address:X};");
                        }
                    } else
                    {
                        sb.AppendLine($"#define {signature.Name} NOTFOUND;");
                    }
                }

                File.WriteAllText(o.OutputFile, sb.ToString());
            });
        }
    }
}
