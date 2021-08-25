@ECHO OFF
SET MyPath=%~dp0
SET MyPath=%MyPath:~0,-1%
set PCName=%ComputerName%

FOR /F "tokens=2* delims==" %%A in (
  'wmic printer where "default=True" get sharename /value'
  ) do SET DefaultPrinter=%%A
set PrinterShareName=%DefaultPrinter%

Copy  "%MyPath%\_PRN\bc1050320_034059PM.prn" \\%PCName%\HP1213
