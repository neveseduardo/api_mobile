document.addEventListener('DOMContentLoaded', () => {
    function toggleSidebar() {
        const wrapper = document.getElementById('wrapper');
        wrapper.classList.toggle('sidebar-open');
        wrapper.classList.toggle('sidebar-closed');

        const isSidebarOpen = wrapper.classList.contains('sidebar-open');
        localStorage.setItem('sidebarOpen', isSidebarOpen);
    }

    function loadSidebarState() {
        const wrapper = document.getElementById('wrapper');
        const isSidebarOpen = localStorage.getItem('sidebarOpen') === 'true';

        if (isSidebarOpen) {
            wrapper.classList.add('sidebar-open');
            wrapper.classList.remove('sidebar-closed');
        } else {
            wrapper.classList.remove('sidebar-open');
            wrapper.classList.add('sidebar-closed');
        }
    }

    document.getElementById('toggle-sidebar').addEventListener('click', toggleSidebar);

    document.addEventListener('DOMContentLoaded', loadSidebarState);

    document.getElementById('user-menu').addEventListener('click', function () {
        const dropdown = document.getElementById('dropdown-menu');
        dropdown.classList.toggle('hidden');
    });

    document.addEventListener('click', function (event) {
        const dropdown = document.getElementById('dropdown-menu');
        const userMenu = document.getElementById('user-menu');
        if (!userMenu.contains(event.target) && !dropdown.contains(event.target)) {
            dropdown.classList.add('hidden');
        }
    });
});