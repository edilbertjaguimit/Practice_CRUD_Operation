$().ready(function () {
    $('#btnLogin').click(function () {
        const email = $('#txtEmail').val();
        const password = $('#txtPassword').val();

        if (email != '' && password != '') {
            if (isEmail(email.trim())) {
                console.log('LoggedIn');
                $.ajax({
                    async: true,
                    url: '/UserManagement/LoginCredentials', // Specify the URL of your controller action 
                    type: 'POST', // HTTP method (GET, POST, PUT, DELETE, etc.)
                    dataType: 'json', // Expected data type from the server
                    data: { email: email, password: password }, // Data to be sent to the server
                    success: function (response) {
                        // Handle the successful response from the server
                        console.log('Response:', response);
                        if (response[0].success === 1) {
                            console.log('Logged in successfully');
                            $.notify("Logged In Successfully", "success");
                            setInterval(function () {
                                window.location.reload();
                            }, 3000);
                        }
                        console.log(response[0].success);
                    },
                    error: function (xhr, status, error) {
                        // Handle errors that occur during the AJAX request
                        console.error('Error:', error);
                    }
                });
            }
        }
    });
    const isEmail = (email) => /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(email);
});

setTimeout(function () {
    document.getElementById('alertMessage').classList.add('none');
}, 25000);