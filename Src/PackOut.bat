@echo off
echo �������� ͼƬչʾС���ߣ�IBoard��...
pause > nul
if exist %~dp0IBoard-Bin rd /s /q %~dp0IBoard-Bin
md %~dp0IBoard-Bin
copy %~dp0IBoard\bin\Release\IBoard.exe %~dp0IBoard-Bin\IBoard.exe
copy %~dp0IBoard\bin\x64\Release\IBoard.exe %~dp0IBoard-Bin\IBoard64.exe

copy %~dp0IBoard\files\1-��װ.bat %~dp0IBoard-Bin\1-��װ.bat
copy %~dp0IBoard\files\2-ж��.bat %~dp0IBoard-Bin\2-ж��.bat
copy %~dp0IBoard\files\3-�Զ���������.bat %~dp0IBoard-Bin\3-�Զ���������.bat
copy %~dp0IBoard\files\IBoardAdmxs.exe %~dp0IBoard-Bin\IBoardAdmxs.exe
copy %~dp0IBoard\files\IBoard.xml %~dp0IBoard-Bin\IBoard.xml

echo.
echo ��ɣ�
echo ������˳�...
pause > nul