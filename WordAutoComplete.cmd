
set destination=c:\temp\WordAutoComplete

rd %destination% /S/Q

robocopy ..\WordAutoComplete %destination% /E

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
