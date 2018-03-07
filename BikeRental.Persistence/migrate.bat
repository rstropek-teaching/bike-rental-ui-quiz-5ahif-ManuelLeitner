set /p migration=Migration-Name:
dotnet ef migrations add %migration%
dotnet ef database update
IF %ERRORLEVEL% NEQ 0 ( 
   pause 
)