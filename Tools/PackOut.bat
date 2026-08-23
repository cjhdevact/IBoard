@echo off
echo [PACKOUT] 正在打包程序
if exist "%~dp0IBoard-Bin" rd /s /q "%~dp0IBoard-Bin"
md "%~dp0IBoard-Bin"
copy "%~dp0..\Src\IBoard\files\1-安装.bat" "%~dp0IBoard-Bin\1-安装.bat"
copy "%~dp0..\Src\IBoard\files\2-卸载.bat" "%~dp0IBoard-Bin\2-卸载.bat"
copy "%~dp0..\Src\IBoard\files\3-自动启动管理.bat" "%~dp0IBoard-Bin\3-自动启动管理.bat"
copy "%~dp0..\Src\IBoard\files\IBoardAdmxs.exe" "%~dp0IBoard-Bin\IBoardAdmxs.exe"
copy "%~dp0..\Src\IBoard\files\IBoard.xml" "%~dp0IBoard-Bin\IBoard.xml"
copy "%~dp0..\Src\IBoard\bin\Release\IBoard.exe" "%~dp0IBoard-Bin\IBoard.exe"
copy "%~dp0..\Src\IBoard\bin\x64\Release\IBoard.exe" "%~dp0IBoard-Bin\IBoard64.exe"
copy "%~dp0..\Src\IBoard\files\certmgr.exe" "%~dp0IBoard-Bin\certmgr.exe"
copy "%~dp0..\Src\IBoard\files\rootcert.cer" "%~dp0IBoard-Bin\rootcert.cer"
echo [PACKOUT] 打包完成
echo.