using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CulturalNames
{
	public class NameBuilder
	{
		static System.Random rnd = new System.Random();
		
		//all the names from files can be loaded into memory and stored here during runtime
		public static Dictionary<string, string[][]> loadedNames;

		#region Random Names
		/// <summary>
		/// Get a random name of a given kind
		/// </summary>	
		public static string RandomName(Gender gender, string culture)
		{
			string[][] names = new string[][]{};

			NameType nametype = (gender == Gender.MALE) ? NameType.MALE : NameType.FEMALE;
			
			if(loadedNames.TryGetValue(culture, out names))
			{
				return GetRandomArrayElement(names, nametype);
			}
			else return "-";
		}

		/// <summary>
		/// Gets a random name in its entirety
		/// </summary>
		public static Name RandomFullName(Gender gender, string culture)
		{
			string forename = "";
			string surname = "";

			string[][] names = new string[][]{};

			NameType nametype = (gender == Gender.MALE) ? NameType.MALE : NameType.FEMALE;

			if(loadedNames.TryGetValue(culture, out names))
			{
				forename = GetRandomArrayElement(names, nametype);
				surname = GetRandomArrayElement(names, NameType.SURNAME);
			}
			
			return new Name(forename, surname);
		}

		#endregion


		#region Utility

		///<summary>
		/// Loads and parses all the name files in "TextAssets/Names"
		///</summary>
		///
		public static void LoadNames()
		{
			string path = Application.streamingAssetsPath + "/TextAssets/Names";

			loadedNames = ParseDirectoryIntoCSVDictionary(path);
		}

		///<summary>
		/// Returns a random element from a given array
		///</summary>
		public static T GetRandomArrayElement<T>(T[][] array, NameType nameType)
		{
			return array[(int)nameType][rnd.Next(array[(int)nameType].Length)];
		}

		/// <summary>
		/// Loads the files in a local directory into a dictionary using the filename as a key
		/// </summary>
		/// <param name="path"> The absolute path to the file to load </param>
		/// <param name="fileExtension"> The extension of the file to load (eg ".csv", ".txt")</param>
		public static Dictionary<string, string> LoadDirectoryToDictionary(string path, string fileExtension)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();

			if(Directory.Exists(path))
			{
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo[] files = dir.GetFiles();
				int count = dir.GetFiles().Length;
				for(int i = 0; i < count; i++)
				{
					WWW file = new WWW(files[i].FullName);
					string loadedText = "";

					if(files[i].Extension == fileExtension && !files[i].Extension.Contains(".meta")) //curse you, .meta files
					{
						loadedText = file.text;

						if (loadedText == "")
						{
							Debug.Log("The requested " + fileExtension + " file at " + path + " could not be found.");
						}
						else
							Debug.Log("Loaded " + files[i].Name + " from " + path);
					}

					dict.Add(Path.GetFileNameWithoutExtension(files[i].Name), loadedText);
				}
			}

			return dict;
		}

		/// <summary>
		/// Takes a file at a path, reads into a dictionary, then converts to a dictionary with a string key and jagged array of strings as values
		/// </summary>
		public static Dictionary<string, string[][]> ParseDirectoryIntoCSVDictionary(string path)
		{
			Dictionary<string, string[][]> parsedDict = new Dictionary<string, string[][]>();

			Dictionary<string, string> oldDict = LoadDirectoryToDictionary(path, ".csv");

			//copy over from the text asset dict to the desired string, string[][] dict
			//by parsing the CSV
			foreach(KeyValuePair<string, string> entry in oldDict)
			{
				parsedDict.Add(entry.Key, CSVReader.ParseCSV(entry.Value));
			}

			return parsedDict;
		}


			#endregion
	}
}