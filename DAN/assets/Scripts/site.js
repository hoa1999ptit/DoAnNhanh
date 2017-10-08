$(document).ready(function () {
    loadNewProduct(0);
    loadMostView(0);
});
$(function () {
    $(this).scroll(function () {
        if ($(this).scrollTop() >= 100) {
            $('#up-arrow').css("display", "block");
        } else {
            $('#up-arrow').css("display", "none");
        }
    });
    $('#up-arrow').click(function () {
        $('body,html').animate({
            scrollTop: 0
        });
    });
    $('#select-mate option').click(function () {
        location.href = $('#select-mate').val();
    });
    $(document).on('click', '.close, #over', function () {
        closeCart();
    });
});
function showCart() {
    if (window.innerWidth >= 700) {
        $.ajax({
            url: "/Cart/ShowCart",
            success: function (result) {
                closeCart();
                $('body').append('<div id="over">');
                $('body').append(result);
            }
        });
    } else {
        location.href = '/Cart';
    }
}
function addCart(id, soLuong) {
    $.ajax({
        url: "/Cart/AddCart",
        data: {itemId : id, soLuong : soLuong},
        success: function (result) {
        }
    });
}
function removeCart(id) {
    $.ajax({
        url: "/Cart/RemoveCart",
        data: { itemId: id },
        success: function (result) {
            showCart();
        }
    });
}
function closeCart() {
    $('div#over').remove();
    $('div.cartPartial').remove();
}
function loadNewProduct(i) {
    $.ajax({
        url: "/Home/NewProduct",
        data: { page: i },
        success: function (result) {
            $("#new-product").html(result);
        }
    });
}
function loadMostView(i) {
    $.ajax({
        url: "/Home/MostView",
        data: {page : i},
        success: function (result) {
            $("#most-view").html(result);
        }
    });
}
function loadCategory(i) {
    $.ajax({
        url: "/Category",
        data: { itemId : $('div.title').attr("id"),  page : i },
        success: function (result) {
            $("#catgory").html(result);
        }
    });
}