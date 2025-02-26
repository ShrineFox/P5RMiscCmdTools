using AtlusScriptCompiler;
using AtlusScriptLibrary.Common.Logging;
using AtlusScriptLibrary.Common.Text.Encodings;
using AtlusScriptLibrary.Common.Libraries;
using AtlusScriptLibrary.FlowScriptLanguage;
using AtlusScriptLibrary.MessageScriptLanguage.Compiler;
using AtlusScriptLibrary.MessageScriptLanguage;
using AtlusScriptLibrary.FlowScriptLanguage.Decompiler;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace InitScriptMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach(var file in Directory.GetFiles(args[0], "*.BF", SearchOption.TopDirectoryOnly))
            {
                string procedure = ExtractProcedureText(GetFlowText(file));
                List<string> procedureLines = procedure.Split('\n').ToList();
                string procedureDeclaration = procedureLines[1].Trim('\r').Replace("()","") + "_hook()\r";
                procedureLines[1] = procedureDeclaration;
                string injectLogic = "\tif (FLD_GET_SCRIPT_TIMING() == 4)\r\n\t{\r\n\t\tSpawnCorruptions();\r\n\t}";
                procedureLines.Insert(3, injectLogic);
                procedure = string.Join('\n', procedureLines);

                procedure = $"import(\"../../_CustomScripts/Corruptions.flow\");\r\n\r\n{procedure}\r\n}}";
                
                File.WriteAllText(file.Replace(".BF", ".flow"), procedure);
            }
        }

        private static string ExtractProcedureText(string text)
        {
            var match = Regex.Match(text, @"// Procedure Index: 0(.*?)^\}", RegexOptions.Singleline | RegexOptions.Multiline);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private static string GetFlowText(string bfPath)
        {
            if (!File.Exists(bfPath) || Path.GetExtension(bfPath).ToLower() != ".bf")
                return null;

            FlowScript flowScript = FlowScript.FromFile(bfPath, null);
            FlowScriptDecompiler decompiler = new FlowScriptDecompiler()
                {
                    DecompileMessageScript = false, 
                    Library = LibraryLookup.GetLibrary("P5R"), 
                    SumBits = true 
                };

            string tempPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "temp.flow");
            InitializeScriptCompiler(bfPath, tempPath);

            if (File.Exists(tempPath))
                File.Delete(tempPath);

            if (decompiler.TryDecompile(flowScript, tempPath))
            {
                return File.ReadAllText(tempPath);
            }

            return null;
        }

        private static void InitializeScriptCompiler(string inputPath, string outputPath)
        {
            AtlusScriptCompiler.Program.IsActionAssigned = false;
            AtlusScriptCompiler.Program.InputFilePath = inputPath;
            AtlusScriptCompiler.Program.OutputFilePath = outputPath;
            //AtlusScriptCompiler.Program.MessageScriptEncoding = AtlusEncoding.Persona5RoyalEFIGS;
            //AtlusScriptCompiler.Program.MessageScriptTextEncodingName = AtlusEncoding.Persona5RoyalEFIGS.EncodingName;
            switch (Path.GetExtension(inputPath).ToLower())
            {
                case ".bmd":
                    AtlusScriptCompiler.Program.InputFileFormat = InputFileFormat.MessageScriptBinary;
                    break;
                case ".bf":
                    AtlusScriptCompiler.Program.InputFileFormat = InputFileFormat.FlowScriptBinary;
                    break;
                case ".flow":
                    AtlusScriptCompiler.Program.InputFileFormat = InputFileFormat.FlowScriptTextSource;
                    break;
                case ".msg":
                    AtlusScriptCompiler.Program.InputFileFormat = InputFileFormat.MessageScriptTextSource;
                    break;
            }
            AtlusScriptCompiler.Program.Logger = new Logger($"{nameof(AtlusScriptCompiler)}_{Path.GetFileNameWithoutExtension(outputPath)}");
            AtlusScriptCompiler.Program.Listener = new FileAndConsoleLogListener(true, LogLevel.Info | LogLevel.Warning | LogLevel.Error | LogLevel.Fatal);
        }
    }
}
