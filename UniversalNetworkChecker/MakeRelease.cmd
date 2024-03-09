@echo off
echo Build Targets

setlocal enabledelayedexpansion
set ScriptLocation=%~d0%~p0
set BinFolderNet8=!ScriptLocation!..\bin\Release\net7.0


set ReleaseZipNet8=!BinFolderNet8!\..\UniversalNetworkChecker.zip

del !ReleaseZipNet8! 2> NUL

rd /q /s !BinFolderNet8!


dotnet build -c Release --no-restore 


7z a  !ReleaseZipNet8! -r "!BinFolderNet8!\*.*"


