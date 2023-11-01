// See https://aka.ms/new-console-template for more information
using CommandersCall;

Console.WriteLine("Hello, World!");
Logger.Entry.Information("Starting Commander's Call...");

string bug = "oops";
Logger.Entry.Debug("Some bug occurred {bug}", bug);