using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace CulturalNames
{
	/// <summary>
	/// The heart of the asset; use random methods or utility methods to query the jagged array directly.
	/// </summary>
	public class NameBuilder
	{
		//all the names from files can be loaded into memory and stored here during runtime
		public static Dictionary<string, string[][]> loadedNames;

		static System.Random rnd = new System.Random();
		//to prevent duplicates
		static List<string> activeHashedNames = new List<string>();

#region Random Methods
		/// <summary>
		/// Get a random name of a given kind, not guaranteed to be unique
		/// </summary>	
		public static string RandomForename(Gender gender, string culture)
		{
			string[][] names = new string[][]{};

			NameType nametype = (gender == Gender.MALE) ? NameType.MALE : NameType.FEMALE;
			
			if(loadedNames.TryGetValue(culture, out names))
			{
				return GetNameElement(names, nametype);
			}
			
			Debug.Log("Name generation failed. Check the 'culture' parameter for errors.");
			return " - ";
		}

		/// <summary>
		/// Gets a random name in its entirety, not guaranteed to be unique
		/// </summary>
		public static Name RandomName(Gender gender, string culture)
		{
			string forename = " - ";
			string surname = " - ";

			string[][] names = new string[][]{};

			NameType nameType = (gender == Gender.MALE) ? NameType.MALE : NameType.FEMALE;

			if(loadedNames.TryGetValue(culture, out names))
			{
				forename = GetNameElement(names, nameType);
				surname = GetNameElement(names, NameType.SURNAME);
			}
			
			Debug.Log("Name generation failed. Check the 'culture' parameter for errors.");
			return new Name(forename, surname);
		}

		///<summary>
		/// Returns a random name guaranteed to be unique from a given culture.
		///</summary>
		public static Name RandomUniqueName(Gender gender, string culture)
		{
			string forename = " - ";
			string surname = " - ";
			string[][] names = new string[][]{};
			Name name;

			NameType nameType = (gender == Gender.MALE) ? NameType.MALE : NameType.FEMALE;

			if(loadedNames.TryGetValue(culture, out names))
			{
				int iterations = 0;
				do
				{
					forename = GetNameElement(names, nameType);
					surname = GetNameElement(names, NameType.SURNAME);
					name = new Name(forename, surname);
					iterations++;
					if(iterations > 20) //sanity check to prevent infinite loops
					{
						Debug.Log("Too few names in the CSV; multiple collisions have occurred.");
						break;
					}
				}
				while(!IsUniqueName(name));

				AddName(name); //add this name to the hashed list
				return name;
			}

			Debug.Log("Name generation failed. Check the 'culture' parameter for errors.");
			return new Name(forename, surname);
		}

#endregion

#region Initialization and List Management
		///<summary>
		/// Loads and parses all the name files in "TextAssets/Names"; call at initialization.
		///</summary>
		public static void LoadNames()
		{
			string path = Application.streamingAssetsPath + "/TextAssets/Names";

			loadedNames = ParseDirectoryIntoCSVDictionary(path);
		}

		/// <summary>
		/// Empties then refills the hashed names list.
		/// </summary>		
		public static void PopulateNameList(Name[] names)
		{
			activeHashedNames = new List<string>();

			for(int i = 0; i < names.Length; i++)
			{
				activeHashedNames.Add(HashName(names[i]));
			}
		}

		/// <summary>
		/// Adds a particular name's hash to the hash list
		/// </summary>
		public static void AddName(Name name)
		{
			if(IsUniqueName(name))
			{
				activeHashedNames.Add(HashName(name));
			}
			else Debug.Log("Cannot add to name list; this is a duplicate name." +
							" Try using RandomUniqueName to generate a name.");
			
		}

		/// <summary>
		/// Removes a particular name's hash from the hash list
		/// </summary>
		public static void RemoveName(Name name)
		{
			if(!IsUniqueName(name))
			{
				activeHashedNames.Remove(HashName(name));
			}
			else Debug.Log("Cannot remove the name from the list because it's not on the list.");
		}
#endregion


#region Utility
		/// <summary>
		/// Hashes a name using MD5 algorithm and returns as a string.
		/// </summary>
		static string HashName(Name name)
		{
			StringBuilder hashedName = new StringBuilder();
			MD5 md5Hasher = MD5.Create();

			Byte[] array = Encoding.ASCII.GetBytes(name.ToString());

			//hash the byte array
			foreach (Byte b in md5Hasher.ComputeHash(array))
			{
				hashedName.Append(b.ToString("x2"));
			}

			return hashedName.ToString();
		}

		/// <summary>
		/// Measures given name against all hashes stored in static list to prevent duplicates.
		/// </summary>
		static bool IsUniqueName(Name name)
		{
			string hashedName = HashName(name);

			return !activeHashedNames.Contains(hashedName);
		}

		/// <summary>
		/// Gets a single random name element from a given jagged array.
		/// </summary>
		public static string GetNameElement(string[][] array, NameType nameType)
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