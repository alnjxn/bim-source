; BIM Source SP.Writer for Revit 2014 Installer
#define ApplicationName 'SP.Writer for Revit 2014'
#define ApplicationVersion '2014.3.20'

[Setup]
AppName={#ApplicationName}
AppVersion={#ApplicationVersion}
AppPublisher=BIM Source
AppPublisherURL=http://bim-source.com/
DefaultDirName={userappdata}\Autodesk\Revit\Addins\2014
DisableDirPage=yes
DisableProgramGroupPage=yes
DefaultGroupName=BIM Source
Compression=lzma2
SolidCompression=yes
OutputBaseFilename={#ApplicationName} - {#ApplicationVersion}

[Files]
Source: "..\BIMSource.SPWriter\bin\Release\BIMSource.SPWriter.dll"; DestDir: "{app}"
Source: "..\BIMSource.SPWriter\BIMSource.SPWriter.addin"; DestDir: "{app}"
Source: "..\BIMSource.SPWriter\Resources\BIMSource.SPWriter.16.png"; DestDir: "{app}"
Source: "..\BIMSource.SPWriter\Resources\BIMSource.SPWriter.32.png"; DestDir: "{app}"

[Icons]
Name: "{group}\Uninstall {#ApplicationName}"; Filename: "{uninstallexe}"
