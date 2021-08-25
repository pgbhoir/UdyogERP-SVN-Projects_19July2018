set MyPath=%cd%
echo %MyPath%
set PCName=%ComputerName%

FOR /F "tokens=2* delims==" %%A in (
  'wmic printer where "default=True" get sharename /value'
  ) do SET DefaultPrinter=%%A
set PrinterShareName=%DefaultPrinter%

Copy  "%MyPath%\_PRN\AmbikaPr1271218_124137PM.prn" \\%PCName%\TSCR1
