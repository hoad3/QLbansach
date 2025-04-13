$(document).ready(function() {
    $('.add-to-cart').on('click', function(e) {
        e.preventDefault();
        const bookId = $(this).data('book-id');
        const quantity = parseInt($('#quantity').val()) || 1;

        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: {
                bookId: bookId,
                quantity: quantity
            },
            success: function(response) {
                if (response.success) {
                    alert('Đã thêm sản phẩm vào giỏ hàng');
                    location.reload();
                } else {
                    if (response.message.includes('đăng nhập')) {
                        if (confirm('Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng. Đi đến trang đăng nhập?')) {
                            window.location.href = '/Auth/Login?returnUrl=' + window.location.pathname;
                        }
                    } else {
                        alert(response.message || 'Có lỗi xảy ra khi thêm vào giỏ hàng');
                    }
                }
            },
            error: function() {
                alert('Có lỗi xảy ra khi thêm vào giỏ hàng');
            }
        });
    });
});