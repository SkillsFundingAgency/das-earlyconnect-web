// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var orderedCheckboxes = document.querySelectorAll('.ordered-checkbox');
    var toggledCheckboxes = document.querySelectorAll('.toggled-checkbox');

    orderedCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            toggledCheckboxes.forEach(function (toggledCheckbox) {
                toggledCheckbox.checked = false;
            });
        });
    });

    toggledCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            orderedCheckboxes.forEach(function (orderedCheckbox) {
                orderedCheckbox.checked = false;
            });
        });
    });
});
