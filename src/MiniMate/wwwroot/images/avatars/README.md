# Avatar Images Directory

This directory contains avatar images for the clothing recommendation system.

## Required Image Files

Place the following image files in this directory:

### Extreme Cold Conditions
- `extreme-winter.jpg` - For extreme cold with snow (< -10°C with snow)
- `extreme-cold.jpg` - For extreme cold without snow (< -10°C)

### Cold Conditions
- `snow-day.jpg` - For heavy snow (< 0°C with heavy snow > 2mm)
- `winter-snow.jpg` - For light snow (< 0°C with snow)
- `winter.jpg` - For cold weather (< 0°C)

### Cool Conditions
- `rainy-cold.jpg` - For heavy rain and cool (0-10°C with heavy rain > 5mm)
- `rainy-cool.jpg` - For light rain and cool (0-10°C with rain)
- `windy-cool.jpg` - For windy and cool (0-10°C with wind > 30 km/h)
- `cool.jpg` - For cool weather (0-10°C)

### Mild Conditions
- `mild-rainy.jpg` - For mild with rain (10-15°C with rain)
- `mild.jpg` - For mild weather (10-15°C)

### Warm Conditions
- `warm-rainy.jpg` - For warm with rain (15-20°C with rain)
- `warm.jpg` - For warm weather (15-20°C)

### Hot Conditions
- `thunderstorm.jpg` - For hot with thunderstorm (≥ 25°C with weather code 95-99)
- `hot-rainy.jpg` - For hot with rain (≥ 25°C with rain)
- `very-hot.jpg` - For very hot weather (≥ 30°C)
- `hot.jpg` - For hot weather (20-30°C)

### Default
- `default.jpg` - Fallback image for unknown conditions

## Image Specifications

### Recommended Format
- **Format**: JPG, PNG, or WebP
- **Dimensions**: 400x400 px (square) or 16:9 aspect ratio
- **File Size**: Keep under 200 KB per image for fast loading
- **Style**: Child-friendly, colorful, and clear

### Image Guidelines
- Use bright, appealing colors that kids will enjoy
- Consider using illustrations or friendly characters
- Ensure images clearly represent the weather condition
- Maintain consistent style across all images
- Optimize images for web (compress before adding)

## Usage in Code

These images are referenced in `AvatarMapper.cs`:

```csharp
AvatarMapper.GetAvatarPath(WeatherDescription.ExtremeColdWithSnow)
// Returns: "images/avatars/extreme-winter.jpg"
```

## Adding/Updating Images

1. Place image files directly in this directory
2. Use exact filenames as listed above
3. Refresh browser cache after updating images (Ctrl+F5)
4. Test in development mode: `dotnet run` from `src/MiniMate/`

## Placeholder Images

Until you add real images, the application will attempt to load these paths.
You may see broken image icons if the files don't exist yet.

Consider using placeholder services temporarily:
- https://placehold.co/400x400/png?text=Winter
- https://via.placeholder.com/400x400.jpg?text=Hot+Weather
