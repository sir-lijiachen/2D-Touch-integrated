# 2D-Touch-integrated
是一个平面设计，触摸一体机

## Scene 1   待机事件
**功能：如果时间大于300（timer >= 300），则启动待机时间，并暂停计时，直到再次点击屏幕。** 

将StandbyTime、Main 挂载上，并在Main中EventManager_OnStandbyHandler配置待机状态。  
- EventManager用于存放事件.
- StandbyTime用于计时.
- Main用于整体把控.

## Scene 2 用list存储信息，并读取
**功能：将所需数据写在List里，直接调用。**  
**调用数据**
```C#
string action= ImageConfig.imgAtbArray[0, 1][0].action;
```

将ImageConfig挂载上，并放在前列。  
- ImageAttribute是一个自定义的类.  
- ImageConfig用于创建list，并将数据存放进去.
- ContentPage2调用数据
  
## Scene 3 用Json存储信息，并读取
**功能：将所需数据写在Json里，放在StreamingAssets，通过代发访问放在StreamingAssets后调用。**  
**配置：安装LitJson。**  
**推荐：将EXCEL转换为Json格式。**  
**调用数据**
```C#
List<ReadJSONAttribute> currentAttributes = ReadJSON.imgAtbArray.FindAll(attr => attr.button == "");
```

将Json放在StreamingAssets里（这个文件可以在打包后访问修改）。
- ReadJSONAttribute是一个自定义的类
- ReadJSON用于创建list，并将Json里的数据存放进去
- ContentPage3调用数据

## Scene 4 左右滑动进行翻页
**功能：通过鼠标滑动进行翻页操作。非Scroll View操作，而是Image操作。**  

- ContentPage4前半段是为翻页操作，后面LoadContent用于创建，并加载图片操作.

## Scene 5 动画执行
**功能1：利用Tweening，进行图片的平移灯操作**
**功能2：利用Tweening，进行数字增长操作**  
**配置：安装DOTween。**  

- ImageExecute为图片动画操作
- TextExecute为数字增长操作。往StartValueAnimation里传值，数值会增长到目标值后会停止

## Scene 6 预加载StreamingAssets里的图片和视频
**功能1：加载StreamingAssets图片**
**功能2：加载StreamingAssets视频**
**调用图片数据：** ContentPage7使用main7中的方法来获取图片数据（视频没办法预加载）
```C#
public Main7 main7;
// 调用主程序中的 GetImagesForCountry 方法，获取相关路径的图片
pakistanImages = main7.GetImagesForCountry(path);
```

**调用视频数据：** 通过路径直接获取视频  
```C#
videoPlayer.url = videoPath; // 设置视频路径
videoPlayer.Pause(); // 播放视频
```

**区分视频和图片数据**  
```C#
// 支持的图片扩展名列表
private readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
// 支持的视频扩展名列表
private readonly string[] videoExtensions = { ".mp4", ".avi", ".mov" };
// 存储所有媒体文件路径（图片或视频）
private List<string> mediaPaths = new List<string>(); 
    public void Click_Path(string path)
    {
        string mediaDir = Path.Combine(Application.streamingAssetsPath, $"{path}");
        mediaPaths = GetFilesByExtension(mediaDir, imageExtensions.Concat(videoExtensions).ToArray());
    }

    /// <summary>
    /// 获取指定目录下符合指定扩展名的所有媒体文件路径。
    /// </summary>
    private List<string> GetFilesByExtension(string directory, string[] extensions)
    {
        List<string> matchingFiles = new List<string>();

        if (Directory.Exists(directory))
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                foreach (string ext in extensions)
                {
                    if (file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingFiles.Add(file);
                    }
                }
            }
        }
        return matchingFiles;
    }

    /// <summary>
    /// 显示当前选中的媒体内容（图片或视频）。
    /// </summary>
    private void DisplayMedia(string mediaPath)
    {
        string extension = Path.GetExtension(mediaPath).ToLower(); // 获取媒体文件扩展名

        if (Array.Exists(imageExtensions, ext => ext == extension)) // 如果是图片类型
        {
        }
        if (Array.Exists(videoExtensions, ext => ext == extension)) // 如果是图片类型
        {
        }
        UpdateUI(mediaPath); // 每次加载媒体后更新UI
    }
```

**图片和视频在一块，使用媒体内容来进行操作** 该操作会使最后一页的下一页跑到第一页。如果不想这样，可以限制按钮，当第一个后最后一个媒体内容时，按钮不显示。
```C#
    private List<string> mediaPaths = new List<string>(); // 存储所有媒体文件路径（图片或视频）
    private int currentMediaIndex = -1; // 当前正在显示的媒体索引

    /// <summary>
    /// 显示下一个媒体内容。
    /// </summary>
    private void ShowNextMedia()
    {
        if (mediaPaths.Count == 0) return; // 如果媒体路径为空，返回

        currentMediaIndex = (currentMediaIndex + 1) % mediaPaths.Count; // 循环切换到下一个媒体
        DisplayMedia(mediaPaths[currentMediaIndex]); // 加载当前媒体内容

        // 强制更新按钮显示状态
        UpdateButtonsVisibility();
    }

    /// <summary>
    /// 显示上一个媒体内容。
    /// </summary>
    private void ShowPreviousMedia()
    {
        if (mediaPaths.Count == 0) return; // 如果媒体路径为空，返回

        currentMediaIndex = (currentMediaIndex - 1 + mediaPaths.Count) % mediaPaths.Count; // 循环切换到上一个媒体
        DisplayMedia(mediaPaths[currentMediaIndex]); // 加载当前媒体内容

        // 强制更新按钮显示状态
        UpdateButtonsVisibility();
    }

    /// <summary>
    /// 更新按钮的可见性或状态
    /// </summary>
    private void UpdateButtonsVisibility()
    {
        if (currentMediaIndex == 0)
        {
            // 第一项时，隐藏“上一项”按钮
            btn_left.gameObject.SetActive(false);
        }
        else
        {
            // 否则显示“上一项”按钮
            btn_left.gameObject.SetActive(true);
        }

        if (currentMediaIndex == mediaPaths.Count - 1)
        {
            // 最后一项时，隐藏“下一项”按钮
            btn_right.gameObject.SetActive(false);
        }
        else
        {
            // 否则显示“下一项”按钮
            btn_right.gameObject.SetActive(true);
        }
    }
```
