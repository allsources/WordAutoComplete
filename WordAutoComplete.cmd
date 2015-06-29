
set destination=c:\temp\WordAutoComplete

rd %destination% /S/Q

xcopy ..\WordAutoComplete\README.md %destination%\ /Y
xcopy ..\WordAutoComplete\WordAutoComplete.sln %destination%\ /Y
robocopy ..\WordAutoComplete\Build %destination%\Build /E
robocopy ..\WordAutoComplete\WordAutoComplete %destination%\WordAutoComplete /E
robocopy ..\WordAutoComplete\WordAutoCompleteTest %destination%\WordAutoCompleteTest /E

del %destination%\*.cmd
del %destination%\*.suo /F/Q/A:H
del %destination%\Build\*.xml /F/Q
del %destination%\Build\*.vshost.* /F/Q
del %destination%\Build\log.txt

rd %destination%\TestResults /S/Q
rd %destination%\WordAutoComplete\bin /S/Q
rd %destination%\WordAutoComplete\obj /S/Q
rd %destination%\WordAutoCompleteTest\bin /S/Q
rd %destination%\WordAutoCompleteTest\obj /S/Q
