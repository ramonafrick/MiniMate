﻿namespace MiniMate.Weather.Models
{
    public record WeatherData(CurrentWeather Current, CurrentUnits? Units)
    {
        public string Temperature => $"{Current.Temperature:F1}{Units?.Temperature ?? "°C"}";
        public string ApparentTemperature => $"{Current.ApparentTemperature:F1}{Units?.ApparentTemperature ?? "°C"}";
        public string Humidity => $"{Current.RelativeHumidity}{Units?.RelativeHumidity ?? "%"}";
        public string Precipitation => $"{Current.Precipitation:F1}{Units?.Precipitation ?? "mm"}";
        public string Rain => $"{Current.Rain:F1}{Units?.Rain ?? "mm"}";
        public string Snowfall => $"{Current.Snowfall:F1}{Units?.Snowfall ?? "cm"}";
        public string WindSpeed => $"{Current.WindSpeed:F1}{Units?.WindSpeed ?? "km/h"}";
        public string WindGusts => $"{Current.WindGusts:F1}{Units?.WindGusts ?? "km/h"}";
        public string Pressure => $"{Current.SurfacePressure:F1}{Units?.SurfacePressure ?? "hPa"}";
        public string CloudCover => $"{Current.CloudCover}%";
        public string UvIndex => $"{Current.UvIndex:F1}";
        public string Visibility => $"{Current.Visibility / 1000:F1} km";
        public bool IsDay => Current.IsDay == 1;

        public string WindDirection => GetWindDirection(Current.WindDirection);
        public string WeatherDescription => GetWeatherDescription(Current.WeatherCode);
        public string WeatherIcon => GetWeatherIcon(Current.WeatherCode, IsDay);

        private static string GetWindDirection(int degrees)
        {
            // Normalisiere den Wert auf 0-359 Grad
            degrees = ((degrees % 360) + 360) % 360;

            return degrees switch
            {
                >= 0 and < 12 => "N",
                >= 12 and < 34 => "NNO",
                >= 34 and < 56 => "NO",
                >= 56 and < 78 => "ONO",
                >= 78 and < 102 => "O",
                >= 102 and < 124 => "OSO",
                >= 124 and < 146 => "SO",
                >= 146 and < 168 => "SSO",
                >= 168 and < 192 => "S",
                >= 192 and < 214 => "SSW",
                >= 214 and < 236 => "SW",
                >= 236 and < 258 => "WSW",
                >= 258 and < 282 => "W",
                >= 282 and < 304 => "WNW",
                >= 304 and < 326 => "NW",
                >= 326 and < 348 => "NNW",
                >= 348 => "N" // Explizit für 348-359
            };
        }

        private static string GetWeatherDescription(int code) => code switch
        {
            0 => "Klar",
            1 => "Überwiegend klar",
            2 => "Teilweise bewölkt",
            3 => "Bewölkt",
            45 => "Nebel",
            48 => "Reifnebel",
            51 => "Leichter Nieselregen",
            53 => "Mässiger Nieselregen",
            55 => "Starker Nieselregen",
            56 => "Leichter gefrierender Nieselregen",
            57 => "Starker gefrierender Nieselregen",
            61 => "Leichter Regen",
            63 => "Mässiger Regen",
            65 => "Starker Regen",
            66 => "Leichter gefrierender Regen",
            67 => "Starker gefrierender Regen",
            71 => "Leichter Schneefall",
            73 => "Mässiger Schneefall",
            75 => "Starker Schneefall",
            77 => "Schneekörner",
            80 => "Leichte Regenschauer",
            81 => "Mässige Regenschauer",
            82 => "Starke Regenschauer",
            85 => "Leichte Schneeschauer",
            86 => "Starke Schneeschauer",
            95 => "Gewitter",
            96 => "Gewitter mit leichtem Hagel",
            99 => "Gewitter mit starkem Hagel",
            _ => "Unbekannt"
        };

        private static string GetWeatherIcon(int code, bool isDay) => code switch
        {
            0 => isDay ? "☀️" : "🌙",
            1 => isDay ? "🌤️" : "🌙",
            2 => "⛅",
            3 => "☁️",
            45 or 48 => "🌫️",
            51 or 53 or 55 => "🌦️",
            56 or 57 => "🌨️",
            61 or 63 or 65 => "🌧️",
            66 or 67 => "🌨️",
            71 or 73 or 75 or 77 => "❄️",
            80 or 81 or 82 => "🌦️",
            85 or 86 => "🌨️",
            95 or 96 or 99 => "⛈️",
            _ => "❓"
        };
    }
}
