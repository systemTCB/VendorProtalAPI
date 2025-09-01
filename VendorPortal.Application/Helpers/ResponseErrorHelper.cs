using System;
using System.Diagnostics;
using System.Linq;

namespace VendorPortal.Application.Helpers
{
    public static class ResponseErrorHelper
    {
        public static string GenerateErrorMessage(this object responseObject, string errorMessage = "",
       [System.Runtime.CompilerServices.CallerMemberName] string functionName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string codeError = GetCodeError(sourceFilePath);
            return $"{errorMessage} (DDT{codeError}_9_{sourceLineNumber})";
        }

        public static string GenerateErrorMessage(this object responseObject, Exception ex, string errorMessage = "",
        [System.Runtime.CompilerServices.CallerMemberName] string functionName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string fileName = (new StackTrace(ex, true)).GetFrame(0)?.GetFileName();
            string codeError = GetCodeError(fileName);
            int? lineError = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber();
            return $"{errorMessage} (DDT{codeError}_9_{(lineError != null && lineError != 0 ? lineError : sourceLineNumber)})";
        }

        private static string GetCodeError(string sourceFilePath)
        {
            var fileCallFunction = sourceFilePath.Split("\\").ToList();

            string file = fileCallFunction.LastOrDefault();
            if (!string.IsNullOrEmpty(file) && file.Contains('/'))
            {
                fileCallFunction = file.Split('/').ToList();
            }

            string classCallFunction = string.Empty;
            string typeFileError = string.Empty;

            if (fileCallFunction.Any() && fileCallFunction != null)
            {
                classCallFunction = fileCallFunction?.LastOrDefault().Substring(0, 3).ToUpper();

                if (fileCallFunction.LastOrDefault().Contains("API"))
                {
                    typeFileError = "A";
                }
                else if (fileCallFunction.LastOrDefault().Contains("Application"))
                {
                    typeFileError = "AP";
                }
                else if (fileCallFunction.LastOrDefault().Contains("Domain"))
                {
                    typeFileError = "D";
                }
                else if (fileCallFunction.LastOrDefault().Contains("Infrastructure.IoC"))
                {
                    typeFileError = "IoC";
                }
                else if (fileCallFunction.LastOrDefault().Contains("Infrastructure"))
                {
                    typeFileError = "I";
                }
                else if (fileCallFunction.LastOrDefault().Contains("Logging"))
                {
                    typeFileError = "L";
                }
            }

            return typeFileError + classCallFunction;
        }
    }
}