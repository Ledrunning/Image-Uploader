# Image-Uploader 

This repository contains an image uploader application developed using C# and .NET.
The application allows users to upload images to the server and view them on a desktop application. 
The server-side written on C# using **.NET Core 2.0** in 
2017 but updated in 2023 without migration to the latest version of .NET only for described my knowledge of previous .NET technologies.
The repository also contains two versions of desktop applications:
1- Simple WPF-client application called: **ImageUploader.DesktopClient**
2 - WPF application with more functionality and modern design: **ImageUploader.ModernDesktopClient**

## Features

- Upload images to the server.
- View uploaded images on a desktop application.
- Delete uploaded images.
- Update image and image details 

## UI layout

 ![](imageUploader.gif)

## Getting Started
To get started with the Image Uploader, follow these steps:

Clone the repository:

`git clone https://github.com/Ledrunning/Image-Uploader.git`

Open the project in Visual Studio 2022.

Build the solution to restore the required NuGet packages.

Configure the database connection string in the appsettings.json file. Modify the following section to match your database setup:

`"ConnectionStrings": {
  "DefaultConnection": "YOUR_DATABASE_CONNECTION_STRING"
}`

## Run the application.

- Uploading Images

To upload an image, follow these steps:

Click on the "Open File" button.
Select the image file you want to upload from your local machine.
Click the "Upload" button to start the upload process.

- Viewing Images

After uploading an image, it will be displayed on the image view page. You can select a row in Datagrid and click to view it on the left side.

- Deleting Images

To delete an uploaded image, follow these steps:

Hover over the image you want to delete.

Put the Id

Click on the delete button.

## Contributing

Contributions to the Image Uploader are welcome. If you encounter any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.

## License

The Image Uploader is licensed under the MIT License. Feel free to modify and distribute the code as per the terms of the license.

