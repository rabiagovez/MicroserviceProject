// Function to update notification count
async function updateNotificationCount() {
    try {
        const response = await fetch('/Notification/GetUnreadCount');
        const data = await response.json();
        const count = data.count;
        
        // Update notification count in desktop menu
        const desktopNotificationBadge = document.querySelector('.nav-link[href*="Notification"] .notification-dot');
        if (desktopNotificationBadge) {
            desktopNotificationBadge.style.display = count > 0 ? 'block' : 'none';
        }
        
        // Update notification count in mobile menu
        const mobileNotificationBadge = document.querySelector('#mobileMenu .nav-link[href*="Notification"] .bg-red-500');
        if (mobileNotificationBadge) {
            mobileNotificationBadge.style.display = count > 0 ? 'block' : 'none';
        }
    } catch (error) {
        console.error('Error updating notification count:', error);
    }
}

// Function to mark notification as read
async function markNotificationAsRead(id) {
    try {
        const response = await fetch(`/Notification/MarkAsRead/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        
        if (response.ok) {
            // Update the UI to show the notification as read
            const notificationElement = document.querySelector(`[data-notification-id="${id}"]`);
            if (notificationElement) {
                notificationElement.classList.remove('bg-blue-50');
                notificationElement.classList.add('bg-gray-50');
            }
            
            // Update notification count
            await updateNotificationCount();
        }
    } catch (error) {
        console.error('Error marking notification as read:', error);
    }
}

// Function to show a toast notification
function showToastNotification(message) {
    const toast = document.createElement('div');
    toast.className = 'fixed bottom-4 right-4 bg-white rounded-lg shadow-lg p-4 border border-gray-200 transform transition-all duration-300 translate-y-0 opacity-100';
    toast.innerHTML = `
        <div class="flex items-center space-x-3">
            <div class="flex-shrink-0">
                <div class="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
                    <i class="fas fa-bell text-blue-600"></i>
                </div>
            </div>
            <div class="flex-1">
                <p class="text-sm font-medium text-gray-900">${message}</p>
            </div>
            <button onclick="this.parentElement.parentElement.remove()" class="text-gray-400 hover:text-gray-500">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
    
    document.body.appendChild(toast);
    
    // Remove toast after 5 seconds
    setTimeout(() => {
        toast.classList.add('translate-y-2', 'opacity-0');
        setTimeout(() => toast.remove(), 300);
    }, 5000);
}

// Initialize notification system
document.addEventListener('DOMContentLoaded', () => {
    // Update notification count on page load
    updateNotificationCount();
    
    // Set up polling for new notifications every 30 seconds
    setInterval(updateNotificationCount, 30000);
    
    // Add click handlers for marking notifications as read
    document.querySelectorAll('[data-notification-id]').forEach(element => {
        element.addEventListener('click', () => {
            const id = element.dataset.notificationId;
            markNotificationAsRead(id);
        });
    });
}); 