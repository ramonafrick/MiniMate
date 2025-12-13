// Geolocation API wrapper for Blazor
window.getCurrentPosition = () => {
    return new Promise((resolve, reject) => {
        console.log('getCurrentPosition called');

        if (!navigator.geolocation) {
            console.error('Geolocation is not supported by this browser');
            reject(new Error('Geolocation is not supported by this browser.'));
            return;
        }

        console.log('Geolocation is available, requesting position...');

        const options = {
            enableHighAccuracy: false, // Use network location (faster, works indoors)
            timeout: 15000, // 15 seconds timeout
            maximumAge: 60000 // Cache for 1 minute
        };

        console.log('Geolocation options:', options);

        navigator.geolocation.getCurrentPosition(
            position => {
                console.log('Position acquired successfully:', position);
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
                console.error('Geolocation error:', error);
                console.error('Error code:', error.code);
                console.error('Error message:', error.message);

                let errorMessage = 'Unknown error while determining location.';

                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = 'Location access was denied. Please allow access in your browser settings.';
                        console.error('PERMISSION_DENIED');
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = 'Location is not available. Please check your internet connection or GPS settings.';
                        console.error('POSITION_UNAVAILABLE');
                        break;
                    case error.TIMEOUT:
                        errorMessage = 'Timeout while determining location. Please try again.';
                        console.error('TIMEOUT after 30 seconds');
                        break;
                }

                reject(new Error(errorMessage));
            },
            options
        );
    });
};

// Check if geolocation is available
window.isGeolocationAvailable = () => {
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