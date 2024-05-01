using PowershellCSharp;

string computerInfo = PowerShellHandler.Command("systeminfo | more");

Console.WriteLine(computerInfo);
Console.ReadLine();
