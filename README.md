# Live2D Viewer

这个项目非常古老，只支持 Cubism 2 的 .moc 模型文件，例如[这里的模型](https://github.com/doitian/live2DModel) (**注意需要调整目录结构**)。
新的 .moc3 可以使用[官方 Viewer](https://docs.live2d.com/en/cubism-editor-manual/cubism3-viewer-for-ow/)

编译好的包下载：

- [macOS](https://github.com/doitian/live2dviewer/releases/download/latest/live2dviewer-macOS.zip)
- [Windows](https://github.com/doitian/live2dviewer/releases/download/latest/live2dviewer-Windows.zip)

相比官方的 air 版 Viewer

- 不需要额外安装 Adobe AIR
- 可以查看一个根目录下所有子目录中的模型，方便批量查看
- 自动扫描 motion 和 expression 文件

操作

- 先选择根目录加载 Live2D 模型，可以是单独一个模型的目录，也可以是包含多个模型的根目录，每个模型是一个子目录
- 单击或者空格播放下一个动作，可以在设置中播放指定动作并设置循环。有时候按钮有焦点空格会变成点击按钮，在空白处先单击取消掉焦点。
- 键盘左右或者用工具栏浏览上一个/下一个模型，可以在设置中使用目录名快速定位模型
- 如果有表情文件，可以在设置中使用表情
- 可以在设置中设置各个部件的透明值
- 拖拽移到模型，滚轮缩放

对于[这里的模型](https://github.com/doitian/live2DModel)，下面是两个示例的目录结构。注意原来的 moc 目录中的文件都往上移动了一层。在 [#9](https://github.com/doitian/live2dviewer/issues/9) 中上传了示例用的模型文件包。

```
├───live2d-widget-model-miku
│   │   miku.moc
│   │
│   ├───miku.2048
│   │       texture_00.png
│   │
│   └───mtn
│           miku_idle_01.mtn
│           miku_m_01.mtn
│           miku_m_02.mtn
│           miku_m_03.mtn
│           miku_m_04.mtn
│           miku_m_05.mtn
│           miku_m_06.mtn
│           miku_shake_01.mtn
│
└───live2d-widget-model-tsumiki
    │   tsumiki.moc
    │   tsumiki.model.json
    │   tsumiki.physics.json
    │
    ├───exp
    │       F01.exp.json
    │       F02.exp.json
    │       F03.exp.json
    │       F04.exp.json
    │       F05.exp.json
    │       F06.exp.json
    │       F07.exp.json
    │       F08.exp.json
    │       F09.exp.json
    │       F10.exp.json
    │
    ├───mtn
    │       P01.mtn
    │       tsumiki_idle_01.mtn
    │       tsumiki_m_01.mtn
    │       tsumiki_m_01_df.mtn
    │       tsumiki_m_02.mtn
    │       tsumiki_m_03.mtn
    │       tsumiki_m_04.mtn
    │       tsumiki_m_05.mtn
    │       tsumiki_m_06.mtn
    │       tsumiki_m_07.mtn
    │       tsumiki_m_08.mtn
    │       tsumiki_m_09.mtn
    │       tsumiki_m_10.mtn
    │       tsumiki_m_11.mtn
    │       tsumiki_m_12.mtn
    │       tsumiki_m_13.mtn
    │       tsumiki_m_14.mtn
    │       tsumiki_m_15.mtn
    │       tsumiki_m_16.mtn
    │       tsumiki_m_17.mtn
    │       tsumiki_m_18.mtn
    │       tsumiki_m_19.mtn
    │       tsumiki_m_20.mtn
    │       tsumiki_m_21.mtn
    │       tsumiki_m_22.mtn
    │       tsumiki_m_23.mtn
    │       tsumiki_m_24.mtn
    │
    └───tsumiki.2048
            texture_00.png
            texture_01.png
```

