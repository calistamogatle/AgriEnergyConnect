
    // site.js - Custom JavaScript for your Razor Pages project

    // Example: Highlight table rows on click
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.table-row').forEach(function (row) {
            row.addEventListener('click', function () {
                // Toggle highlight class
                this.classList.toggle('highlight');
                // Example: Show an alert (remove in production)
                // alert('Row clicked!');
            });
        });
    });

    // Example: Form validation (basic)
    document.addEventListener('DOMContentLoaded', function () {
        var forms = document.querySelectorAll('.needs-validation');
        Array.prototype.slice.call(forms).forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    });
//});