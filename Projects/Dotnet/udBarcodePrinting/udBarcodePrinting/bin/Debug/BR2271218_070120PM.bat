@ECHO OFF
SET MyPath=%~dp0
SET MyPath=%MyPath:~0,-1%
set PCName=%ComputerName%

FOR /F "tokens=2* delims==" %%A in (
  'wmic printer where "default=True" get sharename /value'
  ) do SET DefaultPrinter=%%A
set PrinterShareName=%DefaultPrinter%

Copy  "%MyPath%\_BAT\_PRN\BR2271218_070120PM.prn" \\%PCName%\TSCR1
Pause