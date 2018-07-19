@echo off
cls
set arg1=%1
set arg2=%2
vfp9 "%arg1%" "%arg2%" /t /nologo
@echo on