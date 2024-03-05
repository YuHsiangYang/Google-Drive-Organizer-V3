其他語言: [English](ReadME-en.md)

# Google Drive Organizer V3

# 概觀

這個程式旨在解決從 Google 相片下載的圖片問題。當圖片從 Google 相片匯出時，會移除相片拍攝資訊。這導致其他平台例如(Synology Photos)難以排列圖片。由於圖片中沒有拍攝時間的資訊，軟體只能使用從 Google 相片下載的資料來整理圖片。這種排列方式導致了圖片無法按照正確的順序排列的問題。因此，我的程式通過使用 JSON 檔案來恢復遺失的資訊來解決此問題。

# 主要組件概觀

**進度條 —** 用於進度視覺化。

**搜尋控制 —** 用於按文件名和拍攝時間搜尋圖片的組件。

**階段導覽 —** 用於在整個過程中導覽。用於控制下一頁或上一頁。

**顯示類型（資料夾） —** 此資料夾包含兩種不同顯示類型（圖示和列表）的組件。

**圖片內容查看器 —** 包含 Exif 數據的不同視圖。

**Exif_ContentViewer —** 用於檢視JSON檔案資料。

**Image_DataViewer —** 查看圖片中包含的資料。

**Json_ContentViewer —** 查看 JSON 檔案中包含的資訊。

**排序控制器 —** 此控制器控制排序類型（文件名稱或拍攝時間）和方式（由新至舊或由舊至新）。

**刪除按鈕 —** 從合併過程中排除特定/一組圖片。

# 專案大綱
## 功能：

- 匯入
    - 從不同目錄搜尋 JSON
    - 手動匯入圖片
    - 從目錄匯入圖片
    - 手動選擇 JSON 檔案
- 編輯與處理
    - 小圖示中查看圖片
    - 將 JSON 中的資訊套用至圖片
    - 編輯圖片的 **exif** 資訊
        - 使用 Google 地圖的 GPS 座標
        - 拍攝時間
- 導覽
    - 選擇多個圖片以從列表中刪除
        - 每張圖片應該有一個勾選框
        - 勾選後，應該出現垃圾桶圖示
    - 按文件名和拍攝時間排序圖片
        - 使用下拉框選擇類型；
        - 另一個選擇方式（升序或降序）
    - 按文件名和拍攝時間搜索圖片。
- 日誌
    - 建立應用資訊的日誌檔案

## 階段

### 建立應用和修改 Exif 數據的 API

1. Python 腳本
    
    JSON 映射文件的格式：
    
    ```json
    {
    	"<EXIF 標籤（exiv2 標籤）>": [<path>, <path>], // 路徑到另一個文件
    }
    ```
    
    用於修改圖片 exif 數據的 JSON 檔案的格式：
    
    ```json
    {
    	"<EXIF 標籤>": <指定格式的資料>
    }
    ```
    
    [Exiv2 - 圖片元數據庫和工具](https://exiv2.org/tags.html)
    
2. 建立可執行檔
    - 使用 [PyInstaller](https://pyinstaller.org/en/stable/) 程式庫

### 建立自定義組件

1. 使用百分比設置角半徑的按鈕
2. 預覽側邊欄
3. 帶有動畫和調用方法的階段監控器
4. 帶有動畫和調用方法的階段監控器
5. 垃圾桶
6. 圖片圖示
7. 搜尋欄
8. 首頁
9. 用於顯示導入資料夾的控制項
10. 應用圖示

### 主應用程式

- 選擇 json
- 選擇圖片
- 搜尋
    - 按文件名稱
    - 按拍攝時間
- 不同頁面
    - 選擇 json 的頁面
    - 選擇圖片的頁面
    - 選擇目的地的頁面
    - 顯示匹配圖片的頁面
        - 顯示不匹配圖片的功能
    - 監視複製和套用圖片進度的頁面
        - 調用腳本

### 單元測試
- 從示例中
- 實際測試

此專案大綱已從我的 Notion 記事本移動。[程式設計](https://yuhsiangnote.notion.site/Program-Design-23abb37d5e8e40369b7f80a4edff40e6?pvs=4) 或 https://yuhsiangnote.notion.site/Program-Design-23abb37d5e8e40369b7f80a4edff40e6?pvs=4


## 示範影片:
[Demonstration](/Program-demonstration.mp4)

# 參考資料
### 我的應用程式中使用的圖案：

<a href="https://www.flaticon.com/free-icons/image" title="image icons">Image icons created by Pixel perfect - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/duplicate" title="duplicate icons">Duplicate icons created by Phoenix Group - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/search" title="search icons">Search icons created by Catalin Fertu - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/sorting" title="sorting icons">Sorting icons created by yaicon - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/delete" title="delete icons">Delete icons created by Ilham Fitrotul Hayat - Flaticon</a>

### 程式碼參考：

特別感謝 Stack Overflow 社群