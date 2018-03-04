/*
 *  Document   : readyRegister.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Register page
 */

var ReadyRegister = function() {

    return {
        init: function() {
            /*
             *  Jquery Validation, Check out more examples and documentation at https://github.com/jzaefferer/jquery-validation
             */

            /* Register form - Initialize Validation */
            $('#form-register').validate({
                errorClass: 'help-block animation-slideUp', // You can change the animation class for a different entrance animation - check animations page
                errorElement: 'div',
                errorPlacement: function(error, e) {
                    e.parents('.form-group > div').append(error);
                },
                highlight: function(e) {
                    $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                    $(e).closest('.help-block').remove();
                },
                success: function(e) {
                    if (e.closest('.form-group').find('.help-block').length === 2) {
                        e.closest('.help-block').remove();
                    } else {
                        e.closest('.form-group').removeClass('has-success has-error');
                        e.closest('.help-block').remove();
                    }
                },
                rules: {
                    'register-username': {
                        required: true,
                        minlength: 3
                    },
                    'register-email': {
                        required: true,
                        email: true
                    },
                    'register-password': {
                        required: true,
                        minlength: 5
                    },
                    'register-password-verify': {
                        required: true,
                        equalTo: '#register-password'
                    },
                    'register-terms': {
                        required: true
                    }
                },
                messages: {
                    'register-username': {
                        required: 'لطفا یک نام کاربری وارد کنید',
                        minlength: 'لطفا یک نام کاربری وارد کنید'
                    },
                    'register-email': 'لطفا یک پست الکترونیکی مناسب وارد کنید',
                    'register-password': {
                        required: 'لطفا کلمه عبور امن انتخاب کنید',
                        minlength: 'حداقل کلمه عبور بایستی 5 کاراکتر باشد'
                    },
                    'register-password-verify': {
                        required: 'لطفا کلمه عبور امن انتخاب کنید',
                        minlength: 'حداقل کلمه عبور بایستی 5 کاراکتر باشد',
                        equalTo: 'لطفا کلمه عبور بالا عینا تکرار کنید'
                    },
                    'register-terms': {
                        required: 'لطفا قوانین و ضوابط را تایید کنید!'
                    }
                }
            });
        }
    };
}();