using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CulturalNames
{
	public enum NameType
	{
		SURNAME,
		MALE,
		FEMALE,
		TITLE,
		SUFFIX
	}

	public struct Name
	{
		string title;
		string forename;
		string surname;
		string suffix;

		/// <summary>
		/// Constructor for Name; title and suffix default to empty for convenience.
		/// </summary>
		public Name(string _forename, string _surname, string _title = "", string _suffix = "")
		{
			forename = _forename;
			surname = _surname;
			title = _title;
			suffix = _suffix;
		}

		/// <summary>
		/// Returns a combined string of all the character's name components.
		/// </summary>
		public override string ToString()
		{
			if(title != "")
			{
				return title + " " + forename + " " + surname + " " + suffix;
			}
			else if(suffix != "")
			{
				return forename + " " + surname + " " + suffix;
			}
			else return forename + " " + surname;
		}
	}
}