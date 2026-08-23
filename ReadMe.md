<h1 align="center">
  <a href="https://github.com/cjhdevact/IBoard">IBoard - 图片展示小工具</a>
</h1>

## 关于本项目

这是一个可以将一张或多张图片展示在屏幕上的工具。本软件可以安装在大屏上（例如教学大屏），也可以安装在普通电脑上。

## 功能

本程序支持的功能有：

- [x] 展示图片（基础功能）
- [x] 一系列可以自定义的设置，支持通过将图片拖放到程序窗口方便地打开图片
- [x] 可以自动启动，并且自动加载上一次打开的图片（或者程序启动目录的`ShowImage`名称图片）
- [x] 可以通过双击图片隐藏/显示窗口框架
- [x] 支持通过组策略配置策略
- [x] UIAccess 级别顶置（需要 Windows 8 以上版本）
- [x] 适配DPI缩放（适配高分屏）

补充说明：

- UIAccess 级别顶置功能需要 Windows 8 以上版本才可使用，Windows 7 及以下版本无需此功能支持。UIAccess 需要以管理员身份运行，本程序内部自动提权的功能在 Windows 8 默认自带的 .NET Framework 4.5 下可能会导致程序崩溃，建议升级 .NET Framework 4.6 及以上版本，或者手动以管理员身份运行。Windows 8 和 8.1 开机自启开始屏幕会覆盖住UAC窗口，导致开机自启UAC确认窗口最小化到任务栏，如果不确认UAC，操作程序会出现初始化Bug，建议关闭UAC提示使用。

## 下载

请访问[发布页](https://github.com/cjhdevact/IBoard/releases)下载最新版本的可执行文件或源代码。

早期版本亦可在发布页中找到。

## 数字签名

本项目发布的二进制文件使用自签名证书进行代码签名，以确保文件的完整性和来源可信。

证书信息：
```
Name: CJH Root Certificate
Create: ‎2024‎年‎12‎月‎27‎日 20:42:16
Expires: ‎2150‎年‎12‎月‎31‎日 0:00:00
MD5: 0bc507db70947e57ddd81bec63b581d9
SHA256: d2d67c8ebea3cc954c7ee0e94f5f45537dde7709053ca9e89f352fda60283
Key fingerprint (SHA1): 73b80a8d0ba3f662b575f2fc0b78612469e22e59
KeyID: d929e453f645017190dac5001a736a4d
Certificate SerialNumber: dbde77418068d5a34b2064626a12ecde
Key Type: md5RSA
```

如需验证，可从[这里](Src/IBoard/files/rootcert.cer)获取根证书文件。

## 程序截图

### 主程序界面

![主程序界面](Assets/MainUI.png)

### 设置界面

![设置界面](Assets/SettingUI.png)

## 相关项目

[UsefulControl](https://github.com/cjhdevact/UsefulControl) - 实用小工具的整合项目（包括本项目的功能）

## 引用

本项目使用了以下第三方库：

[UIAccessHelper](https://github.com/cjhdevact/UIAccessHelper)

## 开源说明

本项目基于 `GPLv3` 许可证开源，详情请参阅 [License](License) 文件。 

您可以在遵守许可证的前提下自由使用、修改和分发本软件。