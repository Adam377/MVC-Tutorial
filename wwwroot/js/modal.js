$('document').ready(function () {
    if (document.cookie.indexOf('popup-cookie') === -1) {
        var url = $('#userModal').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                $('#userContainer').html(res);
                $('#userModal').modal('show');
            }
        })
    }
})

function closeModal() {
    console.log("Close modal");

    setCookie();

    var url = $('#userModal').data('url');

    $.ajax({
        url: url,
        type: 'GET',
        success: function (res) {
            $('#userContainer').html(res);
            $('#userModal').modal('hide');
        }
    })
}

function setCookie() {
    var expireTime = new Date();
    expireTime.setMinutes(expireTime.getMinutes() + 60);
    document.cookie = "name=popup-cookie;expires=" + expireTime;
}