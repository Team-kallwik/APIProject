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

    // Add a debounce function to limit API calls while typing
    const debounce = (func, delay) => {
        let timeout;
        return (...args) => {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), delay);
        };
    };
});
