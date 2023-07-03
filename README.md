# Welcome to the UCAB GO Mobile App! 🚗🎓

This mobile application is designed to work with the UCAB GO API, a ride-sharing platform for the UCAB community. With UCAB GO, students, faculty, and staff can easily request rides to and from campus, making transportation more convenient and accessible. This repository contains the code for the mobile application, which is developed using .NET MAUI with .NET 7.

## Technologies Used

- .NET MAUI
- .NET 7
- Google Maps API

## Quick Start

1. Clone the repository to your local machine.
2. Navigate to the cloned repository.
3. Create a `data.json` file inside the `Raw` folder in `Resources`. This file should contain the following environment variables:

```json
{
  "ApiUrl": "<Your API URL>",
  "GoogleMapsApiKey": "<Your Google Maps API Key>"
}
```

4. Add your Google Maps API Key to your Android manifest file. You can do this by adding the following line inside the `<application>` tag:

```xml
<meta-data
    android:name="com.google.android.geo.API_KEY"
    android:value="<Your Google Maps API Key>" />
```

5. Open the solution in Visual Studio and run the application.

Please note that you need to replace `<Your API URL>` and `<Your Google Maps API Key>` with your actual API URL and Google Maps API Key.

Enjoy using the UCAB GO Mobile App!
