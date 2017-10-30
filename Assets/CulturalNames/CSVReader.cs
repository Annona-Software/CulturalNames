using UnityEngine;
using System.Linq; 
using System.Text;

namespace CulturalNames
{
	public class CSVReader
	{
		private const char delimiter = '|';
		private static char[] lineDelimiter = new char[] {'\n'};

		/// <summary>
		/// Takes CSV file from TextAssets and returns a jagged array of strings
		/// </summary>
		public static string[][] ParseCSV(string lines)
		{
			return lines.Replace("\r","").Split(lineDelimiter)
				.Select(line => line.Split(delimiter))
				.ToArray();
		}

		/// <summary>
		/// Prints CSV out as a string into Unity's debug logger for debugging issues.
		/// </summary>
		public static void DebugCSVOutput(string[][] data)
		{
			var output = new StringBuilder();

			foreach (string[] record in data) 
			{
				foreach (string field in record)
					output.Append (field + delimiter);

				output.Append ('\n');
			}
			
			Debug.Log(output);
		}
	}
}
