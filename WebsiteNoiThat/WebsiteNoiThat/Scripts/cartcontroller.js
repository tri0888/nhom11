var cart = {
    init: function () {
        cart.regEvents();
    },

    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        
        $('.txtQuantity').change(function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ProductId: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

        $('#btnDeleteAll').off('click').on('click', function () {


            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

    }
}
cart.init();

// Thêm các hàm xử lý tăng giảm số lượng
function decreaseQuantity(productId) {
    var input = document.querySelector(`.quantity-input[data-id="${productId}"]`);
    var currentValue = parseInt(input.value);
    if (currentValue > 1) {
        input.value = currentValue - 1;
        updateQuantity(input);
    }
}

function increaseQuantity(productId) {
    var input = document.querySelector(`.quantity-input[data-id="${productId}"]`);
    var currentValue = parseInt(input.value);
    input.value = currentValue + 1;
    updateQuantity(input);
}

function updateQuantity(input) {
    var productId = input.getAttribute('data-id');
    var quantity = parseInt(input.value);
    
    // Kiểm tra giá trị hợp lệ
    if (isNaN(quantity) || quantity < 1) {
        quantity = 1;
        input.value = 1;
    }

    // Gọi API cập nhật số lượng
    $.ajax({
        url: '/Cart/UpdateQuantity',
        data: { productId: productId, quantity: quantity },
        type: 'POST',
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        }
    });
}

function updateTotalPrice(input) {
    const quantity = parseInt(input.value);
    const price = parseFloat(input.getAttribute('data-price'));
    const discount = parseFloat(input.getAttribute('data-discount')) || 0;
    
    let finalPrice;
    if (discount > 0) {
        finalPrice = (price - (price * discount * 0.01)) * quantity;
    } else {
        finalPrice = price * quantity;
    }
    
    // Tìm phần tử hiển thị tổng tiền gần nhất
    const row = input.closest('tr');
    const totalPriceElement = row.querySelector('.total-price');
    
    // Cập nhật hiển thị tổng tiền
    totalPriceElement.innerHTML = finalPrice.toLocaleString('vi-VN') + '₫';
}
