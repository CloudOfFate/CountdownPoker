using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SpriteRepository
{
	public static Dictionary<string,Sprite[]> Sprites = new Dictionary<string, Sprite[]>();
	
	public static Sprite GetSpriteFromSheet(string pathName)
	{
		string query = @"((\w|-|_|\s)+\/)*((\w|-|_|\s)+)(_)(\d+)$";
		
		Match match = Regex.Match(pathName,query);
		
		//string sprName = match.Groups[3].Value; //Obtain the name of the spritesheet
		string sprPath = pathName.Remove(pathName.LastIndexOf('_')); //obtain the path to the spritesheet
		int sprIndex = int.Parse(match.Groups[6].Value);//obtain the index of the sprite you want from the sprite sheet
		
		Sprite[] group;
		//Check whether if the dictionary contains that spritesheet, if it doesn't, load the sheet into the dictionary
		if(!Sprites.TryGetValue(pathName,out group))
		{
			Sprites[pathName] = Resources.LoadAll<Sprite>(sprPath);
		}

		//return the sprite from that sprite sheet as a Sprite variable
		return Sprites[pathName][sprIndex];
	}
}
