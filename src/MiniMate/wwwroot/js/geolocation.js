// Geolocation API wrapper for Blazor
window.getCurrentPosition = () => {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject(new Error('Geolocation wird von diesem Browser nicht unterstützt.'));
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
                let errorMessage = 'Unbekannter Fehler beim Ermitteln des Standorts.';

                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = 'Standortzugriff wurde verweigert. Bitte erlauben Sie den Zugriff in den Browser-Einstellungen.';
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = 'Standort ist nicht verfügbar. Bitte überprüfen Sie Ihre Internetverbindung oder GPS-Einstellungen.';
                        break;
                    case error.TIMEOUT:
                        errorMessage = 'Zeitüberschreitung beim Ermitteln des Standorts. Bitte versuchen Sie es erneut.';
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