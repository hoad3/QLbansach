$(document).ready(function () {
    $('#switchToLogin').click(function (e) {
        e.preventDefault();
        $('#login-tab').tab('show');
    });

    $('form[asp-action="Register"]').submit(function (e) {
        let isValid = true;

        if ($('#hoTen').val().trim() === '') {
            $('#hoTen').addClass('is-invalid');
            $('#hoTenError').removeClass('d-none');
            isValid = false;
        } else {
            $('#hoTen').removeClass('is-invalid');
            $('#hoTenError').addClass('d-none');
        }

        if ($('#taiKhoan').val().trim() === '') {
            $('#taiKhoan').addClass('is-invalid');
            $('#taiKhoanError').removeClass('d-none');
            isValid = false;
        } else {
            $('#taiKhoan').removeClass('is-invalid');
            $('#taiKhoanError').addClass('d-none');
        }

        if ($('#email').val().trim() === '') {
            $('#email').addClass('is-invalid');
            $('#emailError').removeClass('d-none');
            isValid = false;
        } else {
            $('#email').removeClass('is-invalid');
            $('#emailError').addClass('d-none');
        }

        if ($('#password').val().trim() === '') {
            $('#password').addClass('is-invalid');
            $('#passwordError').removeClass('d-none');
            isValid = false;
        } else if ($('#password').val().length < 3) {
            $('#password').addClass('is-invalid');
            $('#passwordError').text('Mật khẩu phải có ít nhất 3 ký tự');
            $('#passwordError').removeClass('d-none');
            isValid = false;
        } else {
            $('#password').removeClass('is-invalid');
            $('#passwordError').addClass('d-none');
        }

        if ($('#repeatPassword').val().trim() === '') {
            $('#repeatPassword').addClass('is-invalid');
            $('#repeatPasswordError').removeClass('d-none');
            isValid = false;
        } else if ($('#repeatPassword').val() !== $('#password').val()) {
            $('#repeatPassword').addClass('is-invalid');
            $('#repeatPasswordError').text('Mật khẩu và phần xác nhận không khớp với nhau');
            $('#repeatPasswordError').removeClass('d-none');
            isValid = false;
        } else {
            $('#repeatPassword').removeClass('is-invalid');
            $('#repeatPasswordError').addClass('d-none');
        }

        if ($('#ngaySinh').val() === '') {
            $('#ngaySinh').addClass('is-invalid');
            $('#ngaySinhError').removeClass('d-none');
            isValid = false;
        } else {
            $('#ngaySinh').removeClass('is-invalid');
            $('#ngaySinhError').addClass('d-none');
        }

        if (!isValid) {
            e.preventDefault();
        }
    });

    $('form[asp-action="Login"]').submit(function (e) {
        let isValid = true;

        if ($('#loginEmail').val().trim() === '') {
            $('#loginEmail').addClass('is-invalid');
            $('#loginEmail').next('.invalid-feedback').removeClass('d-none');
            isValid = false;
        } else {
            $('#loginEmail').removeClass('is-invalid');
            $('#loginEmail').next('.invalid-feedback').addClass('d-none');
        }

        if ($('#loginPassword').val().trim() === '') {
            $('#loginPassword').addClass('is-invalid');
            $('#loginPassword').next('.invalid-feedback').removeClass('d-none');
            isValid = false;
        } else if ($('#loginPassword').val().length < 3) {
            $('#loginPassword').addClass('is-invalid');
            $('#loginPassword').next('.invalid-feedback').text('Mật khẩu phải có ít nhất 3 ký tự');
            $('#loginPassword').next('.invalid-feedback').removeClass('d-none');
            isValid = false;
        } else {
            $('#loginPassword').removeClass('is-invalid');
            $('#loginPassword').next('.invalid-feedback').addClass('d-none');
        }

        if (!isValid) {
            e.preventDefault();
        }
    });
});