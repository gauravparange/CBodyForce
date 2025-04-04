document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-delete]').forEach(button => {
        button.addEventListener('click', function (event) {
            var action = button.getAttribute('data-action');
            var controller = button.getAttribute('data-controller');
            var itemId = button.getAttribute('data-id');

            var form = document.getElementById('deleteForm');
            form.setAttribute('action', `/${controller}/${action}`);

            document.getElementById('deleteItemId').value = itemId;

            var modal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
            modal.show();
        });
    });
});