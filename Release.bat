echo Build release files.
if "%1"=="" (
 set /p VERSION="input VERSION:"
) else (
 set  VERSION=%1
)
echo VERSION: %VERSION%
set UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2017.4.15f1\Editor\Unity.exe"
set LOG_FILE="release.log"
set PACKAGE_NAME="vketBoothValidator-%VERSION%.unitypackage"
set EXPORT_PACKAGES="Assets\VketBoothValidator"
subst Z: .
%UNITY_PATH%^
 -exportPackage %EXPORT_PACKAGES% %PACKAGE_NAME%^
 -projectPath "Z:\VketBoothValidator"^
 -batchmode^
 -nographics^
 -logfile %LOG_FILE%^
 -quit

mkdir Release\VketBoothValidator
move .\VketBoothValidator\%PACKAGE_NAME% Release\VketBoothValidator
copy /Y LICENSE Release\VketBoothValidator
copy /Y README.md Release\VketBoothValidator\README.txt
copy /Y ForDevelopper.md Release\VketBoothValidator\ForDevelopper.txt
subst Z: /D
