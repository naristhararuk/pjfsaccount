@echo off
set SERVICE_HOME=E:\ScheduleJob
set SERVICE_EXE=E:\ScheduleJob\SCG.eAccounting.Interface.RefreshWorkFlowPermission.exe
REM the following directory is for .NET 2.0.50727, your mileage may vary
set INSTALL_UTIL_HOME=C:\Windows\Microsoft.NET\Framework\v2.0.50727
REM Account credentials if the service uses a user account
set USER_NAME=
set PASSWORD=

set PATH=%PATH%;%INSTALL_UTIL_HOME%

cd %SERVICE_HOME%

echo Installing Service...
installutil /name=RefreshWorkFlowPermission /account=localsystem /user=%USER_NAME% /password=%PASSWORD% %SERVICE_EXE%

echo Done.