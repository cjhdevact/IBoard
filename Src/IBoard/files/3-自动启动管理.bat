::/*****************************************************\
::
::     IBoard - 3-�Զ���������.bat
::
::     ��Ȩ����(C) 2023-2024 CJH��
::
::     �Զ���������������
::
::\*****************************************************/
@echo off
cls
cd /d %~dp0
title ͼƬչʾС�����Զ���������
if "%1" == "/?" goto hlp
if "%1" == "/noadm" goto main
fltmc 1>nul 2>nul&& goto main
echo ���ڻ�ȡ����ԱȨ��...
echo.
echo ����ʹ�� %0 /noadm ����Bat��Ȩ�������ֶ��Թ���Ա�������
echo �����ǰ��������ѭ�����֣�����δ�ɹ���ȡ����ԱȨ�ޣ���ע����ǰ�û����������ԣ�
echo Ȼ���Թ���Ա�û��˺����л��ֶ��Թ���Ա������С�
if "%1" == "/mshtaadm" goto mshtaAdmin
if "%2" == "/mshtaadm" goto mshtaAdmin
if "%1" == "/psadm" goto powershellAdmin
if "%2" == "/psadm" goto powershellAdmin
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
title ͼƬչʾС�����Զ���������
cls
echo.
echo ====================================================
echo              ͼƬչʾС�����Զ���������
echo ====================================================
echo.
echo �����ʹ�����²�����
echo 3-�Զ���������.bat [/noadm ^| /mshtaadm ^| /psadm]
echo.
echo /noadm ����⵽�޹���ԱȨ�������Զ���Ȩ��
echo /mshtaadm ǿ��ʹ��mshta.exe�Զ���Ȩ��
echo /psadm ǿ��ʹ��Powershell.exe�Զ���Ȩ��
echo.
goto enda

:main
cls
echo.
echo ====================================================
echo             ͼƬչʾС�����Զ���������
echo ====================================================
echo ��ѡ����Ĳ�����
echo.
echo         1  ����Զ�����
echo         2  ɾ���Զ�����
echo.
echo         3  ����Զ�����������ƻ�ģʽ��
echo         4  ɾ���Զ�����������ƻ�ģʽ��
echo.
echo         5  �˳�
echo.
echo ========================================================
echo.
set /p chooice=�������Ӧ��������ִ����Ӧ�Ĳ�����
if "%chooice%" == "1" goto ad1
if "%chooice%" == "2" goto de1
if "%chooice%" == "3" goto ad2
if "%chooice%" == "4" goto del2
if "%chooice%" == "5" goto enda
echo.
echo ��Ч��ѡ���������ء� & pause >nul
goto main

:ad1
cls
echo ====================================================
echo             ͼƬչʾС�����Զ���������
echo ====================================================
Reg add HKLM\Software\Microsoft\Windows\CurrentVersion\run /v IBoard /t REG_SZ /d "%programfiles%\CJH\IBoard\IBoard.exe" /f
echo ��ӳɹ������������... & pause > nul
goto main

:de1
cls
echo ====================================================
echo             ͼƬչʾС�����Զ���������
echo ===================================================
Reg delete HKLM\Software\Microsoft\Windows\CurrentVersion\run /v IBoard /f
echo ɾ���ɹ������������... & pause > nul
goto main

:ad2
cls
echo ====================================================
echo             ͼƬչʾС�����Զ���������
echo ====================================================
schtasks.exe /Delete /TN \CJH\IBoard /F
schtasks.exe /create /tn \CJH\IBoard /xml "%~dp0IBoard.xml"
echo ��ӳɹ������������... & pause > nul
goto main

:de12
cls
echo ====================================================
echo             ͼƬչʾС�����Զ���������
echo ===================================================
schtasks.exe /Delete /TN \CJH\IBoard /F
echo ɾ���ɹ������������... & pause > nul
goto main

:enda
