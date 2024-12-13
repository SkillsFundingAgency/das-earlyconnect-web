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

    var submitButton = document.querySelector('button[type="submit"]');
    if (submitButton) {
        submitButton.addEventListener('click', function () {
            var currentForm = document.querySelector('form');
            if (currentForm) {
                this.disabled = 'disabled';
                currentForm.submit();
            }
        });
    }

    var schoolsubmitButton = document.getElementById('searchschoolsubmit');
    var jsEnabledField = document.getElementById('isJsEnabled');
    if (schoolsubmitButton) {
        schoolsubmitButton.textContent = 'Continue';
    }

    var schoolmanualentry = document.getElementById("schoolmanualentry");
    if (schoolmanualentry) {
        schoolmanualentry.style.display = "block";
    }

    if (jsEnabledField) {
        jsEnabledField.value = "true";
    }

    var schoolInput = document.getElementById('schoolsearchterm');
    var selectedUrn = document.getElementById('selectedUrn');

    if (schoolInput) {
        schoolInput.addEventListener('input', function () {
            selectedUrn.value = "";
        });
    }
});

const schoolInputs = document.querySelectorAll(".school-search-autocomplete");
const apiUrl = "/educationalOrganisations/search";
let fullResults = [];

schoolInputs.forEach(input => {
    const container = document.createElement("div");
    container.className = "das-autocomplete-wrap";
    container.dataset.trackUserSelected = input.dataset.trackUserSelected;
    input.parentNode.replaceChild(container, input);

    const getSuggestions = async (query, updateResults) => {
        const xhr = new XMLHttpRequest();
        xhr.onreadystatechange = () => {
            if (xhr.readyState === 4 && xhr.status === 200) {
                fullResults = JSON.parse(xhr.responseText);

                console.log(fullResults);

                const suggestions = fullResults.map(r => {
                    const addressParts = [];

                    if (r.addressLine1) addressParts.push(r.addressLine1);
                    if (r.town) addressParts.push(r.town);
                    if (r.postCode) addressParts.push(r.postCode);

                    const address = addressParts.length > 0 ? ` (${addressParts.join(', ')})` : '';

                    return {
                        label: `${r.name}${address}`,
                        name: r.name,
                        urn: r.urn
                    };
                });

                updateResults(suggestions);
            }
        };

        const lepCode = document.getElementById('lepCode').value;
        xhr.open("GET", `${apiUrl}?searchTerm=${query}&lepCode=${lepCode}`, true);
        xhr.send();
    };

    accessibleAutocomplete({
        element: container,
        id: input.id,
        name: input.name,
        defaultValue: input.value,
        displayMenu: "overlay",
        showNoOptionsFound: false,
        minLength: 2,
        source: getSuggestions,
        templates: {
            inputValue: suggestion => typeof suggestion === 'string' ? suggestion : suggestion?.name || '',
            suggestion: suggestion => typeof suggestion === 'string' ? input.value : suggestion?.label || ''
        },
        placeholder: "",
        confirmOnBlur: false,
        autoselect: true,
        onConfirm: selectedItem => {
            const selectedUrn = document.getElementById('selectedUrn');
            if (selectedUrn && selectedItem && selectedItem.urn) {
                selectedUrn.value = selectedItem.urn;
            }
        }
    });
});

document.querySelectorAll(".autocomplete__input").forEach(input => {
    input.setAttribute("autocomplete", "new-password");
});
