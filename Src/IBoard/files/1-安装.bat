::/*****************************************************\
::
::     IBoard - 1-安装.bat
::
::     版权所有(C) 2023-2025 CJH。
::
::     安装批处理
::
::\*****************************************************/
@echo off
cls
title 图片展示小工具安装程序
if exist "%windir%\cpadver.bat" echo 检测到当前已安装教室计算机批量配置工具，请使用其安装包来卸载本软件。 & echo 任意键关闭... & pause > nul & goto enda
if exist "%windir%\csetver.bat" echo 检测到当前已安装计算机批量配置工具，请使用其安装包来卸载本软件。 & echo 任意键关闭... & pause > nul & goto enda
if "%1" == "/noadm" goto main
if "%1" == "/?" goto hlp
fltmc 1>nul 2>nul&& goto main
echo 正在获取管理员权限...
echo.
echo 可以使用 %0 /noadm 跳过Bat提权，但请手动以管理员身份运行
echo 如果当前窗口无限循环出现，或者未成功获取管理员权限，请注销当前用户或重启电脑，
echo 然后以管理员用户账号运行或手动以管理员身份运行。
if "%1" == "/mshtaadm" goto mshtaAdmin
if "%1" == "/psadm" goto powershellAdmin
ver | findstr "10\.[0-9]\.[0-9]*" >nul && goto powershellAdmin
:mshtaAdmin
rem 原理是利用mshta运行vbscript脚本给bat文件提权
rem 这里使用了前后带引号的%~dpnx0来表示当前脚本，比原版的短文件名%~s0更可靠
rem 这里使用了两次Net session，第二次是检测是否提权成功，如果提权失败则跳转到failed标签
rem 这有效避免了提权失败之后bat文件继续执行的问题
::Net session >nul 2>&1 || mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c ""%~dpnx0""","","runas",1)(window.close)&&exit
set parameters=
:parameter
@if not "%~1"=="" ( set parameters=%parameters% %~1& shift /1& goto :parameter)
set parameters="%parameters:~1%"
mshta vbscript:createobject("shell.application").shellexecute("%~dpnx0",%parameters%,"","runas",1)(window.close)&exit
cd /d "%~dp0"
Net session >nul 2>&1 || goto failed
goto main

:powershellAdmin
rem 原理是利用powershell给bat文件提权
rem 这里使用了两次Net session，第二次是检测是否提权成功，如果提权失败则跳转到failed标签
rem 这有效避免了提权失败之后bat文件继续执行的问题
Net session >nul 2>&1 || powershell start-process \"%0\" -argumentlist \"%1 %2\" -verb runas && exit
Net session >nul 2>&1 || goto failed
goto main

:failed
cls
echo.
echo 当前未以管理员身份运行。请手动以管理员身份运行本程序。
echo.
echo 任意键关闭... & pause > NUL
goto enda

:hlp
title 图片展示小工具安装程序
cls
echo.
echo ====================================================
echo               图片展示小工具安装程序
echo ====================================================
echo.
echo 你可以使用以下参数：
echo 1-安装.bat [/noadm ^| /mshtaadm ^| /psadm]
echo.
echo /noadm 当检测到无管理员权限跳过自动提权。
echo /mshtaadm 强制使用mshta.exe自动提权。
echo /psadm 强制使用Powershell.exe自动提权。
echo.
goto enda

:main
cd /d "%~dp0"
title 图片展示小工具安装程序
cls
echo.
echo ====================================================
echo               图片展示小工具安装程序
echo ====================================================
echo.
echo 版权所有(C) 2023-2025 CJH。
echo.
echo 安装前建议关闭杀毒软件以及在UAC设置中设置UAC等级为最低，否则在安装主程序或如果选择写入自动启动项会被拦截导致安装失败。
echo.
echo 任意键开始安装... & pause >nul

cls
echo.
echo ====================================================
echo               图片展示小工具安装程序
echo ====================================================
echo.
echo 正在安装中...
echo.
taskkill /f /im IBoard.exe
::ver|findstr "\<10\.[0-9]\.[0-9][0-9]*\>" > nul && (set netv=4)
ver|findstr "\<6\.[0-1]\.[0-9][0-9]*\>" > nul && (set netv=4c)
ver|findstr "\<6\.[2-9]\.[0-9][0-9]*\>" > nul && (set netv=4c)
ver|findstr "\<5\.[0-9]\.[0-9][0-9]*\>" > nul && (set netv=4c)

if "%PROCESSOR_ARCHITECTURE%"=="x86" goto x86
if "%PROCESSOR_ARCHITECTURE%"=="AMD64" goto x64

:x86
echo.

if exist "%programfiles%\CJH\IBoard\IBoard.exe" del /q "%programfiles%\CJH\IBoard\IBoard.exe"

if not exist "%programfiles%\CJH\IBoard" md "%programfiles%\CJH\IBoard"
copy "%~dp0IBoard.exe" "%programfiles%\CJH\IBoard\IBoard.exe"
echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)要安装策略到当前系统（安装后可以使用组策略编辑图片展示小工具的策略）（仅Windows Vista以上版本支持）（5秒后自动选择Y）"
if errorlevel 1 set ac=1
if errorlevel 2 set ac=2
if "%ac%" == "1" if exist "%windir%\PolicyDefinitions\*.admx" call "%~dp0IBoardAdmxs.exe"

echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)要创建快捷方式（5秒后自动选择Y）"
if errorlevel 1 set ad=1
if errorlevel 2 set ad=2
if "%ad%" == "1" if not exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具" md "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

if "%ad%" == "1" if exist "%userprofile%\..\Public\Desktop\图片展示小工具.lnk" del /q "%userprofile%\..\Public\Desktop\图片展示小工具.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%userprofile%\..\Public\Desktop\图片展示小工具.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo 正在安装软件根证书
echo.
echo 如果长时间停留在此操作，请检测是否被杀毒软件拦截。
echo.
certmgr.exe -add -c "%~dp0rootcert.cer" -s -r localMachine root

copy /y "%~dp02-卸载.bat" "%programfiles%\CJH\IBoard\Uninstall.bat"
copy /y "%~dp03-自动启动管理.bat" "%programfiles%\CJH\IBoard\AutoBootMgr.bat"
copy /y "%~dp0IBoard.xml" "%programfiles%\CJH\IBoard\IBoard.xml"

if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\AutoBootMgr.bat"":b.IconLocation=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)添加卸载程序列表（5秒后自动选择Y）"
if errorlevel 1 set ae=1
if errorlevel 2 set ae=2
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayIcon /t REG_SZ /d "%programfiles%\CJH\IBoard\IBoard.exe" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayName /t REG_SZ /d "图片展示小工具（IBoard）" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v Publisher /t REG_SZ /d "CJH" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v UninstallString /t REG_SZ /d "%programfiles%\CJH\IBoard\Uninstall.bat" /f

start /d "%programfiles%\CJH\IBoard" "" "%programfiles%\CJH\IBoard\IBoard.exe"

echo.
cls

echo.
echo ====================================================
echo               图片展示小工具安装程序
echo ====================================================
echo.
echo 安装完成！
echo 如果要开机自动启动本程序，请用开始菜单的“图片展示小工具”文件夹里的“管理自动启动”来配置开机自动启动。
echo 任意键退出... & pause > nul
goto enda

:x64
echo.

if exist "%programfiles%\CJH\IBoard\IBoard.exe" del /q "%programfiles%\CJH\IBoard\IBoard.exe"
if exist "%programfiles%\CJH\IBoard\x86\IBoard.exe" del /q "%programfiles%\CJH\IBoard\x86\IBoard.exe"

if not exist "%programfiles%\CJH\IBoard" md "%programfiles%\CJH\IBoard"
if not exist "%programfiles%\CJH\IBoard\x86" md "%programfiles%\CJH\IBoard\x86"
copy "%~dp0IBoard64.exe" "%programfiles%\CJH\IBoard\IBoard.exe"
copy "%~dp0IBoard.exe" "%programfiles%\CJH\IBoard\x86\IBoard.exe"
echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)要安装策略到当前系统（安装后可以使用组策略编辑图片展示小工具的策略）（仅Windows Vista以上版本支持）（5秒后自动选择Y）"
if errorlevel 1 set ac=1
if errorlevel 2 set ac=2
if "%ac%" == "1" if exist "%windir%\PolicyDefinitions\*.admx" call "%~dp0IBoardAdmxs.exe"

echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)要创建快捷方式（5秒后自动选择Y）"
if errorlevel 1 set ad=1
if errorlevel 2 set ad=2
if "%ad%" == "1" if not exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具" md "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具（32位）.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具（32位）.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\图片展示小工具（32位）.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\x86\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard\x86"":b.Save:close")

if "%ad%" == "1" if exist "%userprofile%\..\Public\Desktop\图片展示小工具.lnk" del /q "%userprofile%\..\Public\Desktop\图片展示小工具.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%userprofile%\..\Public\Desktop\图片展示小工具.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo 正在安装软件根证书
echo.
echo 如果长时间停留在此操作，请检测是否被杀毒软件拦截。
echo.
certmgr.exe -add -c "%~dp0rootcert.cer" -s -r localMachine root

copy /y "%~dp02-卸载.bat" "%programfiles%\CJH\IBoard\Uninstall.bat"
copy /y "%~dp03-自动启动管理.bat" "%programfiles%\CJH\IBoard\AutoBootMgr.bat"
copy /y "%~dp0IBoard.xml" "%programfiles%\CJH\IBoard\IBoard.xml"

if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\图片展示小工具\管理自动启动.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\AutoBootMgr.bat"":b.IconLocation=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo.
choice /C YN /T 5 /D Y /M "是(Y)否(N)添加卸载程序列表（5秒后自动选择Y）"
if errorlevel 1 set ae=1
if errorlevel 2 set ae=2
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayIcon /t REG_SZ /d "%programfiles%\CJH\IBoard\IBoard.exe" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayName /t REG_SZ /d "图片展示小工具（IBoard）" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v Publisher /t REG_SZ /d "CJH" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v UninstallString /t REG_SZ /d "%programfiles%\CJH\IBoard\Uninstall.bat" /f

start /d "%programfiles%\CJH\IBoard" "" "%programfiles%\CJH\IBoard\IBoard.exe"

echo.
cls

echo.
echo ====================================================
echo                图片展示小工具安装程序
echo ====================================================
echo.
echo 安装完成！
echo 如果要开机自动启动本程序，请用开始菜单的“图片展示小工具”文件夹里的“管理自动启动”来配置开机自动启动。
echo 任意键退出... & pause > nul
goto enda

:enda