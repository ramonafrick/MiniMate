// Geolocation API wrapper for Blazor
window.getCurrentPosition = () => {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject(new Error('Geolocation is not supported by this browser.'));
            return;
        }

        const options = {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 600000 // 10 minutes cache
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
                        errorMessage = 'Location access was denied. Please allow access in your browser settings.';
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = 'Location is not available. Please check your internet connection or GPS settings.';
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
};

// Check if geolocation is available
window.isGeolocationAvailable = () => {
    return 'geolocation' in navigator;
};