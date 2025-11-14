function addToCart(productId) {
    $.ajax({
        url: '/Cart/AddToCart',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ productId: productId }),
        success: function (response) {

            if (response.success) {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Product added to cart',
                    showConfirmButton: false,
                    timer: 2000
                });

                $("#cartCount").text(response.cartCount);
            }
            else {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'error',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        }
    });
}
