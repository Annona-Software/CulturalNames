[![Annona](https://games.annonasoftware.com/AnnonaLogo.png)](https://games.annonaosoftware.com)
# Cultural Names
> "What's in a name? that which we call a rose
> By any other word would smell as sweet..."
> William Shakespeare, "Romeo and Juliet", II.ii.46-47

**Cultural Names** is a free, open-source Unity asset to create randomized character names from a user- or dev-moddable series of .csv files.

## Features
  - Get a randomized name by culture and by gender
  - Totally customizable: put your own .csvs in the StreamingAssets folder, or let your users do the same for mods
  - Hash-based name collision prevention to prevent duplicate names
  - Add as many or few culture types as you want—the sky's the limit
  - Well-documented and commented code for ease of use in any IDE

## Installation

Clone the repo or download the asset directly into your project via the Unity Asset Store (coming soon!).

## Asset Contents
This repo includes two folders: *CulturalNames* and *StreamingAssets*, plus a demo scene called `Demo`.
*CulturalNames* includes all the code to run the naming system, organized into a single namespace. The only non-namespace script is `Test.cs`, which is included merely to show in debug logs how the generation works. `Test.cs` is included on an empty game object in the `Demo` scene. Entering play mode causes the script to load and parse all the name files located in *StreamingAssets*, and then generate a random assortment of names, including a mal-formed parameter call to demonstrate the error handling.

*StreamingAssets* must be located in the root (Assets) folder according to arcane Unity law. Feel free to move, delete, and rename the other folder and files to your heart's content. The *StreamingAssets* folder comes with four .csv files that hold names for Anglo-Saxon, Early Roman, Germanic, and Irish cultures, just as an example of what it can do.

## Usage
### Basic Design
`Test.cs` shows the simple usage of this asset:
```csharp
		NameBuilder.LoadNames(); //loads all the .csv files in StreamingAssets
		for(int i = 0; i < 10; i++) //generate some random names!
		{
			Debug.Log(NameBuilder.RandomUniqueName(Gender.FEMALE, "EarlyRoman").ToString());
			Debug.Log(NameBuilder.RandomUniqueName(Gender.MALE, "Irish").ToString());
		}
```
The asset as much as possible makes use of enums or structs to avoid the errors common with "stringly-typed" parameters. `Gender` is defined as "male" or "female" and determines what type of names are chosen for a character; `Name` is a struct containing four strings: `forename`, `surname`, `title`, and `suffix`, and with its own overridden `ToString()` method, as demonstrated above.

The culture names are taken directly from whatever you or the users name the .CSV files, and are stored in a dictionary alongside the parsed .csv, which itself is stored in a jagged array. Obviously, hardcoding a random name using `NameBuilder.RandomUniqueName` prevents your users from including their modded .csvs; we recommend instead using it in the context of the UI. For instance, you could populate a dropdown with the keys from the `loadedNames` dictionary (made public for this purpose), then allowing the user to decide on a culture name for a character. If you have suggestions for more components to add into the core asset, let us know!

### Unique vs. True Random
Without knowing the particulars of your setup, you might run into problems if a name is duplicated; alternatively, it might not matter and indeed be desired behavior. We've included three main public methods for generating names: `RandomForename`, `RandomName`, and `RandomUniqueName`. `RandomForename` and `RandomName` return a string and a `Name` respectively, the first drawn only from the list of forenames in row 2 and 3 of each .csv (sometimes called "Christian names" or "first names"), and the second building a forename and a surname. Both of these have no constraints: given a short list of names, they could conceivably return many duplicates, particularly if hundreds or thousands of names are being generated.

`RandomUniqueName`, however, takes advantage of MD5 hashing by storing a list of all the names currently in use by the project, which is then tested against by each newly generated name to ensure it doesn't collide. If it does, a new name is generated, until a unique name is found.

**Important Note**: If you are saving and loading data in your game (as most do), you will need to use the initialization method `PopulateNameList(Name[] names)` to populate that hashed list with all existing names, or else it won't know about already existing names that may collide! Since we don't know your project, we've just made this simple method that you can use during initialization or load; we've also included simple `AddName` and `RemoveName` methods that can be independently called to modify existing names in the hashed list.


### Modding
The great part about using the *StreamingAssets* folder is that your users can add their own .csvs to expand the names to their hearts' content. Each .csv is organized like so:
```
surnames 
male forenames 
female forenames
titles
suffixes
```
The delimiter (separation character) for the .csv is a pipe ("|"), which can be changed in `CSVReader.cs` at line 12 if so desired. We chose this character as it's not potentially found in names or titles, unlike an apostrophe or comma.

You may include however many names you want in each row, just be sure there are no spaces between each name and the delimiter (standard .csv behavior), like so:
`Adam|Bob|Charles|Duke`
Don't worry about leaving a row empty; as you can see from the example files, all of them don't include one or more rows.

## Questions? Issues? Pull Requests?
We'd love to hear feedback on what works, what doesn't, and what you'd love to see in this free asset. Please let us know, either by leaving us an issue or a pull request for fixes and bugs, or drop us a line at [our site](https://games.annonasoftware.com/contact/). Don't forget to check out our [paid asset](http://u3d.as/Y6b) **UI Automator** in the Unity Asset Store as well!

## Donations
If you feel this deserves a beer or two, or maybe just a hearty digital handshake, feel free to [drop us a few bucks/Euros/[insert currency here] through PayPal](https://www.paypal.me/AnnonaSoftware/). We'd love to be able to continue to offer free assets to the community, and a few dollars here and there goes a long way towards helping us part-time game devs do just that.

## License (MIT)
Copyright (c) 2017 Annona Software LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.