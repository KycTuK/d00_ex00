﻿using System;
using System.IO;
using System.Globalization;

int GetMinNumber(int a, int b, int c)
{
	return ((a = a > b ? b : a) > c ? c : a);
}

int GetLevenshteinDistance(string str1, string str2)
{
	int IsCharEqual;
	int len1 = str1.Length + 1;
	int len2 = str2.Length + 1;
	int [,]mas = new int [len1, len2];

	for (int i = 0; i < len1; i++)
		mas[i, 0] = i;
	for (int i = 0; i < len2; i++)
		mas[0, i] = i;
	for (int i = 1; i < len1; i++)
		for (int j = 1; j < len2; j++)
		{
			IsCharEqual = str1[i - 1] == str2[j - 1] ? 0 : 1;
			mas[i, j] = GetMinNumber(mas[i, j - 1] + 1, mas[i - 1, j] + 1, mas[i - 1, j - 1] + IsCharEqual);
		}
	return (mas[len1 - 1, len2 - 1]);
}

bool SearchTheName(string SearchedName, string[] NameList)
{
	foreach (string Name in NameList)
	{
		if (SearchedName == Name)
			return (true);
	}
	return (false);
}

bool NameClarification(string Name)
{
	ConsoleKey PressedKey;

	Console.Write($">Did you mean ”{Name}”? (Y/N): ");
	while (true)
	{
		PressedKey = Console.ReadKey().Key;
		Console.WriteLine();
		if (PressedKey == ConsoleKey.Y)
			return (true);
		else
		if (PressedKey == ConsoleKey.N)
			return (false);
		else
			Console.Write(">The answer can be either Y (yes) or N (no): ");
	}
}

bool IsValid(string Name)
{
	if (Name.Length == 0)
		return (false);
	foreach (char c in Name)
		if (!(Char.ToUpper(c) >= 'A' && Char.ToUpper(c) <= 'Z' || " -".Contains(c)))
			return (false);
	return (true);
}

string NameVerification(string Name, ref string[] NamesList)
{
	Name.Trim();
	if (!IsValid(Name))
		return (null);
	if (SearchTheName(Name, NamesList))
		return (Name);
	foreach (string PredictName in NamesList)
	{
		if (GetLevenshteinDistance(Name, PredictName) < 2 && NameClarification(PredictName))
			return (PredictName);
	}
	return (null);
}

string Name;
string FileName;
string[] NamesList;

FileName = "names.txt";

CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
if (!File.Exists(FileName))
{
	Console.WriteLine($"File ”{FileName}” Read Error! Check if it exists and available.");
	Environment.Exit(0);
}

NamesList = File.ReadAllLines(FileName);
Console.Write(">Enter Name: ");
if ((Name = NameVerification(Console.ReadLine(), ref NamesList)) == null)
	Console.WriteLine("Your Name was not found!");
else
	Console.WriteLine($"Hello, {Name}!");
