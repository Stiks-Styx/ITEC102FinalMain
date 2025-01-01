Dim shell, scriptPath, exePath
Set shell = CreateObject("WScript.Shell")

' Get the path to the folder containing the VBScript
scriptPath = CreateObject("Scripting.FileSystemObject").GetParentFolderName(WScript.ScriptFullName)

' Path to the .exe file in the same folder as the script
exePath = """" & scriptPath & "\_Game_Main.exe"""

' PowerShell command to create a zoomed-out terminal and execute the .exe file
zoomOutAndRunCommand = "powershell -Command ""$Console = $Host.UI.RawUI; $Console.FontSize = $Console.FontSize - 100; Start-Process cmd -ArgumentList '/c " & exePath & "'"""

' Execute the command
shell.Run zoomOutAndRunCommand
