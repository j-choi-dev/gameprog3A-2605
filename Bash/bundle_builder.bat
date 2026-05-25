@echo off
setlocal EnableExtensions EnableDelayedExpansion

REM ======================================
REM Default values
REM ======================================

set "VERSION="
set "BUILD_TARGET=StandaloneWindows64"

REM bat 파일이 있는 Bash 폴더 기준으로 Unity 프로젝트 루트 계산
set "SCRIPT_DIR=%~dp0"
for %%I in ("%SCRIPT_DIR%..") do set "PROJECT_PATH=%%~fI"

REM 현재 시간 기반 기본 버전 생성: yyyyMMdd_HHmm
for /f %%I in ('powershell -NoProfile -ExecutionPolicy Bypass -Command "Get-Date -Format yyyyMMdd_HHmm"') do set "CURR_SYSTEM_TIME=%%I"

REM Windows Unity Hub 기본 경로
set "UNITY_EDITOR_ROOT=C:\Program Files\Unity\Hub\Editor"
set "UNITY_PATH="

REM Unity 자동 탐색: 가장 뒤에 정렬되는 버전 선택
if exist "%UNITY_EDITOR_ROOT%" (
    for /f "delims=" %%I in ('dir /b /ad "%UNITY_EDITOR_ROOT%" 2^>nul ^| sort') do (
        if exist "%UNITY_EDITOR_ROOT%\%%I\Editor\Unity.exe" (
            set "UNITY_PATH=%UNITY_EDITOR_ROOT%\%%I\Editor\Unity.exe"
        )
    )
)

REM 자동 탐색 실패 시 수동 기본값
if "%UNITY_PATH%"=="" (
    set "UNITY_PATH=C:\Program Files\Unity\Hub\Editor\2022.3.18f1\Editor\Unity.exe"
)

REM ======================================
REM Parse arguments
REM ======================================

:parse_args
if "%~1"=="" goto after_parse

if "%~1"=="-v" (
    if "%~2"=="" (
        echo Missing value for -v
        exit /b 1
    )
    set "VERSION=%~2"
    shift
    shift
    goto parse_args
)

if "%~1"=="-p" (
    if "%~2"=="" (
        echo Missing value for -p
        exit /b 1
    )
    set "PROJECT_PATH=%~2"
    shift
    shift
    goto parse_args
)

if "%~1"=="-t" (
    if "%~2"=="" (
        echo Missing value for -t
        exit /b 1
    )
    set "BUILD_TARGET=%~2"
    shift
    shift
    goto parse_args
)

if "%~1"=="-u" (
    if "%~2"=="" (
        echo Missing value for -u
        exit /b 1
    )
    set "UNITY_PATH=%~2"
    shift
    shift
    goto parse_args
)

echo Unknown argument: %~1
echo Usage: Bash\bundle_builder.bat [-v ^<version^>]
echo Example: Bash\bundle_builder.bat -v 20260526
echo Example without -v: Bash\bundle_builder.bat
exit /b 1

:after_parse

REM -v가 없으면 현재 시간 사용
if "%VERSION%"=="" (
    set "VERSION=%CURR_SYSTEM_TIME%"
)

if not exist "%UNITY_PATH%" (
    echo Unity executable not found:
    echo %UNITY_PATH%
    echo.
    echo Use -u option.
    echo Example:
    echo Bash\bundle_builder.bat -v 20260526 -u "C:\Program Files\Unity\Hub\Editor\2022.3.18f1\Editor\Unity.exe"
    exit /b 1
)

set "LOG_DIR=%PROJECT_PATH%\BuildLogs"
set "LOG_FILE=%LOG_DIR%\bundle_build_%VERSION%.log"

if not exist "%LOG_DIR%" (
    mkdir "%LOG_DIR%"
)

echo ======================================
echo AssetBundle Build Start
echo Unity Path   : %UNITY_PATH%
echo Project Path : %PROJECT_PATH%
echo Build Target : %BUILD_TARGET%
echo Version      : %VERSION%
echo Log File     : %LOG_FILE%
echo ======================================

"%UNITY_PATH%" ^
  -quit ^
  -batchmode ^
  -nographics ^
  -projectPath "%PROJECT_PATH%" ^
  -buildTarget "%BUILD_TARGET%" ^
  -executeMethod SampleEditorSDK.View.AssetBundleBuildMenuItem.AssetBundleBuildOnlyByExternal ^
  -v "%VERSION%" ^
  -logFile "%LOG_FILE%"

set "RESULT=%ERRORLEVEL%"

if not "%RESULT%"=="0" (
    echo AssetBundle Build Failed. ExitCode: %RESULT%
    echo Check log file: %LOG_FILE%
    exit /b %RESULT%
)

echo ======================================
echo AssetBundle Build Success
echo Output:
echo %PROJECT_PATH%\Builds\%BUILD_TARGET%\%VERSION%
echo ======================================

exit /b 0