@echo off
REM =======================================
REM 專案自動 LFS 設定與推送
REM =======================================

setlocal
set PROJECT_NAME=Resolution

REM 1. 檢查 Git 是否安裝
git --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Git 未安裝！
    echo 請先安裝 Git: https://git-scm.com/download/win
    pause
    exit /b
)

REM 2. 檢查 Git LFS 是否安裝
git lfs version >nul 2>&1
if errorlevel 1 (
    echo [INFO] Git LFS 未安裝，正在安裝...
    git lfs install
) else (
    echo [INFO] Git LFS 已安裝
)

REM 3. 追蹤大檔案
echo [INFO] 追蹤大檔案 (png, tga, tif, asset, zip)...
git lfs track "*.png"
git lfs track "*.tga"
git lfs track "*.tif"
git lfs track "*.asset"
git lfs track "*.zip"

REM 4. 新增 .gitattributes
git add .gitattributes
git commit -m "Setup LFS for large files" 

REM 5. 將所有檔案加入 Git
git add .
set /p MSG="請輸入這次更新說明 (commit message): "
if "%MSG%"=="" set MSG="Update files"

git commit -m "%MSG%"

REM 6. 提升 buffer
git config --global http.postBuffer 524288000

REM 7. Push 到 GitHub
git push origin main --force

echo [INFO] 推送完成！
pause
