using System;
using System.IO;
using System.Security.Cryptography;

namespace sha_batch
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length == 1)
				{
					var inputPath = args[0];

					if (Directory.Exists(inputPath))
					{
						string[] files = Directory.GetFiles(inputPath, "*.csv", SearchOption.AllDirectories);

						foreach (var file in files)
						{
							var inputFile = file;
							var outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + ".sha256");

							Console.WriteLine($"{inputFile} -> {outputFile}");

							var fileInfo = new FileInfo(inputFile);
							var hashResult = string.Empty;

							using (var mySHA256 = SHA256.Create())
							{
								var fileStream = fileInfo.Open(FileMode.Open);
								fileStream.Position = 0;
								byte[] hashValue = mySHA256.ComputeHash(fileStream);
								hashResult = ByteArrayToString(hashValue);
								fileStream.Close();
							}

							File.WriteAllText(outputFile, hashResult);
						}
					}
				}
				else
				{
					Console.WriteLine("You must specify one argument: a valid path.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ERROR] {ex.Message}");
			}
		}

		public static string ByteArrayToString(byte[] array)
		{
			string outputString = string.Empty;

			for (int i = 0; i < array.Length; i++)
			{
				outputString += $"{array[i]:X2}";
			}

			return outputString;
		}
	}
}
