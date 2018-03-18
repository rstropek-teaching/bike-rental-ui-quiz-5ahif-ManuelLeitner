set /p migration=Migration-Name:
IF "%migration%" NEQ "" (
	dotnet ef migrations add %migration%
)
IF %ERRORLEVEL% NEQ 0 ( 
   pause 
)
dotnet ef database update
IF %ERRORLEVEL% NEQ 0 ( 
   pause 
)