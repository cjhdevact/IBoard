::Tips Set the CSIGNCERT as your path.
@echo off
path D:\ProjectsTmp\SignPack;%path%
echo �����ǩ�� ͼƬչʾС���ߣ�IBoard��...
pause > nul
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoard.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoard64.exe"
cmd.exe /c signcmd.cmd "%CSIGNCERT%" "%~dp0IBoard-Bin\IBoardAdmxs.exe"
echo.
echo ��ɣ�
echo ������˳�...
pause > nul