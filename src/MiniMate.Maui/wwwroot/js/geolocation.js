// Geolocation API wrapper for Blazor (MAUI version)
window.getCurrentPosition = async () => {
    console.log('getCurrentPosition called (MAUI version)');

    try {
        // Check if we're running in MAUI
        if (window.DotNet) {
            console.log('Using MAUI native geolocation');
            const result = await DotNet.invokeMethodAsync('MiniMate.Maui', 'GetCurrentPositionAsync');
            console.log('Position acquired successfully via MAUI:', result);
            return result;
        }

        // Fallback to browser geolocation (shouldn't happen in MAUI)
        console.log('DotNet not available, falling back to browser geolocation');
        if (!navigator.geolocation) {
            throw new Error('Geolocation is not supported');
        }

        return new Promise((resolve, reject) => {
            const options = {
                enableHighAccuracy: false,
                timeout: 15000,
                maximumAge: 60000
            };

            navigator.geolocation.getCurrentPosition(
                position => {
                    resolve({
                        coords: {
                            latitude: position.coords.latitude,
                            longitude: position.coords.longitude,
                            accuracy: position.coords.accuracy
                        },
                        timestamp: position.timestamp
                    });
                },
                error => {
                    let errorMessage = 'Unknown error while determining location.';
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            errorMessage = 'Location access was denied. Please allow access in your settings.';
                            break;
                        case error.POSITION_UNAVAILABLE:
                            errorMessage = 'Location is not available. Please check your GPS settings.';
                            break;
                        case error.TIMEOUT:
                            errorMessage = 'Timeout while determining location. Please try again.';
                            break;
                    }
                    reject(new Error(errorMessage));
                },
                options
            );
        });
    } catch (error) {
        console.error('Geolocation error:', error);
        throw error;
    }
};

// Check if geolocation is available
window.isGeolocationAvailable = async () => {
    // In MAUI, always return true since we use native APIs
    if (window.DotNet) {
        return await DotNet.invokeMethodAsync('MiniMate.Maui', 'IsGeolocationAvailableAsync');
    }
    return 'geolocation' in navigator;
};

// Get approximate location from IP address (fallback for desktop PCs)
window.getLocationFromIP = async () => {
    console.log('Getting location from IP address...');

    try {
        // Using ip-api.com (free, no API key required)
        const response = await fetch('http://ip-api.com/json/?fields=status,message,lat,lon,city,country');
        const data = await response.json();

        console.log('IP geolocation response:', data);

        if (data.status === 'success') {
            return {
                coords: {
                    latitude: data.lat,
                    longitude: data.lon,
                    accuracy: 10000 // IP-based location is approximate (~10km accuracy)
                },
                timestamp: Date.now(),
                city: data.city,
                country: data.country,
                isApproximate: true
            };
        } else {
            throw new Error(data.message || 'Failed to get location from IP');
        }
    } catch (error) {
        console.error('IP geolocation error:', error);
        throw error;
    }
};