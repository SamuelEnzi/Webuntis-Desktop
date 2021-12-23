@echo off
cd Webuntis-Desktop
dotnet publish -c Release -o ..\publish --runtime win-x64 --self-contained false
exit