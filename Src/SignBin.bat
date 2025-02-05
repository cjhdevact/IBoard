::Tips Set the CSIGNCERT as your path.
@echo off
path D:\ProjectsTmp\SignPack;%path%
echo 任意键签名 图片展示小工具（IBoard）...
pause > nul
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoard.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoard64.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoardAdmxs.exe"
echo.
echo 完成！
echo 任意键退出...
pause > nul