$(document).ready(function() {
    $('.quantity-input').on('change', function() {
        const bookId = $(this).data('book-id');
        const quantity = $(this).val();
        updateQuantity(bookId, quantity);
    });

    $('.increase-quantity').on('click', function() {
        const bookId = $(this).data('book-id');
        const input = $(this).siblings('.quantity-input');
        const newQuantity = parseInt(input.val()) + 1;
        input.val(newQuantity);
        updateQuantity(bookId, newQuantity);
    });

    $('.decrease-quantity').on('click', function() {
        const bookId = $(this).data('book-id');
        const input = $(this).siblings('.quantity-input');
        const newQuantity = Math.max(1, parseInt(input.val()) - 1);
        input.val(newQuantity);
        updateQuantity(bookId, newQuantity);
    });

    $('.remove-item').on('click', function() {
        const bookId = $(this).data('book-id');
        if (confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?')) {
            removeItem(bookId);
        }
    });

    $('#clear-cart').on('click', function() {
        if (confirm('Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?')) {
            clearCart();
        }
    });

    function updateQuantity(bookId, quantity) {
        $.post('/Cart/UpdateQuantity', { bookId, quantity })
            .done(function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            })
            .fail(function() {
                alert('Có lỗi xảy ra khi cập nhật số lượng');
            });
    }

    function removeItem(bookId) {
        $.post('/Cart/RemoveItem', { bookId })
            .done(function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            })
            .fail(function() {
                alert('Có lỗi xảy ra khi xóa sản phẩm');
            });
    }

    function clearCart() {
        $.post('/Cart/ClearCart')
            .done(function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            })
            .fail(function() {
                alert('Có lỗi xảy ra khi xóa giỏ hàng');
            });
    }
});