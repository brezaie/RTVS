using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.FormattableString;

namespace Microsoft.R.Actions.Logging {
    public sealed class FileLogWriter : IActionLogWriter {
        private readonly char[] _lineBreaks = { '\n' };
        private readonly string _filePath;
        private readonly ActionBlock<string> _messages;

        private async Task WriteToFile(string message) {
            try {
                using (var stream = File.AppendText(_filePath)) {
                    await stream.WriteAsync(message);
                }
            } catch (UnauthorizedAccessException ex) {
                Trace.Fail(ex.ToString());
            } catch (PathTooLongException ex) {
                Trace.Fail(ex.ToString());
            } catch (DirectoryNotFoundException ex) {
                Trace.Fail(ex.ToString());
            } catch (NotSupportedException ex) {
                Trace.Fail(ex.ToString());
            } catch (IOException ex) {
                Trace.Fail(ex.ToString());
            }
        }

        public FileLogWriter(string filePath) {
            _filePath = filePath;
            _messages = new ActionBlock<string>(new Func<string, Task>(WriteToFile));
        }

        public Task WriteAsync(MessageCategory category, string message) {
            return _messages.SendAsync(GetStringToWrite(category, message));
        }

        private string GetStringToWrite(MessageCategory category, string message) {
            var categoryString = GetCategoryString(category);
            var prefix = Invariant($"[{DateTime.Now:yy-M-dd_HH-mm-ss}]{categoryString}:");
            if (!message.Take(message.Length - 1).Contains('\n')) {
                return prefix + message;
            }

            var emptyPrefix = new string(' ', prefix.Length);
            var lines = message.Split(_lineBreaks, StringSplitOptions.RemoveEmptyEntries)
                .Select((line, i) => i == 0 ? prefix + line + "\n" : emptyPrefix + line + "\n");
            return string.Concat(lines);
        }

        public static FileLogWriter InTempFolder(string fileName) {
            var path = $@"{Path.GetTempPath()}/{fileName}_{DateTime.Now:yyyyMdd_HHmmss}.log";
            return new FileLogWriter(path);
        }

        private static string GetCategoryString(MessageCategory category) {
            switch (category) {
                case MessageCategory.Error:
                    return "[ERROR]";
                case MessageCategory.Warning:
                    return "[WARNING]";
                default:
                    return string.Empty;
            }
        }
    }
}