using System;
using System.ComponentModel;
using System.IO;

namespace Extensions;

public class Logger {

	private readonly Output _output;
	private readonly Level _level;

	public Logger(string output, string level) {
		_output = output.ParseEnum<Output>();
		_level = level.ParseEnum<Level>();
	}

	public Logger(Output output, Level level) {
		_output = output;
		_level  = level;
	}

	public void Log<T>(string message, Level level, Exception exception = null, bool overrideFileName = false, string fileName = "") {
		var log = new LogFormat {
			DateTime  = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
			Source    = typeof(T).Name,
			Exception = exception?.ToString(),
			Level     = level.GetDescription(),
			Message   = message
		};

		if (level == _level) {
			WriteToFile(log, overrideFileName, fileName);
			WriteToConsole(log);
			// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
		} else switch (_level) {
			case Level.Debug:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Info when level != Level.Debug:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Warning when level is Level.Error or Level.Critical:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Error when level is Level.Error or Level.Critical:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
		}
	}

	public void Log(string message, Level level, Exception exception = null, bool overrideFileName = false, string fileName = "", string source = "Logger") {
		var log = new LogFormat {
			DateTime  = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
			Source    = source,
			Exception = exception?.ToString(),
			Level     = level.GetDescription(),
			Message   = message
		};

		if (level == _level) {
			WriteToFile(log, overrideFileName, fileName);
			WriteToConsole(log);
			// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
		} else switch (_level) {
			case Level.Debug:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Info when level != Level.Debug:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Warning when level is Level.Error or Level.Critical:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
			case Level.Error when level is Level.Error or Level.Critical:
				WriteToFile(log, overrideFileName, fileName);
				WriteToConsole(log);
				break;
		}
	}

	private void WriteToFile(LogFormat log, bool overrideFileName = false, string fileName = "") {

		var message = $"[{log.DateTime}][{log.Level.ToUpper()}] {log.Message}";
		if (!string.IsNullOrEmpty(log.Exception))
			message += $"{Environment.NewLine}{log.Exception}";

		var filename = overrideFileName ? $"{fileName}.txt" : $"{log.Source}.txt";
		if (_output != Output.File && _output != Output.FileConsole && _output != Output.FileWebService &&
		    _output != Output.All) return;
		if (!Directory.Exists("logs"))
			Directory.CreateDirectory("logs");

		if (!File.Exists($"logs/{filename}"))
			File.WriteAllText($"logs/{filename}", message);
		else {
			var text = File.ReadAllText($"logs/{filename}");
			File.WriteAllText($"logs/{filename}", $"{message}{Environment.NewLine}{text}");
		}

	}

	private void WriteToConsole(LogFormat log) {
		if (_output != Output.Console && _output != Output.FileConsole && _output != Output.WebServiceConsole &&
		    _output != Output.All) return;

		Console.WriteLine($"[{log.DateTime}][{log.Level.ToUpper()}][{log.Source}] {log.Message}");
		if (!string.IsNullOrEmpty(log.Exception))
			Console.WriteLine(log.Exception);
	}

	public enum Output {
		[Description("File")]
		File,
		[Description("Console")]
		Console,
		[Description("WebService")]
		WebService,
		[Description("FileConsole")]
		FileConsole,
		[Description("FileWebService")]
		FileWebService,
		[Description("WebServiceConsole")]
		WebServiceConsole,
		[Description("All")]
		All
	}

	public enum Level {
		[Description("Debug")]
		Debug,
		[Description("Info")]
		Info,
		[Description("Warning")]
		Warning,
		[Description("Error")]
		Error,
		[Description("Critical")]
		Critical
	}

}

public class LogFormat {
	public string Level { get; init; }
	public string Message { get; init; }
	public string DateTime { get; init; }
	public string Exception { get; init; }
	public string Source { get; init; }
}