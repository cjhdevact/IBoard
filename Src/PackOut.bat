@echo off
echo 任意键打包 图片展示小工具（IBoard）...
pause > nul
if exist %~dp0IBoard-Bin rd /s /q %~dp0IBoard-Bin
md %~dp0IBoard-Bin
copy %~dp0IBoard\bin\Release\IBoard.exe %~dp0IBoard-Bin\IBoard.exe
copy %~dp0IBoard\bin\x64\Release\IBoard.exe %~dp0IBoard-Bin\IBoard64.exe

copy %~dp0IBoard\files\1-安装.bat %~dp0IBoard-Bin\1-安装.bat
copy %~dp0IBoard\files\2-卸载.bat %~dp0IBoard-Bin\2-卸载.bat
copy %~dp0IBoard\files\3-自动启动管理.bat %~dp0IBoard-Bin\3-自动启动管理.bat
copy %~dp0IBoard\files\IBoardAdmxs.exe %~dp0IBoard-Bin\IBoardAdmxs.exe
copy %~dp0IBoard\files\IBoard.xml %~dp0IBoard-Bin\IBoard.xml

echo.
echo 完成！
echo 任意键退出...
pause > nul