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
		NEUTRAL,
		TITLE,
		SUFFIX
	}

	public struct Name
	{
		string title;
		string forename;
		string surname;
		string suffix;

		public Name(string _forename, string _surname, string _title = "", string _suffix = "")
		{
			forename = _forename;
			surname = _surname;
			title = _title;
			suffix = _suffix;
		}

		public override string ToString()
		{
			if(title != "")
			{
				return title + " " + forename + " " + surname + " " + suffix;
			}
			else return forename + " " + surname + " " + suffix;
		}
	}
}