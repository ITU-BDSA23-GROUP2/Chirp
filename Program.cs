﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Expressions;
using SimpleDB;
using DocoptNet;

const string usage = @"Chirp

Usage:
  Chirp.exe read
  Chirp.exe cheep <message>
  Chirp.exe (-h | --help)

Options:
  -h --help     Show this screen.
";

var arguments = new Docopt().Apply(usage, args, exit: true)!;

string command = args[0];

var db = new CSVDatabase<Cheep>();


if (arguments.ContainsKey("read")) {
    UserInterface.PrintCheeps(db.Read());
} else if(arguments.ContainsKey("cheep")) {
    //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
    
    var author = System.Environment.MachineName;
    var message =  args[1] ;
    var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
    var cheep = new Cheep(author,message,timestamp); 
    db.Store(cheep);
}



