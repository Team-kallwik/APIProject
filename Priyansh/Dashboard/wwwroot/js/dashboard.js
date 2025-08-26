document.addEventListener('DOMContentLoaded', () => {
    // Dynamically load the Lucide script
    const lucideScript = document.createElement('script');
    lucideScript.src = "https://unpkg.com/lucide@latest";
    lucideScript.onload = () => {
        lucide.createIcons();
    };
    document.head.appendChild(lucideScript);

    // Sidebar drawer functionality
    const drawerToggle = document.getElementById('dashboard-drawer-toggle');
    const drawerContent = document.getElementById('dashboard-drawer-content');
    const drawerArrow = document.getElementById('drawer-arrow');

    if (drawerToggle && drawerContent && drawerArrow) {
        drawerToggle.addEventListener('click', () => {
            drawerContent.classList.toggle('hidden');
            drawerArrow.classList.toggle('rotate-90');
        });
    }

    // Theme switching functionality
    const lightThemeButton = document.getElementById('light-theme-btn');
    const darkThemeButton = document.getElementById('dark-theme-btn');

    const applyTheme = (theme) => {
        if (theme === 'dark') {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
        localStorage.setItem('theme', theme);
    };

    if (lightThemeButton && darkThemeButton) {
        lightThemeButton.addEventListener('click', () => {
            applyTheme('light');
        });
        darkThemeButton.addEventListener('click', () => {
            applyTheme('dark');
        });
    }

    // Check for saved theme preference and apply it on page load
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        applyTheme(savedTheme);
    } else {
        // Default to light theme if no preference is found
        applyTheme('light');
    }

    // Mobile menu toggle functionality
    const menuBtn = document.getElementById('menu-btn');
    const sidebar = document.getElementById('sidebar');

    // Check if elements exist before adding event listeners to prevent errors
    if (menuBtn && sidebar) {
        menuBtn.addEventListener('click', () => {
            sidebar.classList.toggle('-translate-x-full');
        });
    }

    // Search functionality
    const searchInput = document.getElementById('search-input');
    const orderTable = document.getElementById('recent-orders-table');

    // Add a debounce function to limit API calls while typing
    const debounce = (func, delay) => {
        let timeout;
        return (...args) => {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), delay);
        };
    };

    if (searchInput && orderTable) {
        searchInput.addEventListener('keyup', debounce(async (event) => {
            const query = event.target.value;
            const url = `/Dashboard/SearchOrders?query=${query}`;

            try {
                const response = await fetch(url);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const html = await response.text();

                // Replace the old table body with the new one.
                const newTbody = document.createElement('tbody');
                newTbody.innerHTML = html;
                orderTable.replaceChild(newTbody, orderTable.querySelector('tbody'));

            } catch (error) {
                console.error('Search error:', error);
            }
        }, 300));
    }
});
