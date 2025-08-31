@echo off
:: -----------------------------------------------------------------------------
:: Professional Project Structure Exporter
:: Creates a UTF-8 sorted & filtered project file list with total count
:: -----------------------------------------------------------------------------

:: Change code page to UTF-8 (Windows)
chcp 65001 >nul

setlocal enabledelayedexpansion

:: Output file
set "OUTPUT=project_structure.txt"

:: Clear old output
if exist "%OUTPUT%" del "%OUTPUT%"

:: Write header
echo Project Structure - %DATE% %TIME% > "%OUTPUT%"
echo ======================================= >> "%OUTPUT%"
echo. >> "%OUTPUT%"

:: Folder / file filters (ignore list)
set "FILTER=/c:"\\bin\\" /c:"\\obj\\" /c:"\\Debug\\" /c:"\\Release\\" ^
             /c:"\\logs\\" /c:"\\temp\\" /c:"\\Backup\\" ^
             /c:".dll" /c:".pdb" /c:".exe" /c:".cache" ^
             /c:".log" /c:".nuget" /c:"Generated" ^
             /c:"AssemblyInfo" /c:"Up2Date" /c:".resources.dll" ^
             /c:"project.assets.json" /c:".deps.json" ^
             /c:"runtimeconfig.json" /c:".sourcelink.json" ^
             /c:".editorconfig" /c:".gitignore" ^
             /c:"*.nupkg" /c:"*.zip" /c:"*.rar""

:: Temp file for sorting
set "TEMPFILE=%TEMP%\project_files.tmp"
if exist "%TEMPFILE%" del "%TEMPFILE%"

set /a count=0

:: Find files and apply filters
for /f "usebackq tokens=*" %%f in (`
    dir /a-d /b /s 2^>nul ^| findstr /i /v %FILTER%
`) do (
    set "fullpath=%%f"
    set "relpath=!fullpath:%CD%\=!"
    set "relpath=!relpath:\=/!"
    echo !relpath! >> "%TEMPFILE%"
    set /a count+=1
)

:: Sort and save final list
sort "%TEMPFILE%" >> "%OUTPUT%"
del "%TEMPFILE%" >nul 2>&1

:: Add footer with total file count
echo. >> "%OUTPUT%"
echo ======================================= >> "%OUTPUT%"
echo Total files: %count% >> "%OUTPUT%"

:: Display result
echo.
echo Project structure saved to: %cd%\%OUTPUT%
echo Total files: %count%
echo Done.
pause
exit /b
