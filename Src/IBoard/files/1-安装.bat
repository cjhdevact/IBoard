::/*****************************************************\
::
::     IBoard - 1-��װ.bat
::
::     ��Ȩ����(C) 2023-2024 CJH��
::
::     ��װ������
::
::\*****************************************************/
@echo off
cls
title ͼƬչʾС���߰�װ����
if exist "%windir%\cpadver.bat" echo ��⵽��ǰ�Ѱ�װ���Ҽ�����������ù��ߣ���ʹ���䰲װ����ж�ر������ & echo ������ر�... & pause > nul & goto enda
if exist "%windir%\csetver.bat" echo ��⵽��ǰ�Ѱ�װ������������ù��ߣ���ʹ���䰲װ����ж�ر������ & echo ������ر�... & pause > nul & goto enda
if "%1" == "/noadm" goto main
if "%1" == "/?" goto hlp
fltmc 1>nul 2>nul&& goto main
echo ���ڻ�ȡ����ԱȨ��...
echo.
echo ����ʹ�� %0 /noadm ����Bat��Ȩ�������ֶ��Թ���Ա�������
echo �����ǰ��������ѭ�����֣�����δ�ɹ���ȡ����ԱȨ�ޣ���ע����ǰ�û����������ԣ�
echo Ȼ���Թ���Ա�û��˺����л��ֶ��Թ���Ա������С�
if "%1" == "/mshtaadm" goto mshtaAdmin
if "%1" == "/psadm" goto powershellAdmin
ver | findstr "10\.[0-9]\.[0-9]*" >nul && goto powershellAdmin
:mshtaAdmin
rem ԭ��������mshta����vbscript�ű���bat�ļ���Ȩ
rem ����ʹ����ǰ������ŵ�%~dpnx0����ʾ��ǰ�ű�����ԭ��Ķ��ļ���%~s0���ɿ�
rem ����ʹ��������Net session���ڶ����Ǽ���Ƿ���Ȩ�ɹ��������Ȩʧ������ת��failed��ǩ
rem ����Ч��������Ȩʧ��֮��bat�ļ�����ִ�е�����
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
rem ԭ��������powershell��bat�ļ���Ȩ
rem ����ʹ��������Net session���ڶ����Ǽ���Ƿ���Ȩ�ɹ��������Ȩʧ������ת��failed��ǩ
rem ����Ч��������Ȩʧ��֮��bat�ļ�����ִ�е�����
Net session >nul 2>&1 || powershell start-process \"%0\" -argumentlist \"%1 %2\" -verb runas && exit
Net session >nul 2>&1 || goto failed
goto main

:failed
cls
echo.
echo ��ǰδ�Թ���Ա������С����ֶ��Թ���Ա������б�����
echo.
echo ������ر�... & pause > NUL
goto enda

:hlp
title ͼƬչʾС���߰�װ����
cls
echo.
echo ====================================================
echo               ͼƬչʾС���߰�װ����
echo ====================================================
echo.
echo �����ʹ�����²�����
echo 1-��װ.bat [/noadm ^| /mshtaadm ^| /psadm]
echo.
echo /noadm ����⵽�޹���ԱȨ�������Զ���Ȩ��
echo /mshtaadm ǿ��ʹ��mshta.exe�Զ���Ȩ��
echo /psadm ǿ��ʹ��Powershell.exe�Զ���Ȩ��
echo.
goto enda

:main
cd /d "%~dp0"
title ͼƬչʾС���߰�װ����
cls
echo.
echo ====================================================
echo               ͼƬչʾС���߰�װ����
echo ====================================================
echo.
echo ��Ȩ����(C) 2023-2024 CJH��
echo.
echo ��װǰ����ر�ɱ������Լ���UAC����������UAC�ȼ�Ϊ��ͣ������ڰ�װ����������ѡ��д���Զ�������ᱻ���ص��°�װʧ�ܡ�
echo.
echo �������ʼ��װ... & pause >nul

cls
echo.
echo ====================================================
echo               ͼƬչʾС���߰�װ����
echo ====================================================
echo.
echo ���ڰ�װ��...
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
choice /C YN /T 5 /D Y /M "��(Y)��(N)Ҫ��װ���Ե���ǰϵͳ����װ�����ʹ������Ա༭ͼƬչʾС���ߵĲ��ԣ�����Windows Vista���ϰ汾֧�֣���5����Զ�ѡ��Y��"
if errorlevel 1 set ac=1
if errorlevel 2 set ac=2
if "%ac%" == "1" if exist "%windir%\PolicyDefinitions\*.admx" call "%~dp0IBoardAdmxs.exe"

echo.
choice /C YN /T 5 /D Y /M "��(Y)��(N)Ҫ������ݷ�ʽ��5����Զ�ѡ��Y��"
if errorlevel 1 set ad=1
if errorlevel 2 set ad=2
if "%ad%" == "1" if not exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����" md "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

if "%ad%" == "1" if exist "%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk" del /q "%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

copy /y "%~dp02-ж��.bat" "%programfiles%\CJH\IBoard\Uninstall.bat"
copy /y "%~dp03-�Զ���������.bat" "%programfiles%\CJH\IBoard\AutoBootMgr.bat"
copy /y "%~dp0IBoard.xml" "%programfiles%\CJH\IBoard\IBoard.xml"

if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\AutoBootMgr.bat"":b.IconLocation=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo.
choice /C YN /T 5 /D Y /M "��(Y)��(N)���ж�س����б�5����Զ�ѡ��Y��"
if errorlevel 1 set ae=1
if errorlevel 2 set ae=2
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayIcon /t REG_SZ /d "%programfiles%\CJH\IBoard\IBoard.exe" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayName /t REG_SZ /d "ͼƬչʾС���ߣ�IBoard��" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v Publisher /t REG_SZ /d "CJH" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v UninstallString /t REG_SZ /d "%programfiles%\CJH\IBoard\Uninstall.bat" /f

start /d "%programfiles%\CJH\IBoard" "" "%programfiles%\CJH\IBoard\IBoard.exe"

echo.
cls

echo.
echo ====================================================
echo               ͼƬչʾС���߰�װ����
echo ====================================================
echo.
echo ��װ��ɣ�
echo ���Ҫ�����Զ��������������ÿ�ʼ�˵��ġ�ͼƬչʾС���ߡ��ļ�����ġ������Զ������������ÿ����Զ�������
echo ������˳�... & pause > nul
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
choice /C YN /T 5 /D Y /M "��(Y)��(N)Ҫ��װ���Ե���ǰϵͳ����װ�����ʹ������Ա༭ͼƬչʾС���ߵĲ��ԣ�����Windows Vista���ϰ汾֧�֣���5����Զ�ѡ��Y��"
if errorlevel 1 set ac=1
if errorlevel 2 set ac=2
if "%ac%" == "1" if exist "%windir%\PolicyDefinitions\*.admx" call "%~dp0IBoardAdmxs.exe"

echo.
choice /C YN /T 5 /D Y /M "��(Y)��(N)Ҫ������ݷ�ʽ��5����Զ�ѡ��Y��"
if errorlevel 1 set ad=1
if errorlevel 2 set ad=2
if "%ad%" == "1" if not exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����" md "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС���ߣ�32λ��.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС���ߣ�32λ��.lnk"
if "%ad%" == "1" if exist "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk" del /q "%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\ͼƬչʾС���ߣ�32λ��.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\x86\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard\x86"":b.Save:close")

if "%ad%" == "1" if exist "%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk" del /q "%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk"
if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%userprofile%\..\Public\Desktop\ͼƬչʾС����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

copy /y "%~dp02-ж��.bat" "%programfiles%\CJH\IBoard\Uninstall.bat"
copy /y "%~dp03-�Զ���������.bat" "%programfiles%\CJH\IBoard\AutoBootMgr.bat"
copy /y "%~dp0IBoard.xml" "%programfiles%\CJH\IBoard\IBoard.xml"

if "%ad%" == "1" call mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(""%systemdrive%\ProgramData\Microsoft\Windows\Start Menu\Programs\ͼƬչʾС����\�����Զ�����.lnk""):b.TargetPath=""%programfiles%\CJH\IBoard\AutoBootMgr.bat"":b.IconLocation=""%programfiles%\CJH\IBoard\IBoard.exe"":b.WorkingDirectory=""%programfiles%\CJH\IBoard"":b.Save:close")

echo.
choice /C YN /T 5 /D Y /M "��(Y)��(N)���ж�س����б�5����Զ�ѡ��Y��"
if errorlevel 1 set ae=1
if errorlevel 2 set ae=2
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayIcon /t REG_SZ /d "%programfiles%\CJH\IBoard\IBoard.exe" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v DisplayName /t REG_SZ /d "ͼƬչʾС���ߣ�IBoard��" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v Publisher /t REG_SZ /d "CJH" /f
if "%ae%" == "1" Reg add HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IBoard /v UninstallString /t REG_SZ /d "%programfiles%\CJH\IBoard\Uninstall.bat" /f

start /d "%programfiles%\CJH\IBoard" "" "%programfiles%\CJH\IBoard\IBoard.exe"

echo.
cls

echo.
echo ====================================================
echo                ͼƬչʾС���߰�װ����
echo ====================================================
echo.
echo ��װ��ɣ�
echo ���Ҫ�����Զ��������������ÿ�ʼ�˵��ġ�ͼƬչʾС���ߡ��ļ�����ġ������Զ������������ÿ����Զ�������
echo ������˳�... & pause > nul
goto enda

:enda