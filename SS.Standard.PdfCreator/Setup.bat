@echo off
IF [%1]==[] GOTO E_FLAG

echo "Create folder " %1
MD %1
MD %1\bin
MD %1\bin\Tex
MD %1\bin\Pdf
MD %1\bin\Log

:L_COPY
echo "Installing Library..."
xcopy /y SSGPDFCreator\bin\Release\*.*  %1\bin

echo "Installing Windows Service"
xcopy /y SSGPDFCreatorService\bin\Release\SSGPDFCreatorService.*  %1\bin
xcopy /y Test\bin\Release\Test.* %1\bin
xcopy /y Test\Tex\*.* %1\bin\Tex
xcopy /y InstallService.bat %1\bin
xcopy /y UninstallService.bat %1\bin

C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe /u %1\bin\SSGPDFCreatorService.exe
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe %1\bin\SSGPDFCreatorService.exe

echo "Installing Web Service"
xcopy /y SSGPDFCreatorWebService\PDFCreatorWebService.asmx %1
xcopy /y SSGPDFCreatorWebService\Web.config %1
xcopy /y SSGPDFCreatorWebService\App_Data %1
xcopy /y SSGPDFCreatorWebService\bin\SSGPDFCreatorWebService.* %1\bin

GOTO L_FINISH

:E_FLAG
echo "Error!!! Please specify destination folder."
GOTO L_END

:L_FINISH
echo "Installing completed."

:L_END