namespace AgriEnergyConnect.wwwroot.js
{
    public class api
    {

        /**
 * AgriEnergyConnect API Service
 * Handles all authenticated API communication
 */

        const API_BASE_URL = '/api';

// Main authenticated fetch wrapper
async function authFetch(endpoint, options = {}) {
        const url = `${API_BASE_URL}${endpoint}`;
        const token = localStorage.getItem('authToken');
        const refreshToken = localStorage.getItem('refreshToken');

        const headers = {
            'Content-Type': 'application/json',
            ...options.headers
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        try {
            let response = await fetch(url, {
                ...options,
                headers,
                credentials: 'include'
            });

            // Token expired - attempt refresh
            if (response.status === 401 && refreshToken) {
                const newToken = await attemptTokenRefresh();
                if (newToken) {
                    headers['Authorization'] = `Bearer ${newToken}`;
                    response = await fetch(url, {
                        ...options,
                        headers,
                        credentials: 'include'
                    });
                } else {
                    handleUnauthorized();
                    return null;
                }
            }

            // Handle other error statuses
            if (!response.ok) {
                await handleApiError(response);
                return null;
            }

            return response;

        } catch (error) {
            console.error('API request failed:', error);
            showToast('Network error - please try again', 'error');
            return null;
        }
    }

    // Token refresh flow
    async function attemptTokenRefresh() {
        try {
            const response = await fetch(`${API_BASE_URL}/auth/refresh`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    refreshToken: localStorage.getItem('refreshToken')
                })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('authToken', data.token);
                return data.token;
            }
        } catch (error) {
            console.error('Token refresh failed:', error);
        }
        return null;
    }

    // Error handling
    async function handleApiError(response) {
        const error = await response.json().catch(() => ({
            message: `HTTP ${response.status} - ${response.statusText}`
        }));

        console.error('API Error:', error);

        if (response.status === 403) {
            showToast('You do not have permission for this action', 'error');
        } else if (response.status === 404) {
            showToast('Requested resource not found', 'error');
        } else {
            showToast(error.message || 'An unexpected error occurred', 'error');
        }

        if (response.status === 401) {
            handleUnauthorized();
        }
    }

    // Redirect to login when unauthorized
    function handleUnauthorized() {
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        window.location.href = '/account/login?sessionExpired=true';
    }

    // Helper function for common methods
    const api = {
        get: (endpoint) => authFetch(endpoint),
        post: (endpoint, body) => authFetch(endpoint, {
            method: 'POST',
            body: JSON.stringify(body)
        }),
        put: (endpoint, body) => authFetch(endpoint, {
            method: 'PUT',
            body: JSON.stringify(body)
        }),
        delete: (endpoint) => authFetch(endpoint, {
            method: 'DELETE'
        }),
        upload: (endpoint, formData) => authFetch(endpoint, {
            method: 'POST',
            headers: {},
            body: formData
        })
    };

    // UI Helper (can be customized)
    function showToast(message, type = 'info') {
        // Implement your toast notification system
        console.log(`${type.toUpperCase()}: ${message}`);
        // Example: toastify.js, Bootstrap toast, etc.
    }

    export default api;
    }
}
