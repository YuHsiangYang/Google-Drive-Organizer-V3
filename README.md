# Google Drive Organizer V3

## Features

1. Reads the images and JSON file to fix the photo take time of the image.
2. Use the GPS data from JSON file to correct the missing data in the image.
3. Lets the user preview and remove unwanted images before merging.

## Overview

This program is intended to solve the issues with the downloaded images. When the photos are exported from Google Photo, the metadata are being removed. This causes the problem for the other platforms to arrange the images. Since there is no photo taken time given in the image, the software can only use the date downloaded from google photo to organize the images. This method of arrangement lead to the problem in which images are not arranged in proper order. Therefore, my program is to solve this issue by using the JSON file to restore the missing information.

## Main components overview

**Progress bar —** Used for progress visualization.

****Search control —**** The general component used to search for images by file name and photo taken time.

****************************************Stage navigation —**************************************** Used for navigating throughout the process. This is the control for the user to proceed to next page or previous page.

**************************************************Display types (Folder) —************************************************** This folder contains the components for two different display types, icon and List.

********Image content viewers —******** Contains the different views of Exif data.

******************Exif_ContentViewer —****************** The view for Exif data retrieved from JSON file.

************Image_DataViewer —************ View the data contained in the image.

**********Json_ContentViewer —********** View the information contained in the JSON file.

******************************************Sort controller —****************************************** This controller controls the types (file name or photo taken time) of sort and manner (ascending or descending).

********************DeleteButton —******************** To exclude a particular/collection of images from the merging process.
