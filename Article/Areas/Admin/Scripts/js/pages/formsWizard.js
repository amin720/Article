/*
 *  Document   : formsWizard.js
 *  Author     : pixelcave
 *  Description: Custom javascript code used in Forms Wizard page
 */

var FormsWizard = function() {

    return {
        init: function() {
            /*
             *  Jquery Wizard, Check out more examples and documentation at http://www.thecodemine.org
             *  Jquery Validation, Check out more examples and documentation at https://github.com/jzaefferer/jquery-validation
             */

            /* Set default wizard options */
            var wizardOptions = {
                focusFirstInput: true,
                disableUIStyles: true,
                inDuration: 0,
                outDuration: 0
            };

            /* Initialize Clickable Wizard */
            var clickableWizard = $('#clickable-wizard');

            clickableWizard.formwizard(wizardOptions);

            $('.clickable-steps a').on('click', function() {
                var gotostep = $(this).data('gotostep');

                clickableWizard.formwizard('show', gotostep);
            });

            /* Initialize Progress Wizard */
            $('#progress-wizard').formwizard(wizardOptions);

            // Get the progress bar and change its width when a step is shown
            var progressBar = $('#progress-bar-wizard');
            progressBar
                .css('width', '33%')
                .attr('aria-valuenow', '33');

            $("#progress-wizard").bind('step_shown', function(event, data) {
                if (data.currentStep === 'progress-first') {
                    progressBar
                        .css('width', '33%')
                        .attr('aria-valuenow', '33')
                        .removeClass('progress-bar-info progress-bar-success')
                        .addClass('progress-bar-danger');
                } else if (data.currentStep === 'progress-second') {
                    progressBar
                        .css('width', '66%')
                        .attr('aria-valuenow', '66')
                        .removeClass('progress-bar-danger progress-bar-success')
                        .addClass('progress-bar-info');
                } else if (data.currentStep === 'progress-third') {
                    progressBar
                        .css('width', '100%')
                        .attr('aria-valuenow', '100')
                        .removeClass('progress-bar-danger progress-bar-info')
                        .addClass('progress-bar-success');
                }
            });

            /* Initialize Validation Wizard */
            $('#validation-wizard').formwizard({
                disableUIStyles: true,
                validationEnabled: true,
                validationOptions: {
                    errorClass: 'help-block animation-slideDown', // You can change the animation class for a different entrance animation - check animations page
                    errorElement: 'span',
                    errorPlacement: function(error, e) {
                        e.parents('.form-group > div').append(error);
                    },
                    highlight: function(e) {
                        $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                        $(e).closest('.help-block').remove();
                    },
                    success: function(e) {
                        // You can use the following if you would like to highlight with green color the input after successful validation!
                        e.closest('.form-group').removeClass('has-success has-error'); // e.closest('.form-group').removeClass('has-success has-error').addClass('has-success');
                        e.closest('.help-block').remove();
                    },
                    rules: {
                        'example-validation-username': {
                            required: true,
                            minlength: 2
                        },
                        'example-validation-password': {
                            required: true,
                            minlength: 5
                        },
                        'example-validation-confirm-password': {
                            required: true,
                            equalTo: '#example-validation-password'
                        },
                        'example-validation-email': {
                            required: true,
                            email: true
                        },
                        'example-validation-terms': {
                            required: true
                        }
                    },
                    messages: {
                        'example-validation-username': {
                            required: 'لطفا یک نام کاربری وارد کنید',
                            minlength: 'نام کاربری شما بایستی حداقل شامل 2 کاراکتر باشد'
                        },
                        'example-validation-password': {
                            required: 'لطفا کلمه عبور امن انتخاب کنید',
                            minlength: 'حداقل کلمه عبور بایستی 5 کاراکتر باشد'
                        },
                        'example-validation-confirm-password': {
                            required: 'لطفا کلمه عبور امن انتخاب کنید',
                            minlength: 'حداقل کلمه عبور بایستی 5 کاراکتر باشد',
                            equalTo: 'لطفا کلمه عبور بالا عینا تکرار کنید'
                        },
                        'example-validation-email': 'لطفا یک پست الکترونیکی مناسب وارد کنید',
                        'example-validation-terms': 'لطفا قوانین و ضوابط را تایید کنید!'
                    }
                },
                inDuration: 0,
                outDuration: 0
            });
        }
    };
}();