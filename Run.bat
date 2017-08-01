@echo Dot Net Hero build and run batch file.
@echo Requires .NET Core 1.1 Runtime to be installed.
@echo Get it at: https://www.microsoft.com/net/download/core
@echo Attempting to build and run...
@Echo OFF

timeout /t 3

cd src\dotnethero\

dotnet restore
dotnet build
dotnet run