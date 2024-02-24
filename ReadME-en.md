other language: [中文](README.md)


# Google Drive Organizer V3

# Overview

This program is intended to solve the issues with the downloaded images. When the photos are exported from Google Photo, the metadata are being removed. This causes a problem for the other platforms to arrange the images. Since there is no photo taken time given in the image, the software can only use the data downloaded from Google Photos to organize the images. This method of arrangement leads to the problem in which images are not arranged in proper order. Therefore, my program is to solve this issue by using the JSON file to restore the missing information.

# Main components overview

**Progress bar —** Used for progress visualization.

**Search control —** The general component used to search for images by file name and photo taken time.

**Stage navigation —** Used for navigating throughout the process. This is the control for the user to proceed to next page or previous page.

**Display types (Folder) —** This folder contains the components for two different display types, icon and List.

**Image content viewers —** Contains the different views of Exif data.

**Exif_ContentViewer —** The view for Exif data retrieved from JSON file.

**Image_DataViewer —** View the data contained in the image.

**Json_ContentViewer —** View the information contained in the JSON file.

**Sort controller —** This controller controls the types (file name or photo taken time) of sort and manner (ascending or descending).

**DeleteButton —** To exclude a particular/collection of images from the merging process.

# Project outline
## Features:

- Importing
    - Retrieving json from a separate directory
    - import image manually
    - import images in a directory
    - Select the json file manually
- Editing & Processing
    - View the images in small icon
    - Apply the information from json to both vide and image files
    - Edit the **exif** info of the image
        - GPS coordination using google map
        - Photo taken time
- Navigating
    - Select multiple images to remove from the list
        - There should be a check box for each image
        - When checked, a trash bin should appear
    - sort images by file name and photo taken time
        - A combo box used to select the type;
        - and another select the manner (ascending or descending)
    - Search images by filename and photo taken time.
- Logging
    - Create a log file for the applying of info

## Stages

### Build the API for applying & modifying Exif data

1. Python script
    
    Format of the json map file:
    
    ```json
    {
    	"<EXIF tag (exiv2 tags)>": [<path>, <path>], //path to another document
    }
    ```
    
    Format for the json file that can be used for modifying exif data of an image:
    
    ```json
    {
    	"<EXIF tag>": <data in specified format>
    }
    ```
    
    [Exiv2 - Image metadata library and tools](https://exiv2.org/tags.html)
    
2. Create executables
    - Using the [PyInstaller](https://pyinstaller.org/en/stable/) library

### Build the custom components

1. Button with corner radius using percentage
2. Preview sidebar
3. Stage monitor with animation and calling method
4. Stage monitor with animation and calling method
5. Trash bin
6. Image icon
7. Search bar
8. Home page
9. Control for displaying imported folders
10. App Icon

### Main application

- Select json
- Select images & videos
- Search
    - By file name
    - by photo taken time
- Different stages
    - Page to select json
    - Page to select images
    - Page to select destination
    - Page to display matched images
        - Feature to display unmatched images
    - Page to monitor the progress of copying and applying images
        - Calling the script

### Unit tests
- From sample
- Actual testing

This project outline was moved from my notion notebook. Link: [Program Design](https://yuhsiangnote.notion.site/Program-Design-23abb37d5e8e40369b7f80a4edff40e6?pvs=4) or https://yuhsiangnote.notion.site/Program-Design-23abb37d5e8e40369b7f80a4edff40e6?pvs=4

# References
### icons used in my app:

<a href="https://www.flaticon.com/free-icons/image" title="image icons">Image icons created by Pixel perfect - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/duplicate" title="duplicate icons">Duplicate icons created by Phoenix Group - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/search" title="search icons">Search icons created by Catalin Fertu - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/sorting" title="sorting icons">Sorting icons created by yaicon - Flaticon</a>

<a href="https://www.flaticon.com/free-icons/delete" title="delete icons">Delete icons created by Ilham Fitrotul Hayat - Flaticon</a>

### Code references:

Special thanks to the Stack Overflow community