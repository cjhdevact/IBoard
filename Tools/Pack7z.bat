::Tips:
::Set the CSIGNCERT as your Certificate File path.
::Set the CSIGNTOOL as your SignPackPath
@echo off
path %CSIGNTOOL%;%path%
echo [PACK] 正在打包
if exist "%~dp0IBoard-Bin.7z" del /q "%~dp0IBoard-Bin.7z"
cmd.exe /c c7z.cmd "%~dp0IBoard-Bin" "%~dp0IBoard-Bin.7z"
echo [PACK] 打包结束
echo.