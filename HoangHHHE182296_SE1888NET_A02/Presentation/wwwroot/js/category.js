const MODAL_TYPES_CONST = {
    create: {
        name: "Create",
        title: "Create A New Category",
        sumbmitText: "Create"
    },
    update: {
        name: "Update",
        title: "Edit Category Information",
        sumbmitText: "Save"
    },
    delete: {
        name: "Delete",
        title: "Delete Category",
        sumbmitText: "OK"
    }
}

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    $('#search-form').on('submit', function (e) {
        e.preventDefault();
        const form = $(this);
        const query = form.serialize();

        const url = '/Category/Index?' + query;
        history.pushState(null, null, url);

        const container = $('#category-table-container');
        container.html('<div class="text-center p-4"><div class="spinner-border text-primary" role="status"></div><p class="mt-2">Loading...</p></div>');

        $.ajax({
            url: '/Category/Index?handler=SearchCategory&' + query,
            type: 'GET',
            success: function (html) {
                container.html(html);
            },
            error: function () {
                container.html('<div class="alert alert-danger">Failed to load category list.</div>');
            }
        });
    });

    $('#search-form').trigger('submit');

    $('#reset-btn').on('click', function (e) {
        e.preventDefault();
        $('#search-form')[0].reset();
        $('#search-form').trigger('submit');
    });

});