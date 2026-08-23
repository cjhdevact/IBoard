'****************************************************************************
'    IBoard
'    Copyright (C) 2023-2026 CJH
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'****************************************************************************
'/*****************************************************\
'*                                                     *
'*     IBoard - pfrm.vb                                *
'*                                                     *
'*     Copyright (c) CJH.                              *
'*                                                     *
'*     The picture show form.                          *
'*                                                     *
'\*****************************************************/
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.IO

Public Class pfrm
    Public pfile As String
    Public pst As Integer
    Public Hsate As Integer
    Public ImgAutoLoad As Integer
    Public MyTitle As String
    Public UseMoveV As Integer
    Public LoadLastImage As Integer
    Public UnSaveData As Integer
    Public DisbFuState As Integer
    Public ShowModeTips As Integer
    Public SaveLoc As Integer
    Public BootLoc As New Point
    Public BootLocErr As Integer
    Public NeedStillTopMost As Integer '是否强制顶置
    'Public FixLoc As Integer

    Public scaleX As Single
    Public scaleY As Single

    '在Alt+Tab中隐藏
    Const WS_EX_COMPOSITED = &H2000000 '0x02000000
    'Const WS_EX_NOACTIVATE = &H8000000 '0x08000000
    Const WS_EX_TOOLWINDOW = &H80 '0x00000080
    'Const WS_EX_TRANSPARENT = &H20 '0x00000020
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_TOOLWINDOW Or WS_EX_COMPOSITED
            Return cp
        End Get
    End Property


    '生成文件Hash函数
    Public Function GetFileHash(ByVal filePath As String, ByVal hashType As String) As String
        Dim hashAlgorithm As Security.Cryptography.HashAlgorithm = Nothing
        Select Case hashType.ToUpper()
            Case "MD5"
                hashAlgorithm = MD5.Create()
            Case "SHA1"
                hashAlgorithm = SHA1.Create()
            Case "SHA256"
                hashAlgorithm = SHA256.Create()
            Case Else
                Throw New ArgumentException("Unsupported hash type.")
        End Select

        Using fileStream As IO.FileStream = IO.File.OpenRead(filePath)
            Dim hashBytes = hashAlgorithm.ComputeHash(fileStream)
            Return BitConverter.ToString(hashBytes).Replace("-", "").ToLower()
        End Using
    End Function
    '<DllImport("kernel32.dll", SetLastError:=True)>
    'Private Shared Function SetDllDirectory(ByVal lpPathName As String) As Boolean
    'End Function
    '<DllImport("TimeControl.UIAccess.dll", EntryPoint:="PrepareForUIAccess", CallingConvention:=CallingConvention.Cdecl)>
    'Private Shared Function PrepareForUIAccess() As Integer
    'End Function
    ' DLL导入
    <DllImport("CJH.UIAccessHelper.dll")>
    Private Shared Function PrepareForUIAccess() As UInteger
    End Function
    <DllImport("CJH.UIAccessHelper.dll")>
    Private Shared Function CheckUIAccessStatus() As Boolean
    End Function
    Private Const ERROR_SUCCESS As UInteger = 0

    '该方法检测管理员身份在win8 .net framework 4.5测试下会导致程序崩溃
    '如果使用该方法，建议手动以管理员身份运行或升级为 .net framework 4.5.1 以上版本
    'Public Function IsAdministrator() As Boolean
    '    Return New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
    'End Function 

    '该方法检测管理员身份在win8 .net framework 4.5测试下如果开了UAC但不提示会导致程序崩溃，如果有UAC提示则正常显示
    '如果使用该方法，建议手动以管理员身份运行或升级为 .net framework 4.5.1 以上版本
    Public Function IsAdministrator() As Boolean
        Try
            Return IsUserAnAdmin()
        Catch
            Return False
        End Try
    End Function

    <DllImport("shell32.dll")>
    Private Shared Function IsUserAnAdmin() As Boolean
    End Function

    'API移动窗体
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean
    Declare Function ReleaseCapture Lib "user32" Alias "ReleaseCapture" () As Boolean
    Const WM_SYSCOMMAND = &H112
    Const SC_MOVE = &HF010&
    Const HTCAPTION = 2
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        If UseMoveV = 1 Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0)
            'FixLoc = 1
            If SaveLoc = 1 Then
                If UnSaveData = 0 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", Me.Location.X, RegistryValueKind.DWord, "HKCU")
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", Me.Location.Y, RegistryValueKind.DWord, "HKCU")
                End If
            End If
        End If
    End Sub
    Private Sub pfrm_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Move
        'FixLoc = 1
        If SaveLoc = 1 Then
            If UnSaveData = 0 Then
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", Me.Location.X, RegistryValueKind.DWord, "HKCU")
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", Me.Location.Y, RegistryValueKind.DWord, "HKCU")
            End If
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        If UseMoveV = 1 Then
            'FixLoc = 1
            Dim a As New System.Drawing.Point
            a.X = Me.Location.X
            a.Y = Me.Location.Y
            ReleaseCapture()
            SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0)
            If Me.Location.X = a.X And Me.Location.Y = a.Y Then
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    'Dim stopwatch As New Stopwatch()
                    'stopwatch.Start() ' 开始计时
                    'If e.Button = Windows.Forms.MouseButtons.Left Then
                    '    stopwatch.Stop()
                    '    Dim latency As Long = stopwatch.ElapsedMilliseconds
                    '    If latency < 1000 Then
                    '        Call PictureBox1_DoubleClick(sender, e)
                    '    End If
                    'End If
                    'stopwatch.Stop()
                    Call PictureBox1_DoubleClick(sender, e)
                End If
            End If
            If SaveLoc = 1 Then
                If UnSaveData = 0 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", Me.Location.X, RegistryValueKind.DWord, "HKCU")
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", Me.Location.Y, RegistryValueKind.DWord, "HKCU")
                End If
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NeedStillTopMost = 0
        prms.ShowDialog()
        NeedStillTopMost = 1
    End Sub

    Private Sub PictureBox1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick
        If pst = 0 Then
            pst = 1
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            FlowLayoutPanel1.Visible = False
            Me.Text = ""
        Else
            pst = 0
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            FlowLayoutPanel1.Visible = True
            Me.Text = "图片展示"
        End If
    End Sub

    Sub disbfu()
        DisbFuState = 1
        prms.Label11.Text = "部分功能由于被管理员禁用而无法使用。"
        prms.CheckBox1.Enabled = False
        prms.CheckBox2.Enabled = False
        prms.CheckBox3.Enabled = False
        prms.CheckBox4.Enabled = False
        prms.CheckBox5.Enabled = False
        prms.CheckBox6.Enabled = False
        prms.ComboBox2.Enabled = False
        prms.Button2.Enabled = False
        prms.Button8.Visible = False
    End Sub

    Private Sub PictureBox1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragDrop
        LoadImage(e.Data.GetData(DataFormats.FileDrop)(0))
    End Sub
    Private Sub PictureBox1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragEnter
        e.Effect = DragDropEffects.Link ' 接受拖放数据，启用拖放效果
    End Sub

    Private Sub Me_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        'e.Cancel = True
        Select Case (e.CloseReason)
            '应用程序要求关闭窗口
            Case CloseReason.ApplicationExitCall
                e.Cancel = False '不拦截，响应操作
                '自身窗口上的关闭按钮
            Case CloseReason.FormOwnerClosing
                e.Cancel = True '拦截，不响应操作
                'MDI窗体关闭事件
            Case CloseReason.MdiFormClosing
                e.Cancel = True '拦截，不响应操作
                '不明原因的关闭
            Case CloseReason.None
                e.Cancel = False
                '任务管理器关闭进程
            Case CloseReason.TaskManagerClosing
                e.Cancel = True '拦截，不响应操作
                '用户通过UI关闭窗口或者通过Alt+F4关闭窗口
            Case CloseReason.UserClosing
                e.Cancel = True '拦截，不响应操作
                '操作系统准备关机()
            Case (CloseReason.WindowsShutDown)
                e.Cancel = False '不拦截，响应操作
        End Select

    End Sub

    Private Sub pfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If IsAdministrator() = True Then
            prms.Text = "设置图片展示小工具（管理员）"
        End If

        NeedStillTopMost = 1
        Timer1.Enabled = True
        ' 获取当前窗体的 DPI
        Dim currentDpiX As Single = Me.CreateGraphics().DpiX
        Dim currentDpiY As Single = Me.CreateGraphics().DpiY

        'If currentDpiX <> 96 OrElse currentDpiY <> 96 Then
        'End If
        '计算缩放比例
        scaleX = currentDpiX / 96
        scaleY = currentDpiY / 96

        '////////////////////////////////////////////////////////////////////////////////////
        '//
        '//  禁用功能策略注册表读取
        '//
        '////////////////////////////////////////////////////////////////////////////////////
        DisbFuState = 0
        UnSaveData = 0
        ShowModeTips = 1
        Dim cdisbfu As Integer = 0
        Dim cdisbfut As Integer = 0
        Dim unsavefut As Integer = 0
        Try

            Dim plkeycr As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Policies\CJH\IBoard", True)

            Dim disfucrt As Integer = -1
            If (Not plkeycr Is Nothing) Then
                disfucrt = plkeycr.GetValue("DisableFeaturesTip", -1)
                If disfucrt = 1 Then
                    prms.Label11.Visible = False
                    ShowModeTips = 0
                    cdisbfut = 1
                ElseIf disfucrt = 0 Then
                    prms.Label11.Visible = True
                    ShowModeTips = 1
                    cdisbfut = 0
                End If
            End If

            Dim disfucr As Integer
            If (Not plkeycr Is Nothing) Then
                disfucr = plkeycr.GetValue("DisableFeatures", -1)
                If disfucr = 1 Then
                    Call disbfu()
                    cdisbfu = 1
                End If
            End If

            Dim unsavecfgcr As Integer
            If (Not plkeycr Is Nothing) Then
                unsavecfgcr = plkeycr.GetValue("NoSaveProfile", -1)
                If unsavecfgcr = 1 Then
                    UnSaveData = 1
                    If DisbFuState = 1 Then
                        prms.Label11.Text = "由于策略设置，你的更改将不会被保存。部分功能已被禁用。"
                    Else
                        prms.Label11.Text = "由于策略设置，你的更改将不会被保存。"
                    End If
                    unsavefut = 1
                End If
            End If

            If (Not plkeycr Is Nothing) Then
                plkeycr.Close()
            End If
        Catch ex As Exception
        End Try

        Try
            Dim plkey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Policies\CJH\IBoard", True)
            Dim disfu As Integer

            If cdisbfut = 0 Then
                Dim disfut As Integer = -1
                If (Not plkey Is Nothing) Then
                    disfut = plkey.GetValue("DisableFeaturesTip", -1)
                    If disfut = 1 Then
                        prms.Label11.Visible = False
                        ShowModeTips = 0
                    ElseIf disfut = 0 Then
                        prms.Label11.Visible = True
                        ShowModeTips = 1
                    End If
                End If
            End If

            If cdisbfu = 0 Then
                If (Not plkey Is Nothing) Then
                    disfu = plkey.GetValue("DisableFeatures", -1)
                    If disfu = 1 Then
                        Call disbfu()
                    End If
                End If
            End If

            If unsavefut = 0 Then
                Dim unsavecfg As Integer
                If (Not plkey Is Nothing) Then
                    unsavecfg = plkey.GetValue("NoSaveProfile", -1)
                    If unsavecfg = 1 Then
                        UnSaveData = 1
                        If DisbFuState = 1 Then
                            prms.Label11.Text = "由于策略设置，你的更改将不会被保存。部分功能已被禁用。"
                        Else
                            prms.Label11.Text = "由于策略设置，你的更改将不会被保存。"
                        End If
                    End If
                End If
            End If
            If (Not plkey Is Nothing) Then
                plkey.Close()
            End If
        Catch ex As Exception
        End Try

        Try
            If Command().ToLower = "/nosaveprofile" Then
                If DisbFuState = 1 Then
                    prms.Label11.Text = "当前你的更改将不会被保存。部分功能已被禁用。"
                Else
                    prms.Label11.Text = "当前你的更改将不会被保存。"
                End If
                UnSaveData = 1
            End If
        Catch ex As Exception
        End Try

        If UnSaveData = 0 And DisbFuState = 0 Then
            prms.Label11.Visible = False
            ShowModeTips = 0
        End If

        Try
            AddKey("Software\CJH", "HKCU")
            AddKey("Software\CJH\IBoard", "HKCU")
            AddKey("Software\CJH\IBoard\Settings", "HKCU")
        Catch ex As Exception
        End Try
        Try

            Dim mykey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\CJH\IBoard\Settings", True)

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  是否启用UIAccess顶置读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            Dim SysBuild As Integer
            Try
                Dim regKey As Microsoft.Win32.RegistryKey
                regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                SysBuild = regKey.GetValue("CurrentBuild").ToString
                regKey.Close()
            Catch ex As Exception
                SysBuild = Environment.OSVersion.Version.Build
            End Try
            'UIAccess提权在Win7及以下版本不需要
            '在Win8.1及以上版本测试稳定运行
            '在Win8 .net framework 4.5 4.5.1测试如果使用内置管理员提权
            '使用.net获取管理员身份的方法后进行UIAccess提权100%崩溃
            '使用系统api获取管理员身份的方法后进行UIAccess提权如果存在UAC弹窗（测试的是第2档）就正常
            '如果不存在UAC弹窗（测试的是第4档）100%崩溃
            '.net framework 4.5.2环境不稳定，有概率崩溃
            '.net framework 4.6环境稳定运行
            '如果在Win8使用建议升级.net framework版本到4.6及以上
            'Windows 8 和 8.1 开机自启开始屏幕会覆盖住UAC窗口，导致开机自启UAC确认窗口最小化到任务栏，如果不确认UAC，操作程序会出现初始化Bug，建议关闭UAC提示使用
            If SysBuild > 7601 Then
                Dim aaa As Integer = 0
                If (Not mykey Is Nothing) Then
                    aaa = mykey.GetValue("GetUIAccess", -1)
                    If aaa = -1 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "GetUIAccess", 0, RegistryValueKind.DWord, "HKCU")
                        aaa = 0
                    ElseIf aaa > 1 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "GetUIAccess", 0, RegistryValueKind.DWord, "HKCU")
                        aaa = 0
                    End If
                Else
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "GetUIAccess", 0, RegistryValueKind.DWord, "HKCU")
                    aaa = 0
                End If
                If aaa = 1 Then
                    prms.CheckBox6.Checked = True
                Else
                    prms.CheckBox6.Checked = False
                End If

                ' 检测并尝试提权
                If prms.CheckBox6.Checked = True Then
                    If Not IsAdministrator() Then
                        Try
                            Dim startInfo As New ProcessStartInfo() With {
                                .UseShellExecute = True,
                                .FileName = Application.ExecutablePath,
                                .Verb = "runas"
                            }
                            Process.Start(startInfo)
                            Application.Exit()
                        Catch
                            'prms.CheckBox6.Enabled = False
                            'prms.CheckBox6.Checked = False
                        End Try
                    Else

                        Try
                            'Dim targetPath As String = Path.Combine(Application.StartupPath, "uiaccess.dll")
                            Dim targetPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "CJH.UIAccessHelper.dll")
                            If File.Exists(targetPath) Then
                                Dim HashExt As String
                                HashExt = GetFileHash(targetPath, "SHA256")
                                If System.Environment.Is64BitProcess = True Then
                                    If Not HashExt = "a35bf5aaabaa149e9975a96188f8d5d202fe7bb560507dab6a13295982cd5b44" Then
                                        File.Delete(targetPath)
                                        File.WriteAllBytes(targetPath, My.Resources.UIAccessHelper64)
                                    End If
                                Else
                                    If Not HashExt = "c133cb9cfcad5aeb608085d7eaacd1944804865ee242b2b7363f9259e28d9e0f" Then
                                        File.Delete(targetPath)
                                        File.WriteAllBytes(targetPath, My.Resources.UIAccessHelper)
                                    End If
                                End If
                            Else
                                If System.Environment.Is64BitProcess = True Then
                                    File.WriteAllBytes(targetPath, My.Resources.UIAccessHelper64)
                                Else
                                    File.WriteAllBytes(targetPath, My.Resources.UIAccessHelper)
                                End If
                            End If

                            'Dim hr As Integer = PrepareForUIAccess()
                            ''Console.WriteLine(hr)

                            'If hr <> 0 Then
                            '    Throw New System.ComponentModel.Win32Exception(hr)
                            'End If
                            If Not CheckUIAccessStatus() Then
                                ' 如果没有UIAccess权限，尝试提升
                                Dim result = PrepareForUIAccess()
                                Dim hr As Integer
                                hr = CType(result, Integer)
                                If Not result = ERROR_SUCCESS Then
                                    Throw New System.ComponentModel.Win32Exception(hr)
                                End If
                            End If
                        Catch ex As Exception
                            MsgBox("获取 UIAccess 权限失败。" & vbCrLf & ex.ToString, MsgBoxStyle.Exclamation, "警告")
                            Exit Try
                        End Try
                    End If
                End If
            Else
                prms.CheckBox6.Enabled = False
                prms.CheckBox6.Checked = False
                prms.CheckBox6.Text = "使用 UIAccess 权限顶置（当前系统不支持）"
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  图片显示模式注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            Dim myv As Integer
            If (Not mykey Is Nothing) Then
                myv = mykey.GetValue("ImageMode", -1)
                If myv > 0 And myv <= 4 Then
                    prms.ComboBox2.SelectedIndex = myv
                    PictureBox1.SizeMode = myv
                Else
                    prms.ComboBox2.SelectedIndex = 1
                    PictureBox1.SizeMode = 1
                    AddReg("Software\CJH\IBoard\Settings", "ImageMode", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
                End If
            Else
                prms.ComboBox2.SelectedIndex = 1
                PictureBox1.SizeMode = 1
                AddReg("Software\CJH\IBoard\Settings", "ImageMode", 1, Microsoft.Win32.RegistryValueKind.DWord, "HKCU")
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  是否保存位置注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////

            If (Not mykey Is Nothing) Then
                Me.SaveLoc = mykey.GetValue("SaveLocations", -1)
                If Me.SaveLoc = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "SaveLocations", 0, RegistryValueKind.DWord, "HKCU")
                    Me.SaveLoc = 0
                ElseIf Me.SaveLoc > 1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "SaveLocations", 0, RegistryValueKind.DWord, "HKCU")
                    Me.SaveLoc = 0
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "SaveLocations", 0, RegistryValueKind.DWord, "HKCU")
                Me.SaveLoc = 0
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  位置注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            'Dim a As New Point
            BootLoc.X = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15
            BootLoc.Y = 15
            If SaveLoc = 1 Then
                Dim aa As Integer
                Dim ba As Integer
                If (Not mykey Is Nothing) Then
                    aa = mykey.GetValue("ImageFormX", -1)
                    If aa = -1 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, RegistryValueKind.DWord, "HKCU")
                        aa = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15
                        BootLocErr = 1
                    ElseIf aa < -2 - Me.Width Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, RegistryValueKind.DWord, "HKCU")
                        aa = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15
                        BootLocErr = 1
                    ElseIf aa > System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width + Me.Width + 2 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, RegistryValueKind.DWord, "HKCU")
                        aa = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15
                        BootLocErr = 1
                    Else
                        BootLoc.X = aa
                    End If
                Else
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, RegistryValueKind.DWord, "HKCU")
                    aa = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15
                    BootLocErr = 1
                End If
                If (Not mykey Is Nothing) Then
                    ba = mykey.GetValue("ImageFormY", -1)
                    If ba = -1 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", 15, RegistryValueKind.DWord, "HKCU")
                        ba = 15
                        BootLocErr = 1
                    ElseIf ba < -2 - Me.Height Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", 15, RegistryValueKind.DWord, "HKCU")
                        ba = 15
                        BootLocErr = 1
                    ElseIf ba > System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height + Me.Height + 2 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", 15, RegistryValueKind.DWord, "HKCU")
                        ba = 15
                        BootLocErr = 1
                    Else
                        BootLoc.Y = ba
                    End If
                Else
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", 15, RegistryValueKind.DWord, "HKCU")
                    ba = 15
                    BootLocErr = 1
                End If

                If Not BootLocErr = 1 Then
                    Me.Location = BootLoc
                End If
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  窗口顶置注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            Dim UseTop As Integer
            If (Not mykey Is Nothing) Then
                UseTop = mykey.GetValue("AllowTopMost", -1)
                If UseTop = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "AllowTopMost", 0, RegistryValueKind.DWord, "HKCU")
                    Me.TopMost = False
                    prms.TopMost = False
                ElseIf UseTop = 0 Then
                    Me.TopMost = False
                    prms.TopMost = False
                ElseIf UseTop = 1 Then
                    Me.TopMost = True
                    prms.TopMost = True
                ElseIf UseTop > 1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "AllowTopMost", 0, RegistryValueKind.DWord, "HKCU")
                    Me.TopMost = False
                    prms.TopMost = False
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "AllowTopMost", 0, RegistryValueKind.DWord, "HKCU")
                Me.TopMost = False
                prms.TopMost = False
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  是否启用图片移动注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////

            If (Not mykey Is Nothing) Then
                Me.UseMoveV = mykey.GetValue("EnableMove", -1)
                If Me.UseMoveV = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableMove", 0, RegistryValueKind.DWord, "HKCU")
                    Me.UseMoveV = 0
                ElseIf Me.UseMoveV > 1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableMove", 0, RegistryValueKind.DWord, "HKCU")
                    Me.UseMoveV = 0
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableMove", 0, RegistryValueKind.DWord, "HKCU")
                Me.UseMoveV = 0
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  是否启用图片拖放注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            Dim myb As Integer
            If (Not mykey Is Nothing) Then
                myb = mykey.GetValue("EnableDrag", -1)
                If myb = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableDrag", 1, RegistryValueKind.DWord, "HKCU")
                    PictureBox1.AllowDrop = True
                ElseIf myb = 0 Then
                    PictureBox1.AllowDrop = False
                ElseIf myb = 1 Then
                    PictureBox1.AllowDrop = True
                ElseIf Me.UseMoveV > 1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableDrag", 1, RegistryValueKind.DWord, "HKCU")
                    PictureBox1.AllowDrop = True
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "EnableDrag", 1, RegistryValueKind.DWord, "HKCU")
                PictureBox1.AllowDrop = True
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  背景颜色注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            Dim tfntcr As Integer
            If (Not mykey Is Nothing) Then
                tfntcr = mykey.GetValue("BkgColorR", -1)
                If tfntcr = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorR", Me.BackColor.R, RegistryValueKind.DWord, "HKCU")
                    tfntcr = Me.BackColor.R
                    If tfntcr > 255 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorR", Me.BackColor.R, RegistryValueKind.DWord, "HKCU")
                        tfntcr = Me.BackColor.R
                    End If
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorR", Me.BackColor.R, RegistryValueKind.DWord, "HKCU")
                tfntcr = Me.BackColor.R
            End If

            Dim tfntcg As Integer
            If (Not mykey Is Nothing) Then
                tfntcg = mykey.GetValue("BkgColorG", -1)
                If tfntcg = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorG", Me.BackColor.G, RegistryValueKind.DWord, "HKCU")
                    tfntcg = Me.BackColor.G
                    If tfntcg > 255 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorG", Me.BackColor.G, RegistryValueKind.DWord, "HKCU")
                        tfntcg = Me.BackColor.G
                    End If
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorG", Me.BackColor.G, RegistryValueKind.DWord, "HKCU")
                tfntcg = Me.BackColor.G
            End If

            Dim tfntcb As Integer
            If (Not mykey Is Nothing) Then
                tfntcb = mykey.GetValue("BkgColorB", -1)
                If tfntcb = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorB", Me.BackColor.B, RegistryValueKind.DWord, "HKCU")
                    tfntcb = Me.BackColor.B
                    If tfntcb > 255 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorB", Me.BackColor.B, RegistryValueKind.DWord, "HKCU")
                        tfntcb = Me.BackColor.B
                    End If
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "BkgColorB", Me.BackColor.B, RegistryValueKind.DWord, "HKCU")
                tfntcb = Me.BackColor.B
            End If

            Me.BackColor = Color.FromArgb(tfntcr, tfntcg, tfntcb)
            prms.ColorDialog1.Color = Me.BackColor

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  是否加载上一次图片注册表读取
            '//
            '////////////////////////////////////////////////////////////////////////////////////

            If (Not mykey Is Nothing) Then
                Me.LoadLastImage = mykey.GetValue("LoadLastImage", -1)
                If Me.LoadLastImage = -1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "LoadLastImage", 0, RegistryValueKind.DWord, "HKCU")
                    Me.LoadLastImage = 1
                ElseIf Me.LoadLastImage > 1 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "LoadLastImage", 0, RegistryValueKind.DWord, "HKCU")
                    Me.LoadLastImage = 0
                End If
            Else
                RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "LoadLastImage", 0, RegistryValueKind.DWord, "HKCU")
                Me.LoadLastImage = 0
            End If

            '////////////////////////////////////////////////////////////////////////////////////
            '//
            '//  读取上一次打开图片的路径
            '//
            '////////////////////////////////////////////////////////////////////////////////////
            If Me.LoadLastImage = 1 Then
                If (Not mykey Is Nothing) Then
                    prms.TextBox1.Text = mykey.GetValue("ImagePath", "-1")
                    If System.IO.File.Exists(prms.TextBox1.Text) = False Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImagePath", "", RegistryValueKind.String, "HKCU")
                        prms.TextBox1.Text = ""
                    End If
                    If prms.TextBox1.Text = "-1" Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImagePath", "", RegistryValueKind.String, "HKCU")
                        prms.TextBox1.Text = ""
                    End If
                Else
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImagePath", "", RegistryValueKind.String, "HKCU")
                    prms.TextBox1.Text = ""
                End If
            Else
                prms.TextBox1.Text = ""
            End If
        Catch ex As Exception
        End Try



        '------------------------------------------------------------------------------
        pst = 0
        Hsate = 0
        ImgAutoLoad = 0
        If ImgAutoLoad <> 1 Then
            If System.IO.File.Exists(Application.StartupPath & "\ShowImage.png") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.png")
            ElseIf System.IO.File.Exists(Application.StartupPath & "\ShowImage.jpg") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.jpg")
            ElseIf System.IO.File.Exists(Application.StartupPath & "\ShowImage.bmp") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.bmp")
            ElseIf System.IO.File.Exists(Application.StartupPath & "\ShowImage.tiff") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.tiff")
            ElseIf System.IO.File.Exists(Application.StartupPath & "\ShowImage.ico") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.ico")
            ElseIf System.IO.File.Exists(Application.StartupPath & "\ShowImage.gif") = True Then
                LoadImage(Application.StartupPath & "\ShowImage.gif")
            ElseIf LoadLastImage = 1 Then
                If System.IO.File.Exists(prms.TextBox1.Text) = True Then
                    LoadImage(prms.TextBox1.Text)
                End If
            End If
        End If
        If ImgAutoLoad = 1 Then
            pst = 1
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            FlowLayoutPanel1.Visible = False
            'Me.Location = New Point(SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, 15)
            If SaveLoc = 0 Then
                BootLoc = New Point(SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, 15)
                Me.Location = BootLoc
            ElseIf SaveLoc = 1 And BootLocErr = 1 Then
                BootLoc = New Point(SystemInformation.PrimaryMonitorSize.Width - Me.Width - 15, 15)
                Me.Location = BootLoc
                If UnSaveData = 0 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", Me.Location.X, RegistryValueKind.DWord, "HKCU")
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", Me.Location.Y, RegistryValueKind.DWord, "HKCU")
                End If
            End If
        Else
            If BootLocErr = 1 Then
                If SaveLoc = 1 Then
                    BootLoc = New Point((SystemInformation.PrimaryMonitorSize.Width - Me.Width) / 2, (SystemInformation.PrimaryMonitorSize.Height - Me.Height) / 2)
                    Me.Location = BootLoc
                    If UnSaveData = 0 Then
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormX", Me.Location.X, RegistryValueKind.DWord, "HKCU")
                        RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImageFormY", Me.Location.Y, RegistryValueKind.DWord, "HKCU")
                    End If
                Else
                    BootLoc = New Point((SystemInformation.PrimaryMonitorSize.Width - Me.Width) / 2, (SystemInformation.PrimaryMonitorSize.Height - Me.Height) / 2)
                    Me.Location = BootLoc
                End If
            End If
        End If
        'FixLoc = 0
        'Timer1.Enabled = True
    End Sub

    Sub LoadImage(ByVal pfile1 As String)
        If System.IO.File.Exists(pfile1) = True Then
            Try
                pfile = pfile1
                PictureBox1.Load(pfile)
                prms.TextBox1.Text = pfile1
                NotifyIcon1.Text = "图片展示小工具 - 已打开图片"
                CFileState.Text = "已打开文件：" & pfile
                ImgAutoLoad = 1
                If UnSaveData = 0 Then
                    RegKeyModule.AddReg("Software\CJH\IBoard\Settings", "ImagePath", pfile, RegistryValueKind.String, "HKCU")
                End If
            Catch ex As Exception
                pfile = pfile1
                prms.TextBox1.Text = ""
                NotifyIcon1.Text = "图片展示小工具 - 未打开图片"
                CFileState.Text = "未打开文件。"
                ImgAutoLoad = 0
                MessageBox.Show("图片加载失败。", "图片展示小工具 - 错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        If ImgAutoLoad = 1 Then
            'Me.Text = ""

            'If PictureBox1.Image.Height <> 0 Then
            '    If PictureBox1.Image.Height <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height Then
            '        Me.Height = PictureBox1.Image.Height
            '    ElseIf PictureBox1.Image.Height / 2 <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height Then
            '        Me.Height = PictureBox1.Image.Height / 2
            '    End If
            'End If

            'If PictureBox1.Image.Width <> 0 Then
            '    If PictureBox1.Image.Width <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width Then
            '        Me.Width = PictureBox1.Image.Width
            '    ElseIf PictureBox1.Image.Width / 2 <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width Then
            '        Me.Width = PictureBox1.Image.Width / 2
            '    End If
            'End If
            If Me.PictureBox1.Image.Height <> 0 Or Me.PictureBox1.Image.Width <> 0 Then
                'If Me.PictureBox1.Image.Height <> 0 Or Me.PictureBox1.Image.Width <> 0 Then
                If Me.PictureBox1.Image.Height <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height And Me.PictureBox1.Image.Width <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width Then
                    Me.Height = Me.PictureBox1.Image.Height
                    Me.Width = Me.PictureBox1.Image.Width
                    'ElseIf Me.PictureBox1.Image.Height / 2 <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height Or Me.PictureBox1.Image.Width / 2 <= System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width Then
                    '    Me.Height = Me.PictureBox1.Image.Height / 2
                    '    Me.Width = Me.PictureBox1.Image.Width / 2
                ElseIf Me.PictureBox1.Image.Height > System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height Or Me.PictureBox1.Image.Width > System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width Then
                    Dim a As Integer
                    Dim b As Integer
                    a = Int(Me.PictureBox1.Image.Height / System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height)
                    b = Int(Me.PictureBox1.Image.Width / System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width)
                    If a > 1 And b <= 1 Then
                        If a > 0 Then
                            Me.Height = (Me.PictureBox1.Image.Height / a) * 0.5 '+ Me.FlowLayoutPanel1.Height
                            Me.Width = (Me.PictureBox1.Image.Width / a) * 0.5
                        Else
                            Me.Height = Me.PictureBox1.Image.Height * 0.5
                            Me.Width = Me.PictureBox1.Image.Width * 0.5
                        End If
                    ElseIf a <= 1 And b > 1 Then
                        If b > 0 Then
                            Me.Height = (Me.PictureBox1.Image.Height / b) * 0.5 '+ Me.FlowLayoutPanel1.Height
                            Me.Width = (Me.PictureBox1.Image.Width / b) * 0.5
                        Else
                            Me.Height = Me.PictureBox1.Image.Height * 0.5
                            Me.Width = Me.PictureBox1.Image.Width * 0.5
                        End If
                    ElseIf a > 1 And b > 1 Then
                        If a > b Then
                            If a > 0 Then
                                Me.Height = (Me.PictureBox1.Image.Height / b) * 0.5 '+ Me.FlowLayoutPanel1.Height
                                Me.Width = (Me.PictureBox1.Image.Width / b) * 0.5
                            Else
                                Me.Height = Me.PictureBox1.Image.Height * 0.5
                                Me.Width = Me.PictureBox1.Image.Width * 0.5
                            End If
                        Else
                            If b > 0 Then
                                Me.Height = (Me.PictureBox1.Image.Height / a) * 0.5 '+ Me.FlowLayoutPanel1.Height
                                Me.Width = (Me.PictureBox1.Image.Width / a) * 0.5
                            Else
                                Me.Height = Me.PictureBox1.Image.Height * 0.5
                                Me.Width = Me.PictureBox1.Image.Width * 0.5
                            End If
                        End If
                    Else
                        Me.Height = Me.PictureBox1.Image.Height * 0.5
                        Me.Width = Me.PictureBox1.Image.Width * 0.5
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub pfrm_FormClosing(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        NotifyIcon1.Visible = False
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Hsate = 0
        CHide.Text = "隐藏(&H)"
        Me.Show()
    End Sub

    Private Sub CHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHide.Click
        If Hsate = 0 Then
            Me.Hsate = 1
            CHide.Text = "显示(&S)"
            prms.Close()
            If pfile <> "" Then
                NotifyIcon1.ShowBalloonTip(7000, "图片展示小工具 - 隐藏到托盘", "当前打开图片：" & pfile & "。" & vbCrLf & "当前已隐藏到系统托盘，双击托盘图标重新显示。", ToolTipIcon.Info)
            Else
                NotifyIcon1.ShowBalloonTip(7000, "图片展示小工具 - 隐藏到托盘", "当前未打开图片。" & vbCrLf & "当前已隐藏到系统托盘，双击托盘图标重新显示。", ToolTipIcon.Info)
            End If
            Me.Hide()
        Else
            Me.Hsate = 0
            CHide.Text = "隐藏(&H)"
            Me.Show()
        End If
    End Sub

    'Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
    '    'If SaveLoc = 0 Then
    '    If FixLoc = 0 Then
    '        If Me.Location.X <> BootLoc.X Or Me.Location.Y <> BootLoc.Y Then
    '            Me.Location = BootLoc
    '        End If
    '    Else
    '        MsgBox(1)
    '        Timer1.Enabled = False
    '    End If
    '    'End If
    'End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Me.TopMost = True Then
            If Me.Visible = True Then
                If NeedStillTopMost = 1 Then
                    SetWindowPos(Me.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS)
                End If
            End If
        End If
    End Sub

    <DllImport("user32.dll")>
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
    End Function
    Const HWND_TOPMOST = -1
    Const SWP_NOSIZE As UInteger = &H1
    Const SWP_NOMOVE As UInteger = &H2
    Const TOPMOST_FLAGS As UInteger = SWP_NOMOVE Or SWP_NOSIZE

End Class