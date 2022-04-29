@echo off
cd Webuntis-Desktop
dotnet publish -c Release -o ..\publish --runtime win-x64 --self-contained false
cd..
del %cd%\PackageManager\Package\Package.zip
start %cd%\PackageManager\PackageCreator.exe %cd%\publish %cd%\PackageManager\Package\Package.zip
exit