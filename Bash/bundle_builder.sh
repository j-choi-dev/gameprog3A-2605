#!/usr/bin/env bash

set -e

curr_system_time=$(date +"%Y%m%d_%H%M")
VERSION=""

# bundle_builder.sh가 있는 Bash 폴더 기준으로 Unity 프로젝트 루트 계산
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_PATH="$(cd "$SCRIPT_DIR/.." && pwd)"

BUILD_TARGET="StandaloneWindows64"

# Windows 11 + Git Bash 기준 Unity 기본 경로
# Unity Hub 설치 버전에 맞게 자동 탐색을 먼저 시도함
UNITY_PATH=""

# Unity 자동 탐색
if [ -d "/c/Program Files/Unity/Hub/Editor" ]; then
  UNITY_PATH="$(find "/c/Program Files/Unity/Hub/Editor" -path "*/Editor/Unity.exe" -type f 2>/dev/null | sort | tail -n 1)"
fi

# 자동 탐색 실패 시 수동 기본값
if [ -z "$UNITY_PATH" ]; then
  UNITY_PATH="/c/Program Files/Unity/Hub/Editor/2022.3.18f1/Editor/Unity.exe"
fi

while [ $# -gt 0 ]; do
  case "$1" in
    -v)
      if [ -z "${2:-}" ]; then
        echo "Missing value for -v"
        exit 1
      fi
      VERSION="$2"
      shift 2
      ;;
    -p)
      if [ -z "${2:-}" ]; then
        echo "Missing value for -p"
        exit 1
      fi
      PROJECT_PATH="$2"
      shift 2
      ;;
    -t)
      if [ -z "${2:-}" ]; then
        echo "Missing value for -t"
        exit 1
      fi
      BUILD_TARGET="$2"
      shift 2
      ;;
    -u)
      if [ -z "${2:-}" ]; then
        echo "Missing value for -u"
        exit 1
      fi
      UNITY_PATH="$2"
      shift 2
      ;;
    *)
      echo "Unknown argument: $1"
      echo "Usage: sh Bash/bundle_builder.sh [-v <version>]"
      echo "Example: sh Bash/bundle_builder.sh -v 20260526"
      echo "Example without -v: sh Bash/bundle_builder.sh"
      exit 1
      ;;
  esac
done

# -v가 없으면 현재 시간 사용
if [ -z "$VERSION" ]; then
  VERSION="$curr_system_time"
fi

if [ ! -f "$UNITY_PATH" ]; then
  echo "Unity executable not found:"
  echo "$UNITY_PATH"
  echo ""
  echo "Use -u option."
  echo "Example:"
  echo "sh Bash/bundle_builder.sh -v 20260526 -u \"/c/Program Files/Unity/Hub/Editor/2022.3.18f1/Editor/Unity.exe\""
  exit 1
fi

LOG_DIR="$PROJECT_PATH/BuildLogs"
LOG_FILE="$LOG_DIR/bundle_build_$VERSION.log"

mkdir -p "$LOG_DIR"

echo "======================================"
echo "AssetBundle Build Start"
echo "Unity Path   : $UNITY_PATH"
echo "Project Path : $PROJECT_PATH"
echo "Build Target : $BUILD_TARGET"
echo "Version      : $VERSION"
echo "Log File     : $LOG_FILE"
echo "======================================"

"$UNITY_PATH" \
  -quit \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -buildTarget "$BUILD_TARGET" \
  -executeMethod SampleEditorSDK.View.AssetBundleBuildMenuItem.AssetBundleBuildOnlyByExternal \
  -v "$VERSION" \
  -logFile "$LOG_FILE"

RESULT=$?

if [ $RESULT -ne 0 ]; then
  echo "AssetBundle Build Failed. ExitCode: $RESULT"
  echo "Check log file: $LOG_FILE"
  exit $RESULT
fi

echo "======================================"
echo "AssetBundle Build Success"
echo "Output:"
echo "$PROJECT_PATH/Builds/$BUILD_TARGET/$VERSION"
echo "======================================"