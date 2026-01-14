window.blazorCulture = {
    get: async () => {
        // In MAUI, use Preferences instead of localStorage
        if (window.DotNet) {
            return await DotNet.invokeMethodAsync('MiniMate.Maui', 'GetPreference', 'BlazorCulture');
        }
        // Fallback to localStorage for web
        return window.localStorage['BlazorCulture'];
    },
    set: async (value) => {
        // In MAUI, use Preferences instead of localStorage
        if (window.DotNet) {
            await DotNet.invokeMethodAsync('MiniMate.Maui', 'SetPreference', 'BlazorCulture', value);
        } else {
            // Fallback to localStorage for web
            window.localStorage['BlazorCulture'] = value;
        }
    }
};
