@echo off
set SERVICE_HOME=D:\ServiceQas
set SERVICE_EXE=D:\ServiceQas\SCG.eAccounting.Interface.RefreshWorkFlowPermission.exe
REM the following directory is for .NET 2.0.50727, your mileage may vary
set INSTALL_UTIL_HOME=C:\Windows\Microsoft.NET\Framework\v2.0.50727
REM Account credentials if the service uses a user account
set PATH=%PATH%;%INSTALL_UTIL_HOME%

cd %SERVICE_HOME%

echo Uninstalling Service...
installutil /u /name=RefreshWorkFlowPermissionQas %SERVICE_EXE%

echo Done.
